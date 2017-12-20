using UnityEngine;
using UnityEngine.UI;

public class AnswerBlock : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private Color initColor;

    void Start()
    {
        initColor = spriteRenderer.color;
    }

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
        {
            Game.GetInstance().Answer(GetComponentInChildren<Text>().text);
            spriteRenderer.color = Color.gray;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player") && !GameState.GetInstance().ValidAnswer)
        {
            spriteRenderer.color = initColor;
        }
    }
}
