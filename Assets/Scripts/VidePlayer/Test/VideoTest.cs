using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Tecktal
{

    public class VideoTest : MonoBehaviour
    {
        VideoPlayer videoPlayer;

        private void Start()
        {
            videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.url = "https://creatorexport.zoho.com/zoho_info23208/tecktal/skillmoduledb/3772331000001494015/video/download/1578398964277_Ayutthaya_-_Easy_Tripod_Paint___360_VR_Master_Series___Free_Download.mp4/SEpCgmFPntsVMD5TqjVAQ38w7AgGHGGK7Q1JS0z1qhXmme9eygDTQOSzyGshHsE5pWMVS4FJGASQeSStzsxbdUvFQwA0B1bHry5K";
            videoPlayer.Play();
            //StartCoroutine(IPlay());
        }

        IEnumerator IPlay()
        {
            yield return new WaitForSeconds(0.5f);
            videoPlayer.url = "https://creatorexport.zoho.com/zoho_info23208/tecktal/skillmoduledb/3772331000001494015/video/download/1578398964277_Ayutthaya_-_Easy_Tripod_Paint___360_VR_Master_Series___Free_Download.mp4/SEpCgmFPntsVMD5TqjVAQ38w7AgGHGGK7Q1JS0z1qhXmme9eygDTQOSzyGshHsE5pWMVS4FJGASQeSStzsxbdUvFQwA0B1bHry5K";
            yield return new WaitForSeconds(2);
            videoPlayer.Play();
        }
    }
}