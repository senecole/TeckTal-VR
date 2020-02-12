using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tecktal
{

    public class HistoryButton : MonoBehaviour
    {
        [SerializeField]
        Module module;

        public void Add(Module module)
        {
            this.module = module;
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            Debug.Log("On History Button Click");
            VideoPlayer360 vp = VideoPlayer360.GetInstance();
            if (vp != null)
            {
                Debug.Log("Found Video Player 360");
                vp.Play(module.URL_Video, module);
            }
        }
    }
}