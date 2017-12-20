using UnityEngine;
using UnityEngine.UI;

public class TriggerBlock : MonoBehaviour {

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
        {
            Game.GetInstance().Answer(GetComponentInChildren<Text>().text);
        }
    }
}
