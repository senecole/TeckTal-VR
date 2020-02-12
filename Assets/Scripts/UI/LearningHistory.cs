using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tecktal
{

    [RequireComponent(typeof(TecktalSkillsAPI))]
    public class LearningHistory : MonoBehaviour
    {
        TecktalSkillsAPI skillAPI;
        [SerializeField]
        ModuleList moduleList;
        [SerializeField]
        Button[] buttons;
        [SerializeField]
        User user;
        static LearningHistory instance;
        public bool autoPlayFirstVideo = false;

        public static LearningHistory GetInstance()
        {
            return instance;
        }

        public bool Contains(string moduleName)
        {
            Debug.Log("Check if contains Name " + moduleName);
            if (moduleList == null || moduleList.skillmodule == null)
                return false;
            for(int i = 0; i < moduleList.skillmodule.Length; i++)
            {
                if (moduleList.skillmodule[i].name == moduleName)
                {
                    return true;
                }
            }
            return false;
        }

        private void Awake()
        {
            instance = this;
            buttons = GetComponentsInChildren<Button>();
            skillAPI = GetComponent<TecktalSkillsAPI>();
        }

        private void Start()
        {
            UpdateList();
            Load();
        }

        public void Load()
        {
            Debug.Log("> LearningHistory Load");
            user = LoginManager.GetLoggedUser();
            if (user != null)
            {
                Debug.Log("> USER " + user.name);
                skillAPI.GetEnrolled(user.ID, OnSuccess, OnError);
            }                   
        }

        void OnSuccess(string text)
        {
            text = FixJson(text);
            Debug.Log("> On success history menu: " + text);
            moduleList = JsonUtility.FromJson<ModuleList>(text);
            UpdateList();
            if(autoPlayFirstVideo)
                buttons[0].onClick.Invoke();
        }

        string FixJson(string json)
        {
            json = json.Replace("\"skillmoduleuser\":", "\"skillmodule\":");
            json = json.Replace("\"skillmodule.", "\"");
            return json;
        }

        void OnError(string text)
        {
            Debug.Log("> on error history menu: " + text);
        }

        void UpdateList()
        {
            Debug.Log("> History Menu Update List");
            int btn = 0;
            if (moduleList != null && moduleList.skillmodule != null)
            {
                for (int i = 0; i < moduleList.skillmodule.Length; i++)
                {
                    if (btn >= buttons.Length)
                        break;
                    SetButton(buttons[btn], moduleList.skillmodule[i]);
                    btn++;
                }
            }
            for (int i = btn; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }

        void SetButton(Button btn, Module module)
        {
            module.Set(btn);
            Image[] img = btn.GetComponentsInChildren<Image>();
            StartCoroutine(Tools.SetImage(module.url_thumbnail, img[1]));
            btn.gameObject.SetActive(true);
            HistoryButton hb = btn.gameObject.AddComponent<HistoryButton>();
            hb.Add(module);
        }
    }
}
