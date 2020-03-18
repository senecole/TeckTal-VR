namespace Evereal.YoutubeDLPlayer
{
  /// <summary>
  /// Indicates the error code of <c>YTDLCore</c> module.
  /// </summary>
  public enum ErrorCode
  {
    FETCH_RELEASE_API_FAILED,
    UPDATE_LIB_FAILED,
    GRANT_LIB_PERMISSION_FAILED,
    PARSE_FAILED,
    PARSE_EXCEPTION,
    PLAY_VIDEO_FAILED,
    ANDROID_PLUGIN_ERROR,
  }

  /// <summary>
  /// Indicates the processing status of <c>YTDLCore</c> module.
  /// </summary>
  public enum ProcessingStatus
  {
    READY,
    UPDATING,
    PARSING,
    ERROR,
  }

  // public enum QualityType
  // {
  //   Auto, // Decide video quality automatically.
  //   Highest, // Select the highest video quality.
  //   Lowest, // Select the lowest video quality.
  // }
}