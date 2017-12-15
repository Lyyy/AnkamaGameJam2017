using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if(string.Equals("Player",col.gameObject.name))
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
