using System.Collections;
using UnityEngine;

public class Question18 : PlatformGame
{
    public SpriteRenderer blackPanel;

	void Start ()
	{
        StartCoroutine(FadeIn());
	}

    private IEnumerator FadeIn()
    {
        const float duration = 45f;
        var color = blackPanel.color;
        for (var i = 0f; i < duration; i+= Time.deltaTime)
        {
            color.a = i / duration;
            blackPanel.color = color;
            yield return null;
        }
        Answer("Oui bien sûr !");
    }
}
