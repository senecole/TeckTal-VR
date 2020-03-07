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
        public GameObject play;
        public GameObject pause;

        private void Start()
        {
            videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.loopPointReached += OnEnd;
        }

        void OnEnd(VideoPlayer source)
        {
            Debug.Log("On End of The Video");
            play.SetActive(true);
            pause.SetActive(false);
        }
        
        public void Home()
        {
            Destroy(gameObject);
        }

        public void Pause()
        {
            Debug.Log("Pause");
            videoPlayer.Pause();
        }

        public void Play()
        {
            Debug.Log("Play");
            //videoPlayer.playbackSpeed = 1;
            videoPlayer.Play();
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
