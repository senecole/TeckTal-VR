﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

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

        public static VideoPlayer360 GetInstance()
        {
            return instance;
        }

        private void Awake()
        {
            instance = this;
        }

        public void Play(string url, Module module = null)
        {
            this.module = module;
            Debug.Log("Play Video 360 " + url);
            if(spherePrefab != null)
            {
                Debug.Log("Create Sphere");
                if(currentSphere != null)
                    Destroy(currentSphere);
                currentSphere = Instantiate(spherePrefab, spherePrefab.transform.position, spherePrefab.transform.rotation);
                currentSphere.SetActive(true);
                vp = currentSphere.GetComponent<VideoPlayer>();
                vp.url = url;
                vp.Play();
                // vp.started += OnStart; 
                vp.loopPointReached += OnEnd;
                vp.errorReceived += OnError;
                QuizManager qm = currentSphere.GetComponentInChildren<QuizManager>();
                if(qm != null)
                {
                    qm.Set(module);
                }
            }
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