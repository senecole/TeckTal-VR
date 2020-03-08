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
        public GameObject right;
        public GameObject wrong;
        public Text scoreLabel;
        int score;

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
            HideFeedback();
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
            ShowScore();
            index = 0;
            score = 0;
            gameObject.SetActive(false);
        }

        void ShowScore()
        {
            if (scoreLabel != null)
            {
                scoreLabel.gameObject.SetActive(true);
                scoreLabel.text = "Score: " + score + "/" + index;
                scoreLabel.StartCoroutine(HideScore());
            }
        }

        IEnumerator HideScore()
        {
            yield return new WaitForSeconds(2);
            scoreLabel.gameObject.SetActive(false);
        }

        public void Anwser(string option)
        {
            if (isLoadingNextQuestion)
                return;
            if(option == q.Answer)
            {
                Debug.Log("Correct!");
                score++;
                SetColor(option, Color.green);
                ShowFeedback(right);
            }
            else
            {
                Debug.Log("Wrong!");
                SetColor(option, Color.red);
                ShowFeedback(wrong);
            }
            StartCoroutine(INextQuestion());
        }

        void HideFeedback()
        {
            if (right != null)
            {
                right.SetActive(false);
            }
            if (wrong != null)
            {
                wrong.SetActive(false);
            }
        }

        void ShowFeedback(GameObject obj)
        {
            if (obj == null)
                return;
            obj.SetActive(true);
            AudioSource audio = obj.GetComponent<AudioSource>();
            if(audio != null)
            {
                audio.Play();
            }
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