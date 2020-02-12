using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tecktal
{

    public class QuizManager : MonoBehaviour
    {
        [SerializeField]
        Quiz quiz;
        [SerializeField]
        Text[] labels;
        [SerializeField]
        int index;
        [SerializeField]
        Inquiry q;

        public void Set(Quiz quiz)
        {
            this.quiz = quiz;
            gameObject.SetActive(false);
        }

        private void Start()
        {
            labels = GetComponentsInChildren<Text>();
            ShowQuestion();
        }

        public void ShowQuestion()
        {
            labels = GetComponentsInChildren<Text>();
            if (index >= labels.Length || quiz == null || quiz.quizzes == null)
            {
                Exit();
                return;
            }
            gameObject.SetActive(true);
            q = quiz.quizzes[index];
            labels[0].text = q.Question;
            string[] options = new string[] { q.A, q.B, q.C, q.D };
            for(int i = 0; i < 4; i++)
            {
                labels[i + 1].text = options[i];
            }
        }

        void Exit()
        {
            gameObject.SetActive(true);
        }

        public void Anwser(string option)
        {
            if(option == q.Answer)
            {
                Debug.Log("Correct!");
                index++;
                ShowQuestion();
            }
            else
            {
                Debug.Log("Wrong!");
            }
        }
    }
}