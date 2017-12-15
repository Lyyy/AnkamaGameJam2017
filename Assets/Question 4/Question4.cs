using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Question4 : PlatformGame
{
    public AnswerBlock thirstyButton;
    public BoxCollider2D thirstyTrap;
    public SpriteRenderer water;
    public SpriteRenderer whitePanel;

    private static bool thirsty = false;
    
	void Start ()
	{
	    if (thirsty)
	    {
            Destroy(thirstyButton.gameObject);
	        thirstyTrap.isTrigger = false;
            water.gameObject.SetActive(false);
	    }
	}

    public override bool Answer(string answer)
    {
        if (!base.Answer(answer))
            return false;

        if (string.Equals("J'ai soif...", answer))
        {
            thirsty = true;
            player.bodyType = RigidbodyType2D.Static;
        }
        else if (answer.IndexOf("pas", System.StringComparison.Ordinal) != -1)
        {
            StartCoroutine(FadeIn());
        }
        return true;
    }

    private IEnumerator FadeIn()
    {
        var color = whitePanel.color;
        color.a *= 0.1f;        
        while (color.a < 1f)
        {
            color.a = Mathf.Clamp01(color.a + Time.deltaTime * 0.1f);
            whitePanel.color = color;
            yield return null;
        }
    }

    public override void Delete()
    {
        base.Delete();
        thirsty = false;
    }
}
