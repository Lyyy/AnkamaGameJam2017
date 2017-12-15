using UnityEngine;

public class PlatformGame : Game
{
    public Rigidbody2D player;
    public int speed = 500;
    public int jump = 10;

    private float previousVelocityY = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var x = Input.GetAxis("Horizontal");
		var velocity = player.velocity;
        velocity.x = Time.deltaTime * speed * x;
        if (Input.GetButtonDown("Jump") && Mathf.Abs(velocity.y) < 0.1f && Mathf.Abs(previousVelocityY) < 0.1f)
            velocity.y = jump;
	        
	    player.velocity = velocity;
	    previousVelocityY = velocity.y;
	}
}
