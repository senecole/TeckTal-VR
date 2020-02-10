using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tecktal
{
    [System.Serializable]
    public class Skill
    {
        public string thumbnail;
        public string Description;
        public string Language;
        public string url_thumbnail;
        public string user_ids;
        public string name;
        public string ID;
        public string category_ids;

        public void Set(Button button)
        {
            TMPro.TextMeshProUGUI [] labels = button.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            labels[0].text = name;
            Image[] imgs = button.GetComponentsInChildren<Image>();
            imgs[1].enabled = true;
        }
    }
}
