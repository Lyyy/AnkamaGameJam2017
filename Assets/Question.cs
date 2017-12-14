using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "GameJam/Question", order = 1)]
public class Question : ScriptableObject
{
    [Multiline]
    public string question;
    public Response[] responses;
    public GameObject game;
    [Multiline]
    public string[] globalReactions;
    public Question globalNextQuestion;
}
