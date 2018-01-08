using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour {

    public Sprite levelZero;
    public Sprite levelOne;
    public Sprite levelThree;
    public Sprite levelFour;
    public Sprite levelInCharge;

    public static GameState.BatteryLevel currentBatteryLevel;

    public static Battery instance;

    // Use this for initialization
    void Awake ()
    {
        instance = this;
    }
	
    public static void UpdateSprite() {
        instance.UpdateSpriteInternal();
    }
	
    // Update is called once per frame
	private void UpdateSpriteInternal() {
        switch(currentBatteryLevel)
        {
            case GameState.BatteryLevel.Zero:
                GetComponent<Image>().sprite = levelZero;
                break;

            case GameState.BatteryLevel.One:
                GetComponent<Image>().sprite = levelOne;
                break;

            case GameState.BatteryLevel.Three:
                GetComponent<Image>().sprite = levelThree;
                break;

            case GameState.BatteryLevel.Four:
                GetComponent<Image>().sprite = levelFour;
                break;

            case GameState.BatteryLevel.InCharge:
                GetComponent<Image>().sprite = levelInCharge;
                break;
        }
        
    }
}
