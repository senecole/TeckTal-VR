namespace Evereal.YoutubeDLPlayer
{
  /// <summary>
  /// Invoked when the <c>VideoPlayer</c> clock is synced back to its VideoTimeReference.
  /// </summary>
  /// <param name="instance">The <c>YTDLPlayer</c> source that is emitting the event.</param>
  /// <param name="seconds">The xc.</param>
  public delegate void ClockResyncOccurredEvent(YTDLPlayer instance, double seconds);
}
