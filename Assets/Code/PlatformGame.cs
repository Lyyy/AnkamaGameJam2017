using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGame : MonoBehaviour
{
    public Rigidbody2D player;
    public int speed = 500;
    public int jump = 10;

    private readonly ContactFilter2D filter = new ContactFilter2D().NoFilter();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    var x = Input.GetAxis("Horizontal");
		var velocity = player.velocity;
        velocity.x = Time.deltaTime * speed * x;
	    if (Input.GetButtonDown("Jump") && Mathf.Abs(velocity.y) < 0.2f && player.IsTouching(filter))
            velocity.y = jump;
	        
	    player.velocity = velocity;
	}
}
