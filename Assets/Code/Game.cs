using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private static Game instance;
    private bool canDisplayWaitingReaction = true;
    protected Rigidbody2D player;

    public bool CanDisplayWaitingReaction
    {
        get { return canDisplayWaitingReaction; }
        set { canDisplayWaitingReaction = value; }
    }

    public static Game GetInstance()
    {
        return instance;
    }

    protected virtual void Awake()
    {
        instance = this;
        var placeholder = GameObject.Find("Player");
        player = Instantiate(Resources.Load("Player") as GameObject, placeholder.transform.position,
        placeholder.transform.rotation, GameObject.Find("Level").transform.parent).GetComponent<Rigidbody2D>();
        player.name = "Player";
        Destroy(placeholder);
    }

    public virtual IEnumerator Reload()
    {
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public virtual bool Answer(string answer)
    {
        return GameState.GetInstance().Answer(answer);
    }

    public virtual void Delete()
    {
        Destroy(gameObject);
    }
}