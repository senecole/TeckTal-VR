using UnityEngine;

namespace Evereal.YoutubeDLPlayer
{
  public class VideoTitle : MonoBehaviour
  {

    private TextMesh textMesh;
    private string LOG_FORMAT = "[VideoTitle] {0}";

    private void Awake()
    {
      textMesh = GetComponent<TextMesh>();
    }

    public void SetText(string text)
    {
      if (textMesh != null)
      {
        textMesh.text = text;
      }
      else
      {
        Debug.LogWarningFormat(LOG_FORMAT, "TextMesh not attached!");
      }
    }
  }
}
