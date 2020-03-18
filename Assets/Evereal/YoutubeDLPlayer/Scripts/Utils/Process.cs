using System;
using System.Diagnostics;

namespace Evereal.YoutubeDLPlayer
{
  public class CommandProcess
  {
    public static bool Run(string procName, string arguments)
    {
      try
      {
        Process process = new Process();
        process.StartInfo.FileName = procName;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.Start();
        process.WaitForExit();
        process.Close();
      }
      catch (Exception e)
      {
        UnityEngine.Debug.LogError(e.Message);
        return false;
      }
      return true;
    }
  }
}