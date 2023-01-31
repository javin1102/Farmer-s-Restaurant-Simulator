using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIStoreQuiz : MonoBehaviour
{

    [SerializeField] protected TMP_Text m_QuestionText;
    [SerializeField] protected Text m_AnswerA, m_AnswerB, m_AnswerC, m_AnswerD;
    [SerializeField] ToggleGroup m_ToggleGroup;
    [SerializeField] protected Toggle m_ToggleA, m_ToggleB, m_ToggleC, m_ToggleD;
    [SerializeField] protected Button m_SubmitButton, m_ShuffleButton, m_TryAgainButton;
    [SerializeField] protected GameObject m_WrongAnswerMenu, m_CorrectAnswerMenu, m_MainMenu, m_ResultMenu;
    protected List<QuizData> m_QuizData;
    protected int RandQuizIndex { get => Random.Range( 0, m_QuizData.Count ); }
    protected QuizData m_SelectedQuiz;
    [SerializeField] protected QuizAnswer m_SelectedAnswer;
    private void Awake()
    {
        m_QuizData = Resources.LoadAll<QuizData>( "Data/Quizzes" ).ToList();
        m_ToggleA.onValueChanged.AddListener( AnswerA );
        m_ToggleB.onValueChanged.AddListener( AnswerB );
        m_ToggleC.onValueChanged.AddListener( AnswerC );
        m_ToggleD.onValueChanged.AddListener( AnswerD );

        m_SubmitButton.onClick.AddListener( FinalizeAnswer );
        m_ShuffleButton.onClick.AddListener( ShuffleQuiz );
        m_TryAgainButton.onClick.AddListener( ResetQuiz );
    }
    private void OnEnable()
    {
        ResetQuiz();
    }


    private void Update()
    {
        if ( !m_ToggleGroup.AnyTogglesOn() ) m_SelectedAnswer = QuizAnswer.NONE;
    }
    private void ResetQuiz()
    {
        m_ResultMenu.SetActive( false );
        m_WrongAnswerMenu.SetActive( false );
        m_CorrectAnswerMenu.SetActive( false );
        m_ToggleA.isOn = false;
        m_ToggleB.isOn = false;
        m_ToggleC.isOn = false;
        m_ToggleD.isOn = false;
        m_MainMenu.SetActive( true );
        ShuffleQuiz();
    }

    private void FinalizeAnswer()
    {
        if ( m_SelectedAnswer == QuizAnswer.NONE ) return;
        m_MainMenu.SetActive( false );
        m_ResultMenu.SetActive( true );
        //Wrong Answer
        if ( m_SelectedAnswer != m_SelectedQuiz.correctAnswer )
        {
            m_WrongAnswerMenu.SetActive( true );
            return;
        }

        m_CorrectAnswerMenu.SetActive( true );
        SetReward();
    }

    protected abstract void SetReward();

    private void AnswerA( bool arg0 )
    {
        if ( arg0 == false ) return;
        m_SelectedAnswer = QuizAnswer.A;
    }

    private void AnswerB( bool arg0 )
    {
        if ( arg0 == false ) return;
        m_SelectedAnswer = QuizAnswer.B;
    }

    private void AnswerC( bool arg0 )
    {
        if ( arg0 == false ) return;
        m_SelectedAnswer = QuizAnswer.C;
    }

    private void AnswerD( bool arg0 )
    {
        if ( arg0 == false ) return;
        m_SelectedAnswer = QuizAnswer.D;
    }


    private void ShuffleQuiz()
    {
        m_SelectedQuiz = m_QuizData[RandQuizIndex];
        UpdateUI();
    }

    private void UpdateUI()
    {
        m_QuestionText.text = m_SelectedQuiz.question;
        m_AnswerA.text = m_SelectedQuiz.answerA;
        m_AnswerB.text = m_SelectedQuiz.answerB;
        m_AnswerC.text = m_SelectedQuiz.answerC;
        m_AnswerD.text = m_SelectedQuiz.answerD;
    }
}
