using UnityEngine;

public class FrozenAnswer : AnswerBlock
{
    public Color color;
    private bool done = false;

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player") && !done)
        {
            done = true;
            spriteRenderer.color = color;
        }
    }

    protected override void OnCollisionExit2D(Collision2D col)
    {
        
    }
}
