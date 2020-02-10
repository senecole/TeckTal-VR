using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public string nextScene;
    // Update is called once per frame
     public void NextScene()
    {
        Debug.Log("Next Scene!");
        SceneManager.LoadScene(nextScene);
    }
}
