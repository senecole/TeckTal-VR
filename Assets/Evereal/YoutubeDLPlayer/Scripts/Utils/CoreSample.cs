using System.Collections.Generic;
using UnityEngine;

namespace Evereal.YoutubeDLPlayer
{
  public class CoreSample : MonoBehaviour
  {
    [Tooltip("The online video URL, i.e https://www.twitch.tv/tfue. If this URL is offline or not available, you can choose a new one from https://www.twitch.tv/.")]
    public string url = "https://www.twitch.tv/tfue";
    [Tooltip("The custom options for youtube-dl")]
    public string options = "--format best[protocol=m3u8]";
    // Youtube-DL instance
    public YTDLCore ytdlCore;
    // Parsed video info
    private VideoInfo videoInfo;
    // Parse error message
    private string errorMsg = null;
    // Available video format can be play
    public List<VideoFormat> availableVideoFormat { get; private set; }

    // Log message format template
    private string LOG_FORMAT = "[CoreSample] {0}";

    private void Awake()
    {
      if (ytdlCore == null)
      {
        Debug.LogErrorFormat(LOG_FORMAT, "YTDLCore component is not found!");
      }
      ytdlCore.parseCompleted += ParseCompleted;
      ytdlCore.errorReceived += ErrorReceived;

      // Set your custom parameters
      ytdlCore.SetOptions(options);
    }

    private void ParseCompleted(VideoInfo info)
    {
      videoInfo = info;

      foreach (var format in info.formats)
      {
        Debug.LogFormat(LOG_FORMAT, string.Format("{0}: {1}", format.format_id, format.url));
      }
    }

    private void ErrorReceived(YTDLCore.ErrorEvent error)
    {
      //Debug.LogErrorFormat(LOG_FORMAT, "Receive error code: " + error.code);
      errorMsg = error.message;
    }

    private void OnGUI()
    {
      GUI.Label(new Rect(10, 10, Screen.width - 10, 20), "URL: " + url);
      if (ytdlCore.status == ProcessingStatus.READY)
      {
        if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 25, 150, 50), "Start Parse"))
        {
          // Start parse
          StartCoroutine(ytdlCore.PrepareAndParse(url));
        }
      }
      else
      {
        if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 25, 150, 50), "Status: " + ytdlCore.status)) { }
      }
      if (videoInfo != null)
      {
        string parsedUrl = videoInfo.url;
        if (parsedUrl.Length > 100)
        {
          parsedUrl = parsedUrl.Substring(0, 100) + "...";
        }
        GUI.Label(new Rect(10, 25, Screen.width - 10, 20), "Parsed URL: " + parsedUrl);
      }
      else if (errorMsg != null)
      {
        GUI.Label(new Rect(10, 25, Screen.width - 10, 20), "Parse Error: " + errorMsg);
      }
    }
  }
}