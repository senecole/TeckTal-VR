using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Evereal.YoutubeDLPlayer
{
  public class YTDLPlayer : MonoBehaviour
  {
    [Tooltip("The online video url, i.e https://www.youtube.com/watch?v=DDsRfbfnC_A")]
    public string url;
    [Tooltip("Whether the video will start playing back as soon as the component awakes.")]
    public bool playOnAwake = true;
    [Tooltip("Determines whether the VideoPlayer will wait for the first frame to be loaded into the texture before starting playback when Video.VideoPlayer.playOnAwake is on.")]
    public bool waitForFirstFrame = false;
    [Tooltip("Determines whether the VideoPlayer restarts from the beginning when it reaches the end of the clip.")]
    public bool loop = false;

    // Youtube-DL instance
    private YTDLCore ytdlCore;
    // Unity video player instance
    private VideoPlayer videoPlayer;
    // Parsed video info
    private VideoInfo videoInfo;
    // Available video format can be play
    public List<VideoFormat> availableVideoFormat { get; private set; }

    // If the youtube-dl already parse the video url
    public bool isParsed { get; private set; }
    // Should play video after parse video url
    private bool playAfterParse = true;

    // Invoked when <c>YTDLCore</c> parse started.
    public event ParseStartedEvent parseStarted = delegate { };
    // Invoked when <c>YTDLCore</c> parse completed.
    public event ParseCompletedEvent parseCompleted = delegate { };
    // Invoked when the <c>VideoPlayer</c> preparation is complete.
    public event PrepareCompletedEvent prepareCompleted = delegate { };
    // Invoked when the <c>VideoPlayer</c> clock is synced back to its <c>VideoTimeReference</c>.
    public event ClockResyncOccurredEvent clockResyncOccurred = delegate { };
    // Invoke after a seek operation completes.
    public event SeekCompletedEvent seekCompleted = delegate { };
    // Invoked immediately after Play is called.
    public event StartedEvent started = delegate { };
    // Invoked when the <c>VideoPlayer</c> reaches the end of the content to play.
    public event LoopPointReachedEvent loopPointReached = delegate { };
    // Invoked when a new frame is ready.
    public event FrameReadyEvent frameReady = delegate { };
    // Invoked when error occurred, such as HTTP connection problems are reported through this callback.
    public event ErrorReceivedEvent errorReceived = delegate { };

    // Log message format template
    private string LOG_FORMAT = "[YTDLPlayer] {0}";

    public bool isPlaying
    {
      get
      {
        if (videoPlayer) return videoPlayer.isPlaying;
        return false;
      }
    }

    private void Awake()
    {
      // initial youtubedl-core
      ytdlCore = GetComponent<YTDLCore>();
      if (ytdlCore == null)
      {
        Debug.LogErrorFormat(LOG_FORMAT, "YTDLCore component is not found!");
      }
      // bind ytdl core events
      ytdlCore.parseCompleted += ParseCompleted;
      ytdlCore.errorReceived += ErrorReceived;

      videoPlayer = GetComponent<VideoPlayer>();
      if (videoPlayer == null)
      {
        Debug.LogErrorFormat(LOG_FORMAT, "VideoPlayer component is not found!");
      }
      // init video player
      videoPlayer.source = VideoSource.Url;
      videoPlayer.playOnAwake = false;
      videoPlayer.waitForFirstFrame = waitForFirstFrame;
      videoPlayer.isLooping = loop;
      // override video player event
      videoPlayer.started += VideoPlayerStarted;
      videoPlayer.prepareCompleted += VideoPlayerPrepareCompleted;
      videoPlayer.frameReady += VideoPlayerFrameReady;
      videoPlayer.seekCompleted += VideoPlayerSeekCompleted;
      videoPlayer.clockResyncOccurred += VideoPlayerClockResyncOccurred;
      videoPlayer.loopPointReached += VideoPlayerLoopPointReached;
      videoPlayer.errorReceived += VideoPlayerErrorReceived;

      availableVideoFormat = new List<VideoFormat>();
    }

    private void Start()
    {
      if (string.IsNullOrEmpty(url))
      {
        Debug.LogWarningFormat(LOG_FORMAT, "Please provide the video url!");
        return;
      }
      if (playOnAwake)
      {
        Play();
      }
    }

    private void ParseCompleted(VideoInfo info)
    {
      videoInfo = info;

      // extract available video format
      ExtractAvailableFormat();

      // set video player url
      videoPlayer.url = Utils.ValidParsedVideoUrl(videoInfo, videoInfo.url);

      // set video to play then prepare audio to prevent buffering
      videoPlayer.Prepare();

      if (playAfterParse)
      {
        // play video
        videoPlayer.Play();
      }

      // parse completed
      isParsed = true;

      parseCompleted(this, videoInfo);
    }

    private void ErrorReceived(YTDLCore.ErrorEvent error)
    {
      errorReceived(this, error.code);
    }

    private void VideoPlayerPrepareCompleted(VideoPlayer source)
    {
      prepareCompleted(this);
    }

    private void VideoPlayerClockResyncOccurred(VideoPlayer source, double seconds)
    {
      clockResyncOccurred(this, seconds);
    }

    private void VideoPlayerSeekCompleted(VideoPlayer source)
    {
      seekCompleted(this);
    }

    private void VideoPlayerErrorReceived(VideoPlayer source, string message)
    {
      Debug.LogWarningFormat(LOG_FORMAT, message);
      errorReceived(this, ErrorCode.PLAY_VIDEO_FAILED);
    }

    private void VideoPlayerStarted(VideoPlayer source)
    {
      started(this);
    }

    private void VideoPlayerLoopPointReached(VideoPlayer source)
    {
      loopPointReached(this);
    }

    private void VideoPlayerFrameReady(VideoPlayer source, long frameIdx)
    {
      frameReady(this, frameIdx);
    }

    private void ExtractAvailableFormat()
    {
      availableVideoFormat.Clear();
      // parse valid format for unity video player
      foreach (VideoFormat videoFormat in videoInfo.formats)
      {
        // TODO, support other container and protocol
        if (
          videoFormat.ext == "mp4" &&
          videoFormat.acodec != "none" &&
          (videoFormat.protocol == "https" || videoFormat.protocol == "http")
        )
        {
          availableVideoFormat.Add(videoFormat);
        }
      }
    }

    public VideoPlayer GetVideoPlayer()
    {
      return videoPlayer;
    }

    public YTDLCore GetYTDLCore()
    {
      return ytdlCore;
    }

    public VideoInfo GetVideoInfo()
    {
      return videoInfo;
    }

    public double time
    {
      get
      {
        return videoPlayer.time;
      }
      set
      {
        videoPlayer.time = value;
      }
    }

    public long frame
    {
      get
      {
        return videoPlayer.frame;
      }
      set
      {
        videoPlayer.frame = value;
      }
    }

    public ulong frameCount
    {
      get
      {
        return videoPlayer.frameCount;
      }
    }

    public void SetVideoUrl(string url)
    {
      videoPlayer.url = Utils.ValidParsedVideoUrl(videoInfo, url);
    }

    public void Parse(bool playAfterParse)
    {
      this.playAfterParse = playAfterParse;
      StartCoroutine(ytdlCore.PrepareAndParse(url));
      parseStarted(this);
    }

    public bool Prepare()
    {
      if (!isParsed)
      {
        return false;
      }
      videoPlayer.Prepare();
      return true;
    }

    public bool Play()
    {
      if (!isParsed)
      {
        Parse(true);
        return false;
      }
      else
      {
        videoPlayer.Play();
        return true;
      }
    }

    public bool Pause()
    {
      if (!isParsed)
      {
        return false;
      }
      videoPlayer.Pause();
      return true;
    }

    public bool Stop()
    {
      if (!isParsed)
      {
        return false;
      }
      videoPlayer.Stop();
      return true;
    }

    public bool StepForward()
    {
      if (!isParsed)
      {
        return false;
      }
      videoPlayer.StepForward();
      return true;
    }

    public bool GetAudioMute(ushort trackIndex)
    {
      if (videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
      {
        return videoPlayer.GetDirectAudioMute(trackIndex);
      }
      else if (videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
      {
        return videoPlayer.GetTargetAudioSource(trackIndex).mute;
      }
      return false;
    }

    public void SetAudioMute(ushort trackIndex, bool mute)
    {
      if (videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
      {
        videoPlayer.SetDirectAudioMute(trackIndex, mute);
      }
      else if (videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
      {
        videoPlayer.GetTargetAudioSource(trackIndex).mute = mute;
      }
    }

    public float GetAudioVolume(ushort trackIndex)
    {
      if (videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
      {
        return videoPlayer.GetDirectAudioVolume(trackIndex);
      }
      else if (videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
      {
        return videoPlayer.GetTargetAudioSource(trackIndex).volume;
      }
      return 0f;
    }

    public void SetAudioVolume(ushort trackIndex, float volume)
    {
      if (videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
      {
        videoPlayer.SetDirectAudioVolume(trackIndex, volume);
      }
      else if (videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
      {
        videoPlayer.GetTargetAudioSource(trackIndex).volume = volume;
      }
    }
  }
}