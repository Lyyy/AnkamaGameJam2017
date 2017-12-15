using UnityEngine;

public class Question5 : PlatformGame
{
    private Vector2 initialGravity;

	// Use this for initialization
	void Start ()
	{
	    initialGravity = Physics2D.gravity;
	    var gravity = initialGravity;
	    gravity.y *= -1;
	    Physics2D.gravity = gravity;
	}

    void OnDestroy()
    {
        Physics2D.gravity = initialGravity;
    }
}
