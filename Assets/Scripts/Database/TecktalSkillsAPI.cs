using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

namespace Tecktal
{

    public class TecktalSkillsAPI : MonoBehaviour
    {
        public string authtoken {
            get
            {
                return TecktalUsersAPI.authtoken;
            }
        }
        const string skillPathURL = "https://creator.zoho.com/api/json/tecktal/view/skillpathdb";
        const string skillModuleURL = "https://creator.zoho.com/api/json/tecktal/view/skillmoduledb";
        const string creditsURL = "https://creator.zoho.com/api/json/tecktal/view/tecktalondb";
        const string enrollURL = "https://creator.zoho.com/api/zoho_info23208/json/tecktal/form/skillmoduleuser/record/add";
        const string getEnrolledURL = "https://creator.zoho.com/api/json/tecktal/view/skillmoduleuserdb";
        public string owner
        {
            get
            {
                return TecktalUsersAPI.owner;
            }
        }

        public void GetAllPublishSkillPath(Action<string> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(IGetAllPublishSkillPath(onSuccess, onError));
        }

        public IEnumerator IGetAllPublishSkillPath(Action<string> onSuccess = null, Action<string> onError = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("authtoken", authtoken);
            form.AddField("scope", "creatorapi");
            form.AddField("criteria", "(publish == true)");
            form.AddField("zc_ownername", owner);
            form.AddField("raw", "true");

            UnityWebRequest www = UnityWebRequest.Post(skillPathURL, form);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (onError != null)
                {
                    onError(www.error);
                }
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log("Data: " + www.downloadHandler.text);
                if (onSuccess != null)
                {
                    onSuccess(www.downloadHandler.text);
                }
            }
        }

        public void GetAllSkillModule(string skillPathID, Action<string> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(IGetAllSkillModule(skillPathID, onSuccess, onError));
        }

        IEnumerator IGetAllSkillModule(string skillPathID, Action<string> onSuccess = null, Action<string> onError = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("authtoken", authtoken);
            form.AddField("scope", "creatorapi");
            form.AddField("criteria", "(skillpath_id == "+skillPathID+")");
            form.AddField("zc_ownername", owner);
            form.AddField("raw", "true");

            UnityWebRequest www = UnityWebRequest.Post(skillModuleURL, form);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (onError != null)
                {
                    onError(www.error);
                }
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log("Data: " + www.downloadHandler.text);
                if (onSuccess != null)
                {
                    onSuccess(www.downloadHandler.text);
                }
            }
        }

        public void GetCredits(string userID, Action<string> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(IGetCredits(userID, onSuccess, onError));
        }

        IEnumerator IGetCredits(string userID, Action<string> onSuccess = null, Action<string> onError = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("authtoken", authtoken);
            form.AddField("scope", "creatorapi");
            form.AddField("criteria", "(user_id == " + userID + ")");
            form.AddField("zc_ownername", owner);
            form.AddField("raw", "true");

            UnityWebRequest www = UnityWebRequest.Post(creditsURL, form);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (onError != null)
                {
                    onError(www.error);
                }
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log("Data: " + www.downloadHandler.text);
                if (onSuccess != null)
                {
                    onSuccess(www.downloadHandler.text);
                }
            }
        }

        public void Enroll(string skillModuleID, string userID, Action<string> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(IEnroll(skillModuleID, userID, onSuccess, onError));
        }

        IEnumerator IEnroll(string skillModuleID, string userID, Action<string> onSuccess = null, Action<string> onError = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("authtoken", authtoken);
            form.AddField("skillmodule", skillModuleID);
            form.AddField("status", "Ongoing");
            form.AddField("user_id", userID);
            UnityWebRequest www = UnityWebRequest.Post(enrollURL, form);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (onError != null)
                {
                    onError(www.error);
                }
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log("Data: " + www.downloadHandler.text);
                if (onSuccess != null)
                {
                    onSuccess(www.downloadHandler.text);
                }
            }
        }

        public void GetEnrolled(string userID, Action<string> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(IGetEnrolled(userID, onSuccess, onError));
        }

        IEnumerator IGetEnrolled(string userID, Action<string> onSuccess = null, Action<string> onError = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("authtoken", authtoken);
            form.AddField("scope", "creatorapi");
            form.AddField("criteria", "(user_id == " + userID + ")");
            form.AddField("zc_ownername", owner);
            form.AddField("raw", "true");

            UnityWebRequest www = UnityWebRequest.Post(getEnrolledURL, form);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (onError != null)
                {
                    onError(www.error);
                }
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log("Data: " + www.downloadHandler.text);
                if (onSuccess != null)
                {
                    onSuccess(www.downloadHandler.text);
                }
            }
        }

    }
}