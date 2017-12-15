using UnityEngine;

public class LaMerNoireBlock : AnswerBlock
{

    public GameObject next;

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);
        if (string.Equals(col.gameObject.name, "Player"))
        {
            if (next)
            {
                Destroy(gameObject);
                next.SetActive(true);
            }
        }
    }
	
}
