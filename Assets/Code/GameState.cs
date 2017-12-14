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
    private float waitingReactionTimer;
    private int lastWaitingReactionIndex = -1;
    private bool enableWaitingReaction = false;
    private bool canAnswer = false;

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
            debugButtons[i].gameObject.SetActive(true);
        }
        for (var i = currentQuestion.responses.Length; i < debugButtons.Length; i++)
            debugButtons[i].gameObject.SetActive(false);
    }

    private IEnumerator DisplayQuestion()
    {
        yield return DisplayText(question, currentQuestion.question, false, false);
        lastWaitingReactionIndex = 0;
        ResetWaitingReactionTimer();
        if (!canAnswer)
            StartCoroutine(SpawnGame());
    }

    private IEnumerator SpawnGame()
    {
        canAnswer = true;
        yield break;
    }

    public void Answer(string anwser)
    {
        if (!canAnswer)
            return;

        canAnswer = false;
        StopAllCoroutines();
        reaction.text = null;
        currentResponse = currentQuestion.responses.First(r => string.Equals(r.text, anwser));
        var nextQuestion = currentResponse.nextQuestion ?? currentQuestion.globalNextQuestion;
        animator.SetTrigger(nextQuestion == currentQuestion ? "Fail" : "Success");
    }

    void OnDisplayReaction()
    {
        StopAllCoroutines();
        ResetWaitingReactionTimer();
        enableWaitingReaction = false;
        StartCoroutine(DisplayReaction());
    }

    private IEnumerator DisplayReaction()
    {
        var reactionText = string.IsNullOrEmpty(currentResponse.reaction)
            ? currentQuestion.globalReaction
            : currentResponse.reaction;
        var nextQuestion = currentResponse.nextQuestion ?? currentQuestion.globalNextQuestion;
        var success = nextQuestion != currentQuestion;
        yield return DisplayText(reaction, reactionText, true, success);

        if (success)
        {
            yield return StartCoroutine(FadeReaction());
            animator.SetTrigger("SuccessEnd");
            currentQuestion = nextQuestion;
            PrepareNextQuestion();
            //destroy game
            
        }
        else
        {
            ResetWaitingReactionTimer();
            //restart game
            canAnswer = true;
        }
    }

    void OnNextQuestion()
    {
        StartCoroutine(DisplayQuestion());
    }

    void Update()
    {
        if (!enableWaitingReaction)
            return;

        waitingReactionTimer -= Time.deltaTime;
        if (waitingReactionTimer < 0f)
        {
            ResetWaitingReactionTimer();
            StopAllCoroutines();
            StartCoroutine(DisplayWaitingReaction());
        }
    }

    private void ResetWaitingReactionTimer()
    {
        enableWaitingReaction = lastWaitingReactionIndex != currentQuestion.waitingReactions.Length;
        waitingReactionTimer = Random.Range(currentQuestion.minWaitingDuration, currentQuestion.maxWaitingDuration);
    }

    private IEnumerator DisplayWaitingReaction()
    {
        var reactionText = currentQuestion.waitingReactions[lastWaitingReactionIndex];
        lastWaitingReactionIndex++;
        enableWaitingReaction = false;
        yield return DisplayText(reaction, reactionText, true, false);
        enableWaitingReaction = lastWaitingReactionIndex != currentQuestion.waitingReactions.Length;
    }

    private IEnumerator DisplayText(Text text, string value, bool fade, bool waitForFade)
    {
        var color = text.color;
        color.a = 1f;
        text.color = color;
        text.text = "";
        string[] values = value.Split('_');

        for (int i = 0; i < values.Length; i++)
        {
            var t = values[i];
            var whiteValue = "";
            foreach (var c in t)
                if(c != '#')
                    whiteValue += c == ' ' ? ' ' : c == '\n' ? '\n' : '\u00A0';

            for (var j = 1; j <= t.Length; j++)
            {
                text.text = t.Substring(0, j) + whiteValue.Substring(j, whiteValue.Length - j);
                yield return new WaitForSeconds(t[j-1] == '.' ? 0.5f : 0.04f);
                if (j < t.Length && t[j] == '#')
                {
                    if (!canAnswer && text == question)
                        StartCoroutine(SpawnGame());
                    yield return new WaitForSeconds(3f);
                    t = t.Remove(j, 1);
                }
            }
            if (i < values.Length - 1)
            {
                yield return new WaitForSeconds(Mathf.Clamp(reaction.text.Length * 0.03f, 0.75f, 3f));
                text.text = "";
            }
        }
        
        if (fade)
        {
            var coroutine = StartCoroutine(FadeReaction());
            if (waitForFade)
                yield return coroutine;    
        }
    }

    private IEnumerator FadeReaction()
    {
        yield return new WaitForSeconds(Mathf.Clamp(reaction.text.Length * 0.03f, 0.75f, 3f));
        Color color;
        for (var i = 0.25f; i > 0f; i -= Time.deltaTime)
        {
            color = reaction.color;
            color.a = i * 4f;
            reaction.color = color;
            yield return null;
        }
        color = reaction.color;
        color.a = 1f;
        reaction.color = color;
        reaction.text = null;
    }
}
