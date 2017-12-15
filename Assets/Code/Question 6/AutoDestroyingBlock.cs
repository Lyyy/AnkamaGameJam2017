using System.Collections;
using UnityEngine;

public class AutoDestroyingBlock : AnswerBlock
{
    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
            StartCoroutine(Explode());
        base.OnCollisionEnter2D(col);
    }

    private IEnumerator Explode()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<BoxCollider2D>().enabled = false;
        var scale = transform.localScale;
        for (var i = 0.25f; i > 0f; i -= Time.deltaTime)
        {
            scale.x = i * 4f;
            scale.y = scale.x;
            transform.localScale = scale;
            yield return null;
        }
        Destroy(gameObject);
    }
}
