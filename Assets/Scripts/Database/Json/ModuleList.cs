using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tecktal
{

    [System.Serializable]
    public class ModuleList 
    {
        public Module[] skillmodule;

        public void LoadQuiz()
        {
            for(int i = 0; i < skillmodule.Length; i++)
            {
                skillmodule[i].LoadQuiz();
            }
        }
    }
}