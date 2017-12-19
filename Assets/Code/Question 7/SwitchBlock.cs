using UnityEngine;
using UnityEngine.UI;

public class SwitchBlock : AnswerBlock
{
    public string newAnswer;

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);
        if (string.Equals(col.gameObject.name, "Player"))
            GetComponentInChildren<Text>().text = newAnswer;
    }
}
