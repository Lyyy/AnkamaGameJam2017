using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "GameJam/Question", order = 1)]
public class Question : ScriptableObject
{
    [Multiline]
    public string question;
    public Response[] responses;
    public string gameScene;
    [Multiline]
    public string[] waitingReactions;
    public float minWaitingDuration;
    public float maxWaitingDuration;
    [Multiline]
    public string globalReaction;
    public Question globalNextQuestion;
}
