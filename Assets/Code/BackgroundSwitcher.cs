using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{

    public Sprite sea;
    public Sprite mountain;

    public static bool isSea = false;

	// Use this for initialization
	void Start ()
	{
	    GetComponent<SpriteRenderer>().sprite = isSea ? sea : mountain;
	}
}
