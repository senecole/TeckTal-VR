using UnityEngine;
using UnityEngine.Video;

namespace Evereal.YoutubeDLPlayer
{
  public class VideoPlayerCtrl : MonoBehaviour
  {
    public VideoTitle videoTitle;
    public VideoTime videoTime;
    public GameObject loadingCircle;
    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject progressCircle;
    public GameObject progressBar;
    public GameObject progressBarBG;
    public GameObject volumeCircle;
    public GameObject volumeBar;
    public GameObject volumeBarBG;
    public QualityButton qualityButton;

    private YTDLPlayer ytdlPlayer;
    private VideoPlayer videoPlayer;
    private Camera mainCamera;

    private int currentFormatIndex = -1;

    private float maxProgressValue;
    private float newProgressX;
    private float maxProgressX;
    private float minProgressX;
    private float progressPosY;
    private float simpleProgressValue;
    private float progressValue;
    private float progressBarWidth;
    private bool isProgressDragging;

    private float maxVolumeValue;
    private float newVolumeX;
    private float maxVolumeX;
    private float minVolumeX;
    private float volumePosY;
    private float simpleVolumeValue;
    private float volumeValue;
    private float volumeBarWidth;

    private bool isVideoJumping = false;
    private bool isVideoPlaying = false;

    private const int VIDEO_TITLE_LENGTH_LIMIT = 36;

    private void Awake()
    {
      ytdlPlayer = GetComponent<YTDLPlayer>();
      ytdlPlayer.parseStarted += ParseStarted;
      ytdlPlayer.started += VideoPlayerStarted;
      ytdlPlayer.prepareCompleted += VideoPlayerPrepareCompleted;

      mainCamera = Camera.main;

      progressPosY = progressCircle.transform.localPosition.y;
      progressBarWidth = progressBarBG.GetComponent<SpriteRenderer>().bounds.size.x;

      volumePosY = volumeCircle.transform.localPosition.y;
      volumeBarWidth = volumeBarBG.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void ParseStarted(YTDLPlayer source)
    {
      loadingCircle.SetActive(true);
    }

    private void VideoPlayerStarted(YTDLPlayer source)
    {
      loadingCircle.SetActive(false);
      isVideoPlaying = true;
      isVideoJumping = false;
      VideoInfo videoInfo = ytdlPlayer.GetVideoInfo();
      // update video title
      string title = videoInfo.title;
      if (title.Length > VIDEO_TITLE_LENGTH_LIMIT)
      {
        title = string.Format("{0}...", title.Substring(0, VIDEO_TITLE_LENGTH_LIMIT - 3));
      }
      videoTitle.SetText(title);
      // update quality button
      if (currentFormatIndex == -1)
      {
        qualityButton.SetText(GenQualityText(videoInfo.format_note, videoInfo.height));
      }
      // toggle play button
      ToggleVideoPlayButton();
    }

    private void VideoPlayerPrepareCompleted(YTDLPlayer source)
    {
      loadingCircle.SetActive(false);
      if (isVideoJumping)
      {
        CalcProgressSimpleValue();
        PlayVideo();
        JumpVideo();
      }
    }

    private void Update()
    {
      if (!isProgressDragging && !isVideoJumping)
      {
        if (ytdlPlayer.frameCount > 0)
        {
          float progress = (float)ytdlPlayer.frame / (float)ytdlPlayer.frameCount;
          progressBar.transform.localScale = new Vector3(progressBarWidth * progress, progressBar.transform.localScale.y, 0);
          progressCircle.transform.localPosition = new Vector2(progressBar.transform.localPosition.x + (progressBarWidth * progress), progressCircle.transform.localPosition.y);
        }
      }
      if (isVideoPlaying && ytdlPlayer.isPlaying)
      {
        double duration = (double)ytdlPlayer.GetVideoInfo().duration;
        string timeString = string.Format("{0} / {1}",
          Utils.GetFormatTimeStringFromSeconds(ytdlPlayer.time),
          Utils.GetFormatTimeStringFromSeconds(duration));
        if (!timeString.Equals(videoTime.GetText()))
        {
          videoTime.SetText(timeString);
        }
      }
    }

    private void OnDestroy() {
      ytdlPlayer.parseStarted -= ParseStarted;
      ytdlPlayer.started -= VideoPlayerStarted;
      ytdlPlayer.prepareCompleted -= VideoPlayerPrepareCompleted;
    }

    public void OnProgressPressDown()
    {
      if (ytdlPlayer.isParsed)
      {
        PauseVideo();
        minProgressX = progressBar.transform.localPosition.x;
        maxProgressX = minProgressX + progressBarWidth;
      }
    }

    public void OnProgressRelease()
    {
      if (ytdlPlayer.isParsed)
      {
        isProgressDragging = false;
        loadingCircle.SetActive(true);
        CalcProgressSimpleValue();
        PlayVideo();
        JumpVideo();
      }
    }

    public void OnProgressDrag()
    {
      if (ytdlPlayer.isParsed)
      {
        isProgressDragging = true;
        isVideoJumping = true;
        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 curPosition = mainCamera.ScreenToWorldPoint(curScreenPoint);
        progressCircle.transform.position = new Vector2(curPosition.x, curPosition.y);
        newProgressX = progressCircle.transform.localPosition.x;
        if (newProgressX > maxProgressX) { newProgressX = maxProgressX; }
        if (newProgressX < minProgressX) { newProgressX = minProgressX; }
        progressCircle.transform.localPosition = new Vector2(newProgressX, progressPosY);
        CalcProgressSimpleValue();
        progressBar.transform.localScale = new Vector3(simpleProgressValue * progressBarWidth, progressBar.transform.localScale.y, 0);
      }
    }

    private void CalcProgressSimpleValue()
    {
      maxProgressValue = maxProgressX - minProgressX;
      progressValue = progressCircle.transform.localPosition.x - minProgressX;
      simpleProgressValue = progressValue / maxProgressValue;
    }

    private void JumpVideo()
    {
      var frame = ytdlPlayer.frameCount * simpleProgressValue;
      ytdlPlayer.frame = (long)frame;
    }

    public void OnVolumePressDown()
    {
      minVolumeX = volumeBar.transform.localPosition.x;
      maxVolumeX = minVolumeX + volumeBarWidth;
    }

    public void OnVolumeRelease()
    {
      CalcVolumeSimpleValue();
    }

    public void OnVolumeDrag()
    {
      float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
      Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
      Vector3 curPosition = mainCamera.ScreenToWorldPoint(curScreenPoint);
      volumeCircle.transform.position = new Vector2(curPosition.x, curPosition.y);
      newVolumeX = volumeCircle.transform.localPosition.x;
      if (newVolumeX > maxVolumeX) { newVolumeX = maxVolumeX; }
      if (newVolumeX < minVolumeX) { newVolumeX = minVolumeX; }
      volumeCircle.transform.localPosition = new Vector2(newVolumeX, volumePosY);
      CalcVolumeSimpleValue();
      volumeBar.transform.localScale = new Vector3(simpleVolumeValue * volumeBarWidth, volumeBar.transform.localScale.y, 0);
    }

    private void CalcVolumeSimpleValue()
    {
      maxVolumeValue = maxVolumeX - minVolumeX;
      volumeValue = volumeCircle.transform.localPosition.x - minVolumeX;
      simpleVolumeValue = volumeValue / maxVolumeValue;
      // set volume value
      ytdlPlayer.SetAudioVolume(0, simpleVolumeValue);
    }

    private void ToggleVideoPlayButton()
    {
      playButton.SetActive(!isVideoPlaying);
      pauseButton.SetActive(isVideoPlaying);
    }

    private string GenQualityText(string formatNote, int height)
    {
      if (string.IsNullOrEmpty(formatNote))
      {
        return string.Format("{0}p", height);
      }
      return formatNote;
    }

    public void SetVideoUrl(string url)
    {
      ytdlPlayer.SetVideoUrl(url);
    }

    public void PrepareVideo()
    {
      ytdlPlayer.Prepare();
    }

    public void ToggleVideo()
    {
      if (isVideoPlaying)
      {
        PauseVideo();
      }
      else
      {
        PlayVideo();
      }
    }

    public void PauseVideo()
    {
      if (ytdlPlayer.Pause())
      {
        isVideoPlaying = false;
        ToggleVideoPlayButton();
      }
    }

    public void PlayVideo()
    {
      if (ytdlPlayer.Play())
      {
        ToggleVideoPlayButton();
      }
    }

    public void StopVideo()
    {
      if (ytdlPlayer.Stop())
      {
        isVideoPlaying = false;
        ToggleVideoPlayButton();
      }
    }

    public void ReplayVideo()
    {
      if (ytdlPlayer.Stop())
      {
        loadingCircle.SetActive(true);
        ytdlPlayer.Play();
      }
    }

    public void VolumeDown()
    {
      float volume = ytdlPlayer.GetAudioVolume(0);
      volume = Mathf.Max(volume - 0.1f, 0f);
      ytdlPlayer.SetAudioVolume(0, volume);
    }

    public void VolumeUp()
    {
      float volume = ytdlPlayer.GetAudioVolume(0);
      volume = Mathf.Min(volume + 0.1f, 1f);
      ytdlPlayer.SetAudioVolume(0, volume);
    }

    public void ToggleQuality()
    {
      if (ytdlPlayer.availableVideoFormat.Count <= 0)
      {
        return;
      }
      loadingCircle.SetActive(true);
      PauseVideo();
      isVideoJumping = true;
      currentFormatIndex = (++currentFormatIndex) % ytdlPlayer.availableVideoFormat.Count;
      VideoFormat videoFormat = ytdlPlayer.availableVideoFormat[currentFormatIndex];
      qualityButton.SetText(GenQualityText(videoFormat.format_note, videoFormat.height));
      SetVideoUrl(videoFormat.url);
      PrepareVideo();
    }
  }
}