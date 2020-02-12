using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tecktal
{

    [RequireComponent(typeof(TecktalSkillsAPI))]
    public class SkillMenu : MonoBehaviour
    {

        TecktalSkillsAPI skillAPI;
        [SerializeField]
        SkillList skillList;
        [SerializeField]
        Button[] buttons;
        public bool autoShowFirstSkill = false;

        private void Start()
        {
            skillAPI = GetComponent<TecktalSkillsAPI>();
            skillAPI.GetAllPublishSkillPath(OnSuccess, OnError);
            buttons = GetComponentsInChildren<Button>();
            UpdateList();
            Debug.Log("Start coroutine");
            if(autoShowFirstSkill)
                StartCoroutine(ShowVideos());
        }

        IEnumerator ShowVideos()
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("buttons.lenght = " + buttons.Length);
            if (buttons.Length > 0)
            {
                while (true)
                {
                    Debug.Log("button activeSelf = " + buttons[0].gameObject.activeSelf);
                    if (buttons[0].gameObject.activeSelf)
                    {
                        Debug.Log("Call onClick");
                        buttons[0].onClick.Invoke();
                        break;
                    }
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        void OnSuccess(string text)
        {
            Debug.Log("on success skill menu: " + text);
            skillList = JsonUtility.FromJson<SkillList>(text);
            UpdateList();
        }

        void OnError(string text)
        {

        }

        void UpdateList()
        {
            int btn = 0;
            for (int i = 0; i < skillList.skillpath.Length; i++)
            {
                if (btn >= buttons.Length)
                    break;
                SetButton(buttons[btn], skillList.skillpath[i]);
                btn++;
            }
            for (int i = btn; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }

        void SetButton(Button btn, Skill skill)
        {
            skill.Set(btn);
            Image[] img = btn.GetComponentsInChildren<Image>();
            StartCoroutine(Tools.SetImage(skill.url_thumbnail, img[1]));
            btn.gameObject.SetActive(true);
            SkillButton sb = btn.gameObject.AddComponent<SkillButton>();
            sb.Add(skill);
        }
 
    }
}