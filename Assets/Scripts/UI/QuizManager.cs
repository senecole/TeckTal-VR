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
        [SerializeField]
        Module module;
        [SerializeField]
        bool loadingQuiz = false;
        [SerializeField]
        bool isLoadingNextQuestion = false;

        public void Set(Module module)
        {
            index = 0;
            this.quiz = module.quiz;
            this.module = module;
            gameObject.SetActive(false);
            Debug.Log(">> Set Module");
        }

        private void Start()
        {
            labels = GetComponentsInChildren<Text>();
            ShowQuestion();
        }

        public void ShowQuestion()
        {
            labels = GetComponentsInChildren<Text>();
            if (quiz == null || quiz.quizzes == null || index >= quiz.quizzes.Length)
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
                SetColor(i + 1, Color.white);
            }
        }

        void Exit()
        {
            index = 0;
            gameObject.SetActive(false);
        }

        public void Anwser(string option)
        {
            if (isLoadingNextQuestion)
                return;
            if(option == q.Answer)
            {
                Debug.Log("Correct!");
                SetColor(option, Color.green);
            }
            else
            {
                Debug.Log("Wrong!");
                SetColor(option, Color.red);
            }
            StartCoroutine(INextQuestion());
        }

        void SetColor(string option, Color color)
        {
            if(option == "A")
            {
                SetColor(1, color);
            }else if(option == "B")
            {
                SetColor(2, color);
            }
            else if (option == "C")
            {
                SetColor(3, color);
            }
            else if (option == "D")
            {
                SetColor(4, color);
            }
        }

        void SetColor(int n, Color color)
        {
            Debug.Log("Set Color " + color);
            labels[n].GetComponentInParent<Image>().color = color;
        }

        IEnumerator INextQuestion()
        {
            isLoadingNextQuestion = true;
            yield return new WaitForSeconds(0.5f);
            index++;
            ShowQuestion();
            isLoadingNextQuestion = false;
        }
    }
}