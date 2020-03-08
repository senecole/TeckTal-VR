using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weelco.VRAuthorization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tecktal
{
    [RequireComponent(typeof(TecktalUsersAPI))]
    public class LoginManager : MonoBehaviour
    {
        public VRAuthInputField password;
        public VRAuthInputField email;
        [SerializeField]
        string pass;
        [SerializeField]
        string userEmail;
        TecktalUsersAPI userAPI;
        [SerializeField]
        UserList users;
        static User loggedUser;
        public GameObject login;
        public GameObject menuScreen;
        public Text errorLabel;
        public bool loginTest = false;

        private void Start()
        {
            userAPI = GetComponent<TecktalUsersAPI>();
            if (loginTest )
            {
                userEmail = "sidyndao@gmail.com";
                pass = "23456";
                userAPI.GetUser(userEmail, OnSuccess, OnError);
                enabled = false;
            }
        }

        private void Update()
        {
            SetCredentials();
        }

        public void TryLogin()
        {
            SetCredentials();
            userAPI.GetUser(userEmail, OnSuccess, OnError);
        }

        void OnSuccess(string text)
        {
            Debug.Log("ON SUCCESS: " + text);
            users = JsonUtility.FromJson<UserList>(text);
            if (users != null && users.user != null && users.user.Length > 0)
            {
                Debug.Log("user is Ok");
                var u = users.user[0];
                if(u.login_code == pass)
                {
                    Debug.Log("password is ok");
                    loggedUser = u;
                    NextScene();
                }
                else
                {
                    Debug.Log("wrong password");
                    ShowError("Email or password is incorrect");
                }
            }
            else
            {
                Debug.Log("null user");
                ShowError("Email or password is incorrect");
            }
        }
        
        public void ShowError(string msg)
        {
            if (errorLabel != null)
            {
                errorLabel.text = msg;
                errorLabel.transform.parent.gameObject.SetActive(true);
            }
        }

        void NextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //login.SetActive(false);
            //menuScreen.SetActive(true);
        }

        void OnError(string text)
        {
            ShowError("Network error, please try again later");
        }

        void SetCredentials()
        {
            pass = password.text;
            userEmail = email.text;
        }

        public static User GetLoggedUser()
        {
            return loggedUser;
        }
    }
}