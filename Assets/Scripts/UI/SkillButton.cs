using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tecktal
{
   
    public class SkillButton : MonoBehaviour
    {
        [SerializeField]
        Skill skill;

        public void Add(Skill skill)
        {
            this.skill = skill;
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            Debug.Log("on click " + name);
            VideoScreen.GetInstance().Set(skill); 
        }
    }
}
