namespace Evereal.YoutubeDLPlayer
{
  /// <summary>
  /// Invoked when a new frame is ready.
  /// </summary>
  /// <param name="instance">The <c>YTDLPlayer</c> source that is emitting the event.</param>
  /// <param name="frameIdx">The frame the <c>VideoPlayer</c> is now at.</param>
  public delegate void FrameReadyEvent(YTDLPlayer instance, double frameIdx);
}