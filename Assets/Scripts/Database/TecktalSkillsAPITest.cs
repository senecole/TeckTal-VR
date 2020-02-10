using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tecktal
{
    [RequireComponent(typeof(TecktalSkillsAPI))]
    public class TecktalSkillsAPITest : MonoBehaviour
    {
        TecktalSkillsAPI api;
        public enum TestType {GetAllSkills, GetAllSkillModule, GetCredits, Enroll, GetEnrolled };
        public TestType testType;
        public string skillPathID;
        public string userID;
        public string skillModuleID;

        private void Awake()
        {
            api = GetComponent<TecktalSkillsAPI>();
            if(testType == TestType.GetAllSkills)
            {
                api.GetAllPublishSkillPath();
            }else if(testType == TestType.GetAllSkillModule)
            {
                api.GetAllSkillModule(skillPathID);
            }else if(testType == TestType.GetCredits)
            {
                api.GetCredits(userID);
            }else if(testType == TestType.Enroll)
            {
                api.Enroll(skillModuleID, userID);
            }else if(testType == TestType.GetEnrolled)
            {
                api.GetEnrolled(userID);
            }
        }
    }
}
