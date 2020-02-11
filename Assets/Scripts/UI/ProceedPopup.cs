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

        }

       void GetCredits()
       {
            User user = LoginManager.GetLoggedUser();
            if(user == null)
            {
                return;
            }
            skillAPI.GetCredits(user.ID, OnCreditSuccess, OnCreditError);
       }

        void OnCreditSuccess(string text)
        {
            Debug.Log("on success credit list: " + text);
            creditList = JsonUtility.FromJson<CreditList>(text);
        }

        void OnCreditError(string text)
        {

        }
    }
}