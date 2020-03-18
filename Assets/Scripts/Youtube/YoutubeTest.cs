using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tecktal
{

    public class YoutubeTest : MonoBehaviour
    {
        [SerializeField]
        VideoPlayer360 vp;

        private void Start()
        {
            vp.Play("https://www.youtube.com/watch?v=cXbtWX26VDw");
        }
    }
}