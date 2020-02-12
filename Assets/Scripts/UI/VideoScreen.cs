using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tecktal
{
    [RequireComponent(typeof(TecktalSkillsAPI))]
    public class VideoScreen : MonoBehaviour
    {
        
        public VideoList allVideos;
        public VideoList featured;
        public VideoList popular;
        [SerializeField]
        VideoList[] videoLists;
        TecktalSkillsAPI skillAPI;
        static VideoScreen instance;
        Skill lastSkill;
        [SerializeField]
        ModuleList moduleList;
        public ProceedPopup proceedPopup;
        public bool autoPlayFirstVideo = false;

        public static VideoScreen GetInstance()
        {
            return instance;
        }

        void Awake()
        {
            instance = this;
            skillAPI = GetComponent<TecktalSkillsAPI>();
            videoLists = new VideoList[] { allVideos, featured, popular };
            EmptyVideoLists();
            //gameObject.SetActive(false);
        }

        private void Start()
        {
            skillAPI.GetAllSkillModule("", OnSuccess, OnError);
        }

        public void Set(Skill skill)
        {
           // gameObject.SetActive(true);
            lastSkill = skill;
            skillAPI.GetAllSkillModule(skill.ID, OnSuccess, OnError);
        }

        public void OnSuccess(string text)
        {
            Debug.Log("OnSuccess: " + text);
            moduleList = JsonUtility.FromJson<ModuleList>(text);
            moduleList.LoadQuiz();
            LoadVideos();
            UpdateVideoLists();
        }

        public void OnError(string text)
        {

        }

        void LoadVideos()
        {
            Debug.Log("load videos");
            EmptyVideoLists();
            for(int i = 0; i < moduleList.skillmodule.Length; i++)
            {
                Debug.Log("load module " + i);
                Module m = moduleList.skillmodule[i];
                allVideos.Add(m);
                if(m.Type == "Featured")
                {
                    featured.Add(m);
                }else if(m.Type == "Popular")
                {
                    popular.Add(m);
                }
            }
            if (autoPlayFirstVideo)
            {
                StartCoroutine(IPlayFirstVideo());
            }
        }

        IEnumerator IPlayFirstVideo()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("Play first");
            featured.buttons[0].onClick.Invoke();
        }

        private void Update()
        {
            if (autoPlayFirstVideo && Input.GetMouseButtonDown(0))
            {
                Debug.Log("CLICK");
                featured.buttons[0].onClick.Invoke();
            }
        }

        void EmptyVideoLists()
        {
            foreach (VideoList v in videoLists)
            {
                if (v != null)
                {
                    v.videoInfos = new List<VideoInfo>();
                }
            }
        }

        void UpdateVideoLists()
        {
            foreach (VideoList v in videoLists)
            {
                if (v != null)
                {
                    v.UpdateList();
                }
            }
        }
    }
}
