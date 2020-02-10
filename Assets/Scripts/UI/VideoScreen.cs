using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tecktal
{
    [RequireComponent(typeof(TecktalSkillsAPI))]
    public class VideoScreen : MonoBehaviour
    {
        
        public VideoList featured;
        public VideoList top;
        public VideoList recommended;
        [SerializeField]
        VideoList[] videoLists;
        TecktalSkillsAPI skillAPI;
        static VideoScreen instance;
        Skill lastSkill;
        [SerializeField]
        ModuleList moduleList;

        public static VideoScreen GetInstance()
        {
            return instance;
        }

        void Awake()
        {
            instance = this;
            skillAPI = GetComponent<TecktalSkillsAPI>();
            videoLists = new VideoList[] { featured, top, recommended };
            EmptyVideoLists();
            //gameObject.SetActive(false);
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
                featured.Add(m);
                if(m.Type == "Top")
                {
                    top.Add(m);
                }else if(m.Type == "Recommended")
                {
                    recommended.Add(m);
                }
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
