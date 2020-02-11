using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopup : MonoBehaviour
{
    public Text label;
    
    public void Show(string msg)
    {
        label.text = msg;
        gameObject.SetActive(true);
    }
}
