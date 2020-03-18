using UnityEngine;

namespace Evereal.YoutubeDLPlayer
{
  public class PlayButton : MonoBehaviour
  {
    public VideoPlayerCtrl videoPlayerCtrl;

    private string LOG_FORMAT = "[PlayButton] {0}";

    void OnMouseUpAsButton()
    {
      if (videoPlayerCtrl != null)
      {
        videoPlayerCtrl.ToggleVideo();
      }
      else
      {
        Debug.LogWarningFormat(LOG_FORMAT, "VideoPlayerCtrl not attached!");
      }
    }
  }
}
