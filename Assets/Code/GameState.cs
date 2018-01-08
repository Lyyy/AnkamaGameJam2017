using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMODUnity;

public class GameState : MonoBehaviour
{
    public Text question;
    public Text reaction;
    public Question startQuestion;
    public bool debug = false;

    public enum BatteryLevel { Zero, One, Three, Four, InCharge };
    public GameObject battery;

    public GameObject debugPanel;
    public Button[] debugButtons;

    private Animator animator;
    private Question currentQuestion;
    private Response currentResponse;
    private float waitingReactionTimer;
    private int lastWaitingReactionIndex = -1;
    private bool enableWaitingReaction = false;
    private bool canAnswer = false;
    private bool validAnswer = false;

    private FMOD.Studio.EventInstance writingSoundEvent;

    public static GameState instance;
    
	void Start ()
	{
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(Camera.main.gameObject);
        instance = this;
	    animator = GetComponent<Animator>();
	    currentQuestion = startQuestion;
        debugPanel.SetActive(Application.isEditor);
        foreach (var button in debugButtons)
            button.onClick.AddListener(() => Answer(button.GetComponentInChildren<Text>().text));
        PrepareNextQuestion();
	    StartCoroutine(DisplayQuestion());
        writingSoundEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Writing");
    }

    public static GameState GetInstance()
    {
        return instance;
    }

    public bool ValidAnswer
    {
        get { return validAnswer; }
    }

    public Response CurrentResponse
    {
        get { return currentResponse; }
    }

    private void PrepareNextQuestion()
    {
        validAnswer = false;
        question.text = null;
        reaction.text = null;
        Battery.currentBatteryLevel = currentQuestion.batteryLevel;
        Battery.UpdateSprite();
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
        if (currentQuestion.soundTransition > 0.01f)
        {
            GetComponent<StudioEventEmitter>().SetParameter("MusicTransition", currentQuestion.soundTransition);
        }
        yield return DisplayText(question, currentQuestion.question, false, false);
        lastWaitingReactionIndex = 0;
        ResetWaitingReactionTimer();
        if (!canAnswer)
            StartCoroutine(SpawnGame());
    }

    private IEnumerator SpawnGame()
    {
        canAnswer = true;
        if (!string.IsNullOrEmpty(currentQuestion.gameScene) && (!Application.isEditor || !SceneManager.GetSceneByName(currentQuestion.gameScene).isLoaded))
        {
            yield return SceneManager.LoadSceneAsync(currentQuestion.gameScene);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentQuestion.gameScene));        
        }
        animator.SetTrigger("DisplayGame");
    }

    public bool Answer(string anwser)
    {
        if (!canAnswer)
            return false;

        if (anwser.Contains("Allons à la"))
            BackgroundSwitcher.isSea = !anwser.Contains("mer");

        canAnswer = false;
        enableWaitingReaction = false;
        StopAllCoroutines();
        reaction.text = null;
        currentResponse = currentQuestion.responses.First(r => string.Equals(r.text, anwser));
        var nextQuestion = currentResponse.nextQuestion ?? currentQuestion.globalNextQuestion;
        validAnswer = nextQuestion != currentQuestion;
        animator.SetTrigger(validAnswer ? "Success" : "Fail");
        return true;
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
        var end = string.Equals(nextQuestion.name, "End");
        var success = nextQuestion != currentQuestion;
        if (success && Game.GetInstance() != null)
        {
            Game.GetInstance().Delete();
            if (nextQuestion.soundTransition > 0.01f)
            {
                GetComponent<StudioEventEmitter>().SetParameter("MusicTransition", nextQuestion.soundTransition);
            }
        }
        yield return DisplayText(reaction, reactionText, !end, success);

        if (success)
        {
            if (end)
            {
                Application.OpenURL("https://fr.wikipedia.org/wiki/Z%C3%A9bulon");
                yield break;
            }
            animator.SetTrigger("SuccessEnd");
            currentQuestion = nextQuestion;
            PrepareNextQuestion();
        }
        else
        {
            ResetWaitingReactionTimer();
            if (Game.GetInstance() != null)
                yield return Game.GetInstance().Reload();
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
        if (waitingReactionTimer < 0f && Game.GetInstance().CanDisplayWaitingReaction)
        {
            ResetWaitingReactionTimer();
            StopAllCoroutines();
            StartCoroutine(DisplayWaitingReaction());
        }
    }

    public void DisableWaitingReaction()
    {
        enableWaitingReaction = false;
    }

    public void ResetWaitingReactionTimer()
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
        var multiplier = Application.isEditor && debug ? 1000 : 1;

        for (int i = 0; i < values.Length; i++)
        {
            var t = values[i];
            var whiteValue = "";
            foreach (var c in t)
                if(c != '#')
                    whiteValue += c == ' ' ? ' ' : c == '\n' ? '\n' : '\u00A0';

            for (var j = 1; j <= t.Length; j++)
            {
                FMOD.Studio.PLAYBACK_STATE playbackState;
                writingSoundEvent.getPlaybackState(out playbackState);
                if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED && t[j - 1] != '.')
                {
                    writingSoundEvent.start();
                }
                if (t[j - 1] == '.')
                {
                    writingSoundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                }

                text.text = t.Substring(0, j) + whiteValue.Substring(j, whiteValue.Length - j);
                yield return new WaitForSeconds((t[j - 1] == '.' ? 0.5f : 0.04f) / multiplier);
                if (j < t.Length && t[j] == '#')
                {
                    if (!canAnswer && text == question)
                        StartCoroutine(SpawnGame());
                    writingSoundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    yield return new WaitForSeconds(3f / multiplier);
                    t = t.Remove(j, 1);
                }
            }
            if (i < values.Length - 1)
            {
                writingSoundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                yield return new WaitForSeconds(Mathf.Clamp(reaction.text.Length * 0.03f, 0.75f, 3f) / multiplier);
                text.text = "";
            }
        }
        
        writingSoundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

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
