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
            spriteRenderer.color *= 0.5f; 
            var text = GetComponentInChildren<Text>().text;
            if (string.Equals(validResponse, text))
                GameState.GetInstance().Answer(text);
            else
            {
                popup.gameObject.SetActive(false);
                popup.gameObject.SetActive(true);
                var button =popup.GetComponentInChildren<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate
                {
                    GetComponentInChildren<Text>().text = "Oui";
                    popup.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                    spriteRenderer.color = initColor;
                });    
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (string.Equals(col.gameObject.name, "Player"))
        {
            spriteRenderer.color = initColor;
        }
    }
}
