using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Evereal.YoutubeDLPlayer;

namespace Tecktal
{

    public class VideoPlayer360:MonoBehaviour 
    {
        public GameObject spherePrefab;
        GameObject currentSphere;
        static VideoPlayer360 instance;
        VideoPlayer vp;
        public bool exitOnError = true;
        [SerializeField]
        Module module;
        public GameObject[] screens;
        YTDLCore ytdlCore;
        [SerializeField]
        bool youtubeTest = false;

        public static VideoPlayer360 GetInstance()
        {
            return instance;
        }

        private void Awake()
        {
            ytdlCore = GetComponent <YTDLCore>();
            instance = this;
        }

        public void Play(string url, Module module = null)
        {
            this.module = module;
            if(youtubeTest && Application.isEditor)
               url = "https://www.youtube.com/watch?v=cXbtWX26VDw";
            Debug.Log("Play Video 360 " + url);
            if(spherePrefab != null)
            {
                Debug.Log("Create Sphere");
                if(currentSphere != null)
                    Destroy(currentSphere);
                currentSphere = Instantiate(spherePrefab, spherePrefab.transform.position, spherePrefab.transform.rotation);
                currentSphere.SetActive(true);
                if (IsYoutube(url))
                {
                    YoutubePlay(url);
                }
                else
                {
                    SetVideo(url);
                }
                QuizManager qm = currentSphere.GetComponentInChildren<QuizManager>();
                if(qm != null)
                {
                    qm.Set(module);
                }
            }
        }

        bool IsYoutube(string url)
        {
            if (ytdlCore == null)
                return false;
            return url.Contains("youtube.com") || url.Contains("youtu.be");
        }

        void SetVideo(string url)
        {
            vp = currentSphere.GetComponent<VideoPlayer>();
            vp.url = url;
            vp.Play();
            // vp.started += OnStart; 
            vp.loopPointReached += OnEnd;
            vp.errorReceived += OnError;
        }

        void YoutubePlay(string url)
        {
            ytdlCore.parseCompleted += ParseCompleted;
            ytdlCore.SetOptions("--format best[protocol=https]");
            StartCoroutine(ytdlCore.PrepareAndParse(url));
        }

        private void ParseCompleted(Evereal.YoutubeDLPlayer.VideoInfo video)
        {
            Debug.Log("youtune title: " + video.title);
            Debug.Log("youtube url: " + video.url);
            SetVideo(video.url);
        }

        void OnStart(VideoPlayer source)
        {
            vp.loopPointReached += OnEnd;
        }

        void OnError(VideoPlayer source, string msg)
        {
            if ((exitOnError || !Application.isEditor ) && currentSphere != null)
                Destroy(currentSphere);
        }

        void OnEnd(VideoPlayer source)
        {
            Debug.Log("On End of The Video");
            //if(currentSphere != null)
              //  Destroy (currentSphere);
        }

        private void Update()
        {
            for(int i = 0; i < screens.Length; i++)
            {
                screens[i].SetActive(currentSphere == null);
            }
        }
    }
}