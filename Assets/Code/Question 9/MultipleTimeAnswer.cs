using UnityEngine;
using UnityEngine.UI;

public class MultipleTimeAnswer : AnswerBlock
{
    private int hitNeeded = 5;
    private int hitLeft = 5;

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
        {
            if(--hitLeft == 0)
                Game.GetInstance().Answer(GetComponentInChildren<Text>().text);
            var color = Color.white * (hitLeft * 1f / hitNeeded);
            color.a = 1f;
            spriteRenderer.color = color;
        }
    }

    protected override void OnCollisionExit2D(Collision2D col)
    {

    }
}
