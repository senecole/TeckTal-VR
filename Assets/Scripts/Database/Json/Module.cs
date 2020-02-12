using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tecktal
{
    [System.Serializable]
    public class Module
    {
        public string thumbnail;
        public string description;
        public string Attachment;
        public string Duration;
        public string video;
        public string Cost;
        public string skillpath_id;
        public string Type;
        public string Language;
        public string url_thumbnail;
        public string name;
        public string ID;
        public string quizzes;
        public string Added_Time;
        public string skillmodule;
        public string URL_Video;
        [SerializeField]
        Quiz _quiz;
        public Quiz quiz
        {
            get
            {
                return _quiz;
            }
        }

        public void LoadQuiz()
        {
            string str = "{\"quizzes\":[" + quizzes + "]}";
            Debug.Log("Load: " + str);
            _quiz = JsonUtility.FromJson<Quiz>(str);
        }

        public void Set(Button button)
        {
            Text[] labels = button.GetComponentsInChildren<Text>();
            labels[0].text = name;
            Image[] imgs = button.GetComponentsInChildren<Image>();
            imgs[1].enabled = true;
            labels[1].text = Added_Time;
        }
    }
}
