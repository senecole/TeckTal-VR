using System;

namespace Evereal.YoutubeDLPlayer
{
  public class Utils
  {
    public static string GetFormatTimeStringFromSeconds(double seconds)
    {
      TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
      string formatTime = "00:00";
      if (timeSpan.Hours > 0)
      {
        formatTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
        timeSpan.Hours,
        timeSpan.Minutes,
        timeSpan.Seconds);
      }
      else
      {
        formatTime = string.Format("{0:D2}:{1:D2}",
        timeSpan.Minutes,
        timeSpan.Seconds);
      }
      return formatTime;
    }

    public static string ValidParsedVideoUrl(VideoInfo videoInfo, string url)
    {
      if (string.IsNullOrEmpty(url))
        return url;
      if (videoInfo.extractor == "vimeo")
      {
        url = url.Replace("source=1", "");
      }
      return url;
    }
  }
}