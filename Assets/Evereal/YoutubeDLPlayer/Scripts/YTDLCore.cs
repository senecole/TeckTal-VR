using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

using Debug = UnityEngine.Debug;

namespace Evereal.YoutubeDLPlayer
{
  public class YTDLCore : MonoBehaviour
  {
    public struct ErrorEvent
    {
      public ErrorEvent(ErrorCode c, String m)
      {
        code = c;
        message = m;
      }
      public ErrorCode code;
      public String message;
    }
    /// <summary>
    /// Invoked when <c>YTDLCore</c> parse completed.
    /// </summary>
    /// <param name="videoInfo">The youtube-dl parsed video info.</param>
    public delegate void ParseCompletedEvent(VideoInfo videoInfo);
    /// <summary>
    /// Invoked when error occurred, such as HTTP connection problems are reported through this callback.
    /// </summary>
    /// <param name="error">Error event contains code and message.</param>
    public delegate void ErrorReceivedEvent(ErrorEvent error);

    // Invoked when parse completed
    public event ParseCompletedEvent parseCompleted = delegate { };
    // Invoked when error occurred
    public event ErrorReceivedEvent errorReceived = delegate { };

    /// <summary>
    /// Get or set the current status.
    /// </summary>
    /// <value>The current status.</value>
    public ProcessingStatus status { get; protected set; }
    /// <summary>
    /// PlayerPrefs key for current youtube-dl version.
    /// </summary>
    private string PREF_VERSION_KEY = "PREF_VERSION_KEY";
    /// <summary>
    /// API request for latest youtube-dl.
    /// </summary>
#if UNITY_ANDROID && !UNITY_EDITOR
    private const string LATEST_RELEASE_API = "https://api.github.com/repos/yausername/youtubedl-lazy/releases/latest";
#else
    private const string LATEST_RELEASE_API = "https://api.github.com/repos/ytdl-org/youtube-dl/releases/latest";
#endif
    /// <summary>
    /// Options settings can be played with Unity <c>VideoPlayer</c>.
    /// </summary>
    private const string UNITY_VIDEO_PLAYER_OPTIONS = "--format [protocol=https][ext=mp4]/[protocol=http][ext=mp4]";
    /// <summary>
    /// Custom parse options for youtube-dl.
    /// See: https://github.com/ytdl-org/youtube-dl#options
    /// </summary>
    private string options = null;
    /// <summary>
    /// Whether the youtube-dl is prepared for parsing.
    /// </summary>
    public bool isPrepared { get; private set; }
    // The youtube-dl parsing thread
    private Thread parsingThread;
    // The youtube-dl update parsing thread
    private Thread updateParsingThread;
    private Queue<VideoInfo> parseCompletedQueue;
    private Queue<ErrorEvent> errorQueue;

    /// <summary>
    /// Android native plugin resources.
    /// </summary>
    private const string ANDROID_PLUGIN = "com.evereal.youtubedl_android.YoutubeDLPlugin";
    private AndroidJavaClass androidPluginClass;
    private AndroidJavaObject androidPluginInstance;
    private AndroidJavaClass androidActivityClass;
    private AndroidJavaObject androidActivityContext;

    // Log message format template
    private string LOG_FORMAT = "[YTDLCore] {0}";

    private void Awake()
    {
      status = ProcessingStatus.READY;

      isPrepared = false;

      parseCompletedQueue = new Queue<VideoInfo>();
      errorQueue = new Queue<ErrorEvent>();

      if (Application.platform == RuntimePlatform.Android)
      {
        if (androidPluginClass == null)
        {
          androidPluginClass = new AndroidJavaClass(ANDROID_PLUGIN);
        }
        if (androidPluginInstance == null)
        {
          androidPluginInstance = androidPluginClass.CallStatic<AndroidJavaObject>("getInstance");
        }
        if (androidActivityClass == null)
        {
          androidActivityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        }
        if (androidActivityContext == null)
        {
          androidActivityContext = androidActivityClass.GetStatic<AndroidJavaObject>("currentActivity");
        }
        androidPluginInstance.Call("init", androidActivityContext);
      }
    }

    private void Update()
    {
      if (parseCompletedQueue.Count > 0)
      {
        VideoInfo videoInfo;
        lock (this) videoInfo = parseCompletedQueue.Dequeue();
        parseCompleted(videoInfo);
      }
      if (errorQueue.Count > 0)
      {
        ErrorEvent error;
        lock (this) error = errorQueue.Dequeue();
        errorReceived(error);
      }
    }

    public void SetOptions(string options)
    {
      this.options = options;
    }

    public IEnumerator PrepareAndParse(string url)
    {
      if (status != ProcessingStatus.READY)
      {
        Debug.LogWarningFormat(LOG_FORMAT, "Previous process not finished yet or there is an error occurred!");
        yield break;
      }
      if (isPrepared)
      {
        // start parse
        ParseAsync(url);
        yield break;
      }
      // fetch ytdl latest version.
      string latestVersion = "";
      string latestDownloadUrl = "";
      status = ProcessingStatus.UPDATING;
      using (UnityWebRequest www = UnityWebRequest.Get(LATEST_RELEASE_API))
      {
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
          errorReceived(new ErrorEvent(ErrorCode.FETCH_RELEASE_API_FAILED, www.error));
          status = ProcessingStatus.ERROR;
          yield break;
        }
        else
        {
          ReleaseInfo releaseInfo = JsonUtility.FromJson<ReleaseInfo>(www.downloadHandler.text);
          latestVersion = releaseInfo.tag_name;
          foreach (ReleaseInfo.AssetInfo asset in releaseInfo.assets)
          {
            if (asset.name == Executable.ytdl)
            {
              latestDownloadUrl = asset.browser_download_url;
            }
          }
        }
      }
      // get current ytdl version
      string version = PlayerPrefs.GetString(PREF_VERSION_KEY);

      if (Application.platform == RuntimePlatform.Android)
      {
        #region ANDROID PREPARE SECTION

        // check ytdl version
        if (string.IsNullOrEmpty(version) || version != latestVersion)
        {
          // update and start parse
          UpdateAndParseAsync(latestVersion, latestDownloadUrl, url);
        }
        else
        {
          isPrepared = true;
          // start parse
          ParseAsync(url);
        }

        #endregion
      }
      else
      {
        #region STANDALONE & EDITOR PREPARE SECTION

        // check ytdl version
        if (string.IsNullOrEmpty(version) || version != latestVersion || !File.Exists(Path.ytdlPath))
        {
          // download ytdl latest version
          using (UnityWebRequest www = UnityWebRequest.Get(latestDownloadUrl))
          {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
              errorReceived(new ErrorEvent(ErrorCode.FETCH_RELEASE_API_FAILED, www.error));
              status = ProcessingStatus.ERROR;
              yield break;
            }
            else
            {
              if (!Directory.Exists(Path.ytdlFolder))
              {
                Directory.CreateDirectory(Path.ytdlFolder);
              }
              System.IO.File.WriteAllBytes(Path.ytdlPath, www.downloadHandler.data);
              Debug.LogFormat(LOG_FORMAT, "Update youtube-dl package success!");
              if (Application.platform == RuntimePlatform.OSXEditor ||
                  Application.platform == RuntimePlatform.OSXPlayer)
              {
                if (CommandProcess.Run("chmod", "a+rx " + "\"" + Path.ytdlPath + "\""))
                {
                  Debug.LogFormat(LOG_FORMAT, "Grant youtube-dl permission success!");
                }
                else
                {
                  errorReceived(new ErrorEvent(ErrorCode.GRANT_LIB_PERMISSION_FAILED, "Grant youtube-dl permission failed!"));
                  yield break;
                }
              }
            }
          }
          Debug.LogFormat(LOG_FORMAT, "Update youtube-dl to latest version " + latestVersion);
          // update pref version
          PlayerPrefs.SetString(PREF_VERSION_KEY, latestVersion);
        }
        isPrepared = true;
        // start parse
        ParseAsync(url);

        #endregion
      }
    }

    private void UpdateAndParseAsync(string version, string downloadUrl, string url)
    {
      if (updateParsingThread != null)
      {
        updateParsingThread.Abort();
      }
      updateParsingThread = new Thread(() => UpdateAndParse(version, downloadUrl, url));
      updateParsingThread.Priority = System.Threading.ThreadPriority.Lowest;
      updateParsingThread.IsBackground = true;
      // update and start parsing thread
      updateParsingThread.Start();
    }

    private void UpdateAndParse(string version, string downloadUrl, string url)
    {
      if (Application.platform == RuntimePlatform.Android)
      {
        #region ANDROID UPDATE & PARSE SECTION

        if (AndroidJNI.AttachCurrentThread() != 0)
        {
          EnqueueError(new ErrorEvent(ErrorCode.ANDROID_PLUGIN_ERROR, "Android attach current thread failed!"));
          return;
        }

        if (!androidPluginInstance.Call<bool>("update", androidActivityContext, downloadUrl))
        {
          EnqueueError(new ErrorEvent(ErrorCode.UPDATE_LIB_FAILED, "Android update youtube-dl lib failed!"));
          return;
        }
        // update pref version
        PlayerPrefs.SetString(PREF_VERSION_KEY, version);

        isPrepared = true;

        status = ProcessingStatus.PARSING;

        if (options == null)
        {
          // default to unity video player option
          options = UNITY_VIDEO_PLAYER_OPTIONS;
        }

        string output = androidPluginInstance.Call<string>("getVideoInfo", url, options);
        VideoInfo info = JsonUtility.FromJson<VideoInfo>(output);
        if (!string.IsNullOrEmpty(info.error))
        {
          EnqueueError(new ErrorEvent(ErrorCode.PARSE_FAILED, info.error));
          return;
        }
        lock (this) parseCompletedQueue.Enqueue(info);

        // done
        status = ProcessingStatus.READY;

        AndroidJNI.DetachCurrentThread();

        #endregion

      }
    }

    private void ParseAsync(string url)
    {
      if (parsingThread != null)
      {
        parsingThread.Abort();
      }
      parsingThread = new Thread(() => Parse(url));
      parsingThread.Priority = System.Threading.ThreadPriority.Lowest;
      parsingThread.IsBackground = true;
      // start parsing thread
      parsingThread.Start();
    }

    private void Parse(string url)
    {
      if (!isPrepared)
      {
        Debug.LogWarningFormat(LOG_FORMAT, "Youtube-dl is not ready for parsing yet!");
        return;
      }
      status = ProcessingStatus.PARSING;

      if (options == null)
      {
        // default to unity video player option
        options = UNITY_VIDEO_PLAYER_OPTIONS;
      }

      if (Application.platform == RuntimePlatform.Android)
      {
        #region ANDROID PARSE SECTION

        if (AndroidJNI.AttachCurrentThread() != 0)
        {
          EnqueueError(new ErrorEvent(ErrorCode.ANDROID_PLUGIN_ERROR, "Android attach current thread failed!"));
          return;
        }

        string output = androidPluginInstance.Call<string>("getVideoInfo", url, options);
        VideoInfo info = JsonUtility.FromJson<VideoInfo>(output);
        if (!string.IsNullOrEmpty(info.error))
        {
          EnqueueError(new ErrorEvent(ErrorCode.PARSE_FAILED, info.error));
          return;
        }
        lock (this) parseCompletedQueue.Enqueue(info);

        // done
        status = ProcessingStatus.READY;

        AndroidJNI.DetachCurrentThread();

        #endregion
      }
      else
      {
        #region STANDALONE & EDITOR PARSE SECTION

        // parse video url
        try
        {
          string arguments = string.Format(" {0} --no-warnings --dump-json {1}", options, url);

          Process process = new Process();
          process.StartInfo.FileName = Path.ytdlPath;
          process.StartInfo.Arguments = arguments;
          process.StartInfo.RedirectStandardOutput = true;
          process.StartInfo.RedirectStandardError = true;
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.UseShellExecute = false;

          process.Start();

          string output = process.StandardOutput.ReadToEnd().Trim();
          string error = process.StandardError.ReadToEnd().Trim();

          process.WaitForExit();
          process.Close();

          if (!string.IsNullOrEmpty(error))
          {
            EnqueueError(new ErrorEvent(ErrorCode.PARSE_FAILED, error));
            return;
          }

          Debug.LogFormat(LOG_FORMAT, "Success getting video resources!");
          VideoInfo info = JsonUtility.FromJson<VideoInfo>(output);

          lock (this) parseCompletedQueue.Enqueue(info);
        }
        catch (Exception e)
        {
          EnqueueError(new ErrorEvent(ErrorCode.PARSE_EXCEPTION, e.Message));
          return;
        }

        // done
        status = ProcessingStatus.READY;

        #endregion
      }
    }

    private void EnqueueError(ErrorEvent error)
    {
      lock (this) errorQueue.Enqueue(error);
      status = ProcessingStatus.ERROR;
    }
  }
}