using System;

namespace Evereal.YoutubeDLPlayer
{
  public class Executable
  {
    public static string ytdl
    {
      get
      {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        return "youtube-dl.exe";
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        return "youtube-dl";
#elif UNITY_ANDROID && !UNITY_EDITOR
        return "youtube_dl.zip";
#else
        return "";
#endif
      }
    }
  }

  public class Path
  {
    public static string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public static string ytdlFolder
    {
      get
      {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        return myDocumentsPath + "/Evereal/YoutubeDL/Windows/";
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        return myDocumentsPath + "/Evereal/YoutubeDL/macOS/";
#else
        return "";
#endif
      }
    }

    public static string ytdlPath
    {
      get
      {
        return ytdlFolder + Executable.ytdl;
      }
    }
  }
}