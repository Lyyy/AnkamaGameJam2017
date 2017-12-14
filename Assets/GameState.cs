﻿using System.Collections;
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
        StopAllCoroutines();
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
        foreach (var c in value)
        {
            if (c == '_')
            {
                yield return new WaitForSeconds(1f);
                text.text = "";
                continue;
            }
            text.text += c;
            yield return new WaitForSeconds(c == '.' ? 0.5f : 0.04f);
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
        yield return new WaitForSeconds(Mathf.Max(0.75f, reaction.text.Length * 0.1f));
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
}
