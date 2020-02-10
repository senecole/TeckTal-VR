using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tecktal
{
    public class VideoList : MonoBehaviour
    {
        public List<VideoInfo> videoInfos;
        public Button[] buttons;
        public GameObject title;

        private void Awake()
        {
            buttons = GetComponentsInChildren<Button>();
            UpdateList();
        }

        public void UpdateList()
        {
            int btn = 0;
            for(int i = 0; i < videoInfos.Count; i++)
            {
                if (btn >= buttons.Length)
                    break;
                if (videoInfos[i].isActive)
                {
                    videoInfos[i].Set(buttons[btn]);
                    buttons[btn].gameObject.SetActive(true);
                    btn++;
                }
            }
            for(int i = btn; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
            if(title != null)
            {
                title.SetActive(btn > 0);
            }
        }

        public void Filter(string query)
        {
            for(int i = 0; i < videoInfos.Count; i++)
            {
                videoInfos[i].Filter(query);
            }
            UpdateList();
        }

        public void Add(Module m)
        {
            VideoInfo v = new VideoInfo();
            v.title = m.name;
            v.imgURL = m.url_thumbnail;
            v.isActive = true;
            v.videoURL = m.video;
            videoInfos.Add(v);
        }
    }
    
    [System.Serializable]
    public class VideoInfo
    {
        public string title;
        public string imgURL;
        public string videoURL;
        public bool isActive;

        public void Set(Button button)
        {
            Text[] labels = button.GetComponentsInChildren<Text>();
            labels[0].text = title;
            Image[] imgs = button.GetComponentsInChildren<Image>();
            if(imgs.Length > 1 && imgURL != "")
            {
                button.gameObject.SetActive(true);
                button.StartCoroutine(Tools.SetImage(imgURL, imgs[1]));
                imgs[1].color = Color.white;
            }
            button.onClick.RemoveAllListeners();
            if (videoURL != "")
            {
                button.onClick.AddListener(OnClick);
            }
            Debug.Log("set text " + title + " at " + button.gameObject.name);
        }

        public void Filter(string query) {
            string q = query.ToLower();
            string t = title.ToLower();
            isActive = (q == "") || t.Contains(q);
        }

        public void OnClick()
        {
            Debug.Log("On Video Button Click");
            VideoPlayer360 vp = VideoPlayer360.GetInstance();
            if(vp != null)
            {
                Debug.Log("Found Video Player 360");
                vp.Play(videoURL);
            }
        }
    }
}
