using UnityEngine;
using UnityEngine.UI;

public class AnswerBloc : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public AnimationCurve curve;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
        {
            GameState.GetInstance().Answer(GetComponentInChildren<Text>().text);
            spriteRenderer.color *= 0.5f;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
        {
            spriteRenderer.color *= 2f;
        }
    }
}
