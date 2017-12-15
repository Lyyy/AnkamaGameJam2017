using UnityEngine;

public class Trap : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        GetComponents<BoxCollider2D>()[1].enabled = true;
        var renderer = GetComponent<SpriteRenderer>();
        var color = renderer.color;
        color.a = 1f;
        renderer.color = color;
    }
}
