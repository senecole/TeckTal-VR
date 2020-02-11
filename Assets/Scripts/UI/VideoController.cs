using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Tecktal
{
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoController : MonoBehaviour
    {

        VideoPlayer videoPlayer;

        private void Start()
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        public void Home()
        {
            Destroy(gameObject);
        }

        public void Reload()
        {
            videoPlayer.Stop();
            StartCoroutine(IPlay());
        }

        IEnumerator IPlay()
        {
            yield return new WaitForSeconds(0.1f);
            videoPlayer.Play();
        }
    }
}
