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
            spriteRenderer.color *= 0.5f;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
        {
            spriteRenderer.color *= 2f;
        }
    }
}
