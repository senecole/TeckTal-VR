using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace Tecktal
{
    public enum RoleType { Customer, Admin};
    public class TecktalUsersAPI : MonoBehaviour
    {
        public const string authtoken = "11cf1cae04da52b793dd51c8b127ba8f";
        const string registerURL = "https://creator.zoho.com/api/zoho_info23208/json/tecktal/form/user/record/add";
        const string updateURL = "https://creator.zoho.com/api/zoho_info23208/json/tecktal/form/user/record/update";
        const string userURL = "https://creator.zoho.com/api/json/tecktal/view/userdb";
        public const string owner = "zoho_info23208";

        public void RegisterUser(string firstName, string lastName, string email, string password, RoleType roleType = RoleType.Customer, Action<string> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(IRegisterUser(firstName, lastName, email, password, roleType, onSuccess, onError));
        }
        
        IEnumerator IRegisterUser(string firstName, string lastName, string email, string password, RoleType roleType = RoleType.Customer, Action<string> onSuccess = null, Action<string> onError = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("authtoken", authtoken);
            form.AddField("scope", "creatorapi");
            form.AddField("name.first_name", firstName);
            form.AddField("name.last_name", lastName);
            form.AddField("email", email);
            form.AddField("login_code", password);
            if(roleType == RoleType.Customer)
                form.AddField("role_type", "Customer");
            else form.AddField("role_type", "Admin");

            UnityWebRequest www = UnityWebRequest.Post(registerURL, form);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if(onError != null)
                {
                    onError(www.error);
                }
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log("Data: " + www.downloadHandler.text);
                if(onSuccess != null)
                {
                    onSuccess(www.downloadHandler.text);
                }
            }
        }

        public void UpdateUser(string firstName, string lastName, string email, string password, Action<string> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(IUpdateUser(firstName, lastName, email, password, onSuccess, onError));
        }

        IEnumerator IUpdateUser(string firstName, string lastName, string email, string password, Action<string> onSuccess = null, Action<string> onError = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("authtoken", authtoken);
            form.AddField("scope", "creatorapi");
            form.AddField("criteria", "(email == \""+email+"\")");
            form.AddField("name.first_name", firstName);
            form.AddField("name.last_name", lastName);
            form.AddField("email", email);
            form.AddField("login_code", password);

            UnityWebRequest www = UnityWebRequest.Post(updateURL, form);
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

        public void GetUser( string email = "", Action<string> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(IGetUser(email, onSuccess, onError));
        }

        IEnumerator IGetUser(string email = "",  Action<string> onSuccess = null, Action<string> onError = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("authtoken", authtoken);
            form.AddField("scope", "creatorapi");
            if(email != "")
            {
                form.AddField("criteria", "(email == \"" + email + "\")");
            }
            form.AddField("zc_ownername", owner);
            form.AddField("raw", "true");

            UnityWebRequest www = UnityWebRequest.Post(userURL, form);
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