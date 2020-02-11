using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tecktal
{
    [RequireComponent(typeof(TecktalSkillsAPI))]
    public class ProceedPopup : MonoBehaviour
    {
        TecktalSkillsAPI skillAPI;
        public Text label;
        [SerializeField]
        VideoInfo videoInfo;
        [SerializeField]
        CreditList creditList;
        User user;

        private void Awake()
        {
            skillAPI = GetComponent<TecktalSkillsAPI>();
        }

        public void Set(VideoInfo v)
        {
            gameObject.SetActive(true);
            label.text = v.title + " will cost you " + v.cost;
            videoInfo = v;
        }

        public void TryPurchase()
        {
            Debug.Log("Try Purchase");
            user = LoginManager.GetLoggedUser();
            if (user == null)
            {
                Debug.Log("No User");
                Cancel();
                return;
            }
            Debug.Log("Try Get Credits");
            skillAPI.GetCredits(user.ID, OnCreditSuccess, OnCreditError);
        }

        public void Cancel()
        {
            Debug.Log("Cancel");
            gameObject.SetActive(false);
        }

        void OnCreditSuccess(string text)
        {
            Debug.Log("on success credit list: " + text);
            try
            {
                creditList = JsonUtility.FromJson<CreditList>(text);
                string str = creditList.tecktalon[0].credit;
                Debug.Log("credit = " + str);
                int credit = int.Parse(str);
                Debug.Log("credit number = " + credit);
                str = videoInfo.cost;
                Debug.Log("cost = " + str);
                str = str.Replace("$", "");
                str = str.TrimEnd();
                Debug.Log("cost2 = " + str);
                float cost = float.Parse(str);
                Debug.Log("cost number = " + cost);
                if(cost < credit)
                {
                    //skillAPI.Enroll(videoInfo.ID, user.ID, OnEnrollSuccess, OnEnrollError);
                }
                else
                {
                    NoCredits();
                }
            }
            catch
            {
                UnexpectedError();
            }
        }

        public void OnEnrollSuccess(string msg)
        {
            //TODO
        }

        public void OnEnrollError(string msg)
        {
            //TODO
        }

        void NoCredits()
        {
            //TODO
        }

        void OnCreditError(string text)
        {
            UnexpectedError();
        }

        void UnexpectedError()
        {
            Debug.Log("Unexpected Error");
            //TODO
        }
    }
}