using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tecktal
{

    public class SearchBar : MonoBehaviour
    {
        public VideoList[] videoLists;
        InputField input;
        string lastText = "";

        private void Awake()
        {
            input = GetComponent<InputField>();
        }

        private void Update()
        {
            if(lastText != input.text)
            {
                lastText = input.text;
                for(int i = 0; i < videoLists.Length; i++)
                {
                    videoLists[i].Filter(lastText);
                }
            }
        }
    }
}