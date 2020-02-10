using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tecktal
{
    public static class Tools 
    {

        public static IEnumerator SetImage(string url, Image img)
        {
            Debug.Log("Set Image " + url + " at " + img.name);
            WWW www = new WWW(url);
            yield return www;
            Debug.Log("Success Loading Image " + url);
            img.overrideSprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
            www.Dispose();
            www = null;
        }
    }
}
