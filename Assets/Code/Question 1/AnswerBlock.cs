using UnityEngine;
using UnityEngine.UI;

public class AnswerBlock : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
        {
            Game.GetInstance().Answer(GetComponentInChildren<Text>().text);
            var color = spriteRenderer.color;
            color *= 0.5f;
            color.a = 1f;
            spriteRenderer.color = color;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player") && !GameState.GetInstance().ValidAnswer)
        {
            spriteRenderer.color *= 2f;
        }
    }
}
