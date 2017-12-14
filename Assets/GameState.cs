using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public Text question;
    public Text reaction;
    public Question startQuestion;

    public GameObject debugPanel;
    public Button[] debugButtons;

    private Animator animator;
    private Question currentQuestion;
    private Response currentResponse;
    private float randomReactionTimer;
    private int lastRandomReactionIndex = -1;
    private bool enableRandomReaction = false;

    public static GameState instance;
    
	void Start ()
	{
	    instance = this;
	    animator = GetComponent<Animator>();
	    currentQuestion = startQuestion;
        debugPanel.SetActive(Application.isEditor);
        foreach (var button in debugButtons)
            button.onClick.AddListener(() => Answer(button.GetComponentInChildren<Text>().text));
        PrepareNextQuestion();
	    StartCoroutine(DisplayQuestion());
	}

    public static GameState GetInstance()
    {
        return instance;
    }

    private void PrepareNextQuestion()
    {
        question.text = null;
        reaction.text = null;
        for (var i = 0; i < currentQuestion.responses.Length; i++)
        {
            debugButtons[i].GetComponentInChildren<Text>().text = currentQuestion.responses[i].text;
        }
    }

    private IEnumerator DisplayQuestion()
    {
        foreach (var c in currentQuestion.question)
        {
            question.text += c;
            yield return new WaitForSeconds(c == '.' ? 0.75f : 0.04f);
        }
        lastRandomReactionIndex = -1;
        enableRandomReaction = currentQuestion.randomReactions.Length != 0;
        ResetRandomReactionTimer();
        //spawn game, fade ?
    }

    public void Answer(string anwser)
    {
        StopAllCoroutines();
        reaction.text = null;
        currentResponse = currentQuestion.responses.First(r => string.Equals(r.text, anwser));
        var nextQuestion = currentResponse.nextQuestion ?? currentQuestion.globalNextQuestion;
        animator.SetTrigger(nextQuestion == currentQuestion ? "Fail" : "Success");
    }

    void OnDisplayReaction()
    {
        StopAllCoroutines();
        ResetRandomReactionTimer();
        enableRandomReaction = false;
        StartCoroutine(DisplayReaction());
    }

    private IEnumerator DisplayReaction()
    {
        var reactionText = string.IsNullOrEmpty(currentResponse.reaction)
            ? currentQuestion.globalReaction
            : currentResponse.reaction;
        foreach (var c in reactionText)
        {
            reaction.text += c;
            yield return new WaitForSeconds(c == '.' ? 0.75f : 0.04f);
        }
        var nextQuestion = currentResponse.nextQuestion ?? currentQuestion.globalNextQuestion;
        if (nextQuestion == currentQuestion)
        {
            StartCoroutine(FadeReaction());
            //restart game
        }
        else
        {
            yield return StartCoroutine(FadeReaction());
            animator.SetTrigger("SuccessEnd");
            currentQuestion = nextQuestion;
            PrepareNextQuestion();
            //destroy game
        }
    }

    void OnNextQuestion()
    {
        StartCoroutine(DisplayQuestion());
    }

    private IEnumerator FadeReaction()
    {
        yield return new WaitForSeconds(0.75f);
        Color color;
        for (var i = 1f; i > 0f; i -= Time.deltaTime)
        {
            color = reaction.color;
            color.a = i;
            reaction.color = color;
            yield return null;
        }
        color = reaction.color;
        color.a = 1f;
        reaction.color = color;
        reaction.text = null;
    }

    void Update()
    {
        if (!enableRandomReaction)
            return;

        randomReactionTimer -= Time.deltaTime;
        if (randomReactionTimer < 0f)
        {
            ResetRandomReactionTimer();
            StartCoroutine(DisplayRandomReaction());
        }
    }

    private void ResetRandomReactionTimer()
    {
        randomReactionTimer = Random.Range(currentQuestion.minRandomDuration, currentQuestion.maxRandomDuration);
    }

    private IEnumerator DisplayRandomReaction()
    {
        int newRandomReactionIndex;
        do
        {
            newRandomReactionIndex = Random.Range(0, currentQuestion.randomReactions.Length);
        } 
        while (newRandomReactionIndex == lastRandomReactionIndex);
        lastRandomReactionIndex = newRandomReactionIndex;
        var reactionText = currentQuestion.randomReactions[newRandomReactionIndex];
        foreach (var c in reactionText)
        {
            reaction.text += c;
            yield return new WaitForSeconds(c == '.' ? 0.75f : 0.04f);
        }
        StartCoroutine(FadeReaction());
    }
}
