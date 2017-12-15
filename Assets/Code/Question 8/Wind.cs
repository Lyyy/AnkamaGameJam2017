using UnityEngine;

public class Wind : MonoBehaviour
{

    public float strength;

    void OnTriggerStay2D(Collider2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
        {
            col.GetComponent<Rigidbody2D>().AddForce(Vector2.left * (GetComponent<BoxCollider2D>().size.x - Vector2.Distance(transform.position, col.transform.position)) * strength * Time.fixedDeltaTime);
        }
    }
}
