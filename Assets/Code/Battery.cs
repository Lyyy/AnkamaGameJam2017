using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour {

    public Sprite levelZero;
    public Sprite levelOne;
    public Sprite levelThree;
    public Sprite levelFour;
    public Sprite levelInCharge;

    public static GameState.enumBatteryLevel currentBatteryLevel;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        print("CurrentBatteryLevel : " + currentBatteryLevel);
        switch(currentBatteryLevel)
        {
            case GameState.enumBatteryLevel.Zero:
                GetComponent<SpriteRenderer>().sprite = levelZero;
                break;

            case GameState.enumBatteryLevel.One:
                GetComponent<SpriteRenderer>().sprite = levelOne;
                break;

            case GameState.enumBatteryLevel.Three:
                GetComponent<SpriteRenderer>().sprite = levelThree;
                break;

            case GameState.enumBatteryLevel.Four:
                GetComponent<SpriteRenderer>().sprite = levelFour;
                print("Sprite : " + GetComponent<SpriteRenderer>().sprite);
                break;

            case GameState.enumBatteryLevel.InCharge:
                GetComponent<SpriteRenderer>().sprite = levelInCharge;
                break;
        }
        
    }
}
