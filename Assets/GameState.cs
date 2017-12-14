using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public Text question;
    public Text reaction;
    public Question startQuestion;
    
    private Animator animator;

    private Question currentQuestion;
    private Response currentResponse;

    public static GameState instance;
    
	void Start ()
	{
	    instance = this;
	    animator = GetComponent<Animator>();
	    currentQuestion = startQuestion;
	    StartCoroutine(DisplayQuestion());
	}

    public static GameState GetInstance()
    {
        return instance;
    }

    private IEnumerator DisplayQuestion()
    {
        question.text = null;
        reaction.text = null;
        foreach (var c in currentQuestion.question)
        {
            question.text += c;
            yield return new WaitForSeconds(c == '.' ? 0.75f : 0.04f);
        }
        //spawn game, fade ?
    }

    public void Answer(string anwser)
    {
        reaction.text = null;
        currentResponse = currentQuestion.responses.First(r => string.Equals(r.text, anwser));
        animator.SetTrigger(currentResponse.nextQuestion == currentQuestion ? "Fail" : "Success");
    }

    void OnDisplayReaction()
    {
        StopAllCoroutines();
        StartCoroutine(DisplayReaction());
    }

    private IEnumerator DisplayReaction()
    {
        foreach (var c in currentResponse.reaction)
        {
            reaction.text += c;
            yield return new WaitForSeconds(c == '.' ? 0.75f : 0.04f);
        }
        if (currentResponse.nextQuestion == currentQuestion)
        {
            StartCoroutine(FadeReaction());
            //restart game
        }
        else
        {
            yield return StartCoroutine(FadeReaction());
            animator.SetTrigger("SuccessEnd");
            question.text = null;
            currentQuestion = currentResponse.nextQuestion;
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
        for (var i = 1f; i > 0f; i -= Time.deltaTime)
        {
            var color = reaction.color;
            color.a = i;
            reaction.color = color;
            yield return null;
        }
    }
}
