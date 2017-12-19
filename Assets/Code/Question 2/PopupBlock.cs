using UnityEngine;
using UnityEngine.UI;

public class PopupBlock : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator popup;
    public string validResponse;

    private Color initColor;

    void Start()
    {
        initColor = spriteRenderer.color;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
        {
            var color = spriteRenderer.color;
            color *= 0.5f;
            color.a = 1f;
            spriteRenderer.color = color;
            var text = GetComponentInChildren<Text>().text;
            if (string.Equals(validResponse, text))
                Game.GetInstance().Answer(text);
            else
            {
                popup.gameObject.SetActive(false);
                popup.gameObject.SetActive(true);
                var button = popup.GetComponentInChildren<Button>();
                GameState.GetInstance().DisableWaitingReaction();
                button.onClick.RemoveAllListeners();
                Game.GetInstance().CanDisplayWaitingReaction = false;
                button.onClick.AddListener(delegate
                {
                    GetComponentInChildren<Text>().text = validResponse;
                    popup.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                    spriteRenderer.color = initColor;
                    GameState.GetInstance().ResetWaitingReactionTimer();
                    Game.GetInstance().CanDisplayWaitingReaction = true;
                });    
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player") && !GameState.GetInstance().ValidAnswer)
        {
            spriteRenderer.color = initColor;
        }
    }
}
