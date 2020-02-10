using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tecktal
{
    [RequireComponent(typeof(TecktalUsersAPI))]
    public class TecktalUsersAPITest : MonoBehaviour
    {
        TecktalUsersAPI api;
        public enum TestType { GetAllUsers, GetUser, Update, Register};
        public TestType testType;
        public string firstName = "Renan";
        public string lastName = "Reis";
        public string email = "renanclaudino@gmail.com";
        public string password = "123456";

        private void Awake()
        {
            api = GetComponent<TecktalUsersAPI>();
            if (testType == TestType.GetAllUsers)
            {
                api.GetUser();
            }else if (testType == TestType.GetUser)
            {
                api.GetUser(email);
            }
            else if(testType == TestType.Register)
            {
                api.RegisterUser(firstName, lastName, email, password);
            }else if(testType == TestType.Update)
            {
                api.UpdateUser(firstName, lastName, email, password);
            }
        }
    }
}