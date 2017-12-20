using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VanishingBlock : MonoBehaviour {

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
        {
            Game.GetInstance().Answer(GetComponentInChildren<Text>().text);
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        foreach (var col in GetComponents<BoxCollider2D>())
            col.enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
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
