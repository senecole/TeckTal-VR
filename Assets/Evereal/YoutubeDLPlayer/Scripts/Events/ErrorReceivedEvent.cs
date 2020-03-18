namespace Evereal.YoutubeDLPlayer
{
  /// <summary>
  /// Invoked when error occurred, such as HTTP connection problems are reported through this callback.
  /// </summary>
  /// <param name="instance"><c>YTDLPlayer</c> instance.</param>
  /// <param name="error">Error code.</param>
  public delegate void ErrorReceivedEvent(YTDLPlayer instance, ErrorCode error);
}