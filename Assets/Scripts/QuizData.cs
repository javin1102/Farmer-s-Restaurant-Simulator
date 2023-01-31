using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuizAnswer
{
    NONE,A,B,C,D
}
[CreateAssetMenu( fileName = "Quiz", menuName = "Custom Data/New Quiz" )]
public class QuizData : ScriptableObject
{
    [TextArea]
    public string question;
    [TextArea]
    public string answerA,answerB, answerC, answerD;
    public QuizAnswer correctAnswer;
}
