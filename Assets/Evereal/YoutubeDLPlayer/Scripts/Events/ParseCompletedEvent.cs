namespace Evereal.YoutubeDLPlayer
{
  /// <summary>
  /// Invoked when <c>YTDLCore</c> parse completed.
  /// </summary>
  /// <param name="instance">The <c>YTDLPlayer</c> source that is emitting the event.</param>
  /// <param name="videoInfo">The youtube-dl parsed video info.</param>
  public delegate void ParseCompletedEvent(YTDLPlayer instance, VideoInfo videoInfo);
}
