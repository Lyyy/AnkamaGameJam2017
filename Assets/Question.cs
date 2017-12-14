using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "GameJam/Question", order = 1)]
public class Question : ScriptableObject
{
    [Multiline]
    public string question;
    public Response[] responses;
    public GameObject game;
    [Multiline]
    public string globalReaction;
    [Multiline]
    public string[] randomReactions;
    public float minRandomDuration;
    public float maxRandomDuration;
    public Question globalNextQuestion;
}
