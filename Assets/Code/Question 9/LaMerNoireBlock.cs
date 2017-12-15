using UnityEngine;

public class LaMerNoireBlock : AutoDestroyingBlock
{

    public GameObject next;

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);
        if (string.Equals(col.gameObject.name, "Player"))
        {
            if (next)
                next.SetActive(true);
        }
    }
	
}
