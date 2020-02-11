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

        public static LearningHistory GetInstance()
        {
            return instance;
        }

        public bool Contains(string moduleID)
        {
            if (moduleList == null)
                return false;
            for(int i = 0; i < moduleList.skillmodule.Length; i++)
            {
                if (moduleList.skillmodule[i].ID == moduleID)
                {
                    return true;
                }
            }
            return false;
        }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            user = LoginManager.GetLoggedUser();
            if (user != null)
            {
                skillAPI = GetComponent<TecktalSkillsAPI>();
                skillAPI.GetEnrolled(user.ID, OnSuccess, OnError);
            }
            buttons = GetComponentsInChildren<Button>();
            UpdateList();
        }

        void OnSuccess(string text)
        {
            Debug.Log("on success history menu: " + text);
            moduleList = JsonUtility.FromJson<ModuleList>(text);
            UpdateList();
        }

        void OnError(string text)
        {

        }

        void UpdateList()
        {
            int btn = 0;
            for (int i = 0; i < moduleList.skillmodule.Length; i++)
            {
                if (btn >= buttons.Length)
                    break;
                SetButton(buttons[btn], moduleList.skillmodule[i]);
                btn++;
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
            SkillButton sb = btn.gameObject.AddComponent<SkillButton>();
           // sb.Add(skill);
        }
    }
}
