using UnityEngine;

public class FakePlatform : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        var renderer = GetComponent<SpriteRenderer>();
        var color = renderer.color;
        color.a *= 0.1f;
        renderer.color = color;
    }
}
