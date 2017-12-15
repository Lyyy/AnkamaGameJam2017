using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform target;

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
            col.transform.position = target.transform.position;
    }
}
