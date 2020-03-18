namespace Evereal.YoutubeDLPlayer
{
  /// <summary>
  /// Invoked when the <c>VideoPlayer</c> reaches the end of the content to play.
  /// </summary>
  /// <param name="instance">The <c>YTDLPlayer</c> source that is emitting the event.</param>
  public delegate void LoopPointReachedEvent(YTDLPlayer instance);
}
