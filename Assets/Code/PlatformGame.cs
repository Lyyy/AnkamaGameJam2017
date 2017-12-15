using UnityEngine;

public class PlatformGame : Game
{
    public Rigidbody2D player;
    public int speed = 500;
    public int jump = 10;
    private bool canJump = false;
    private float previousVelocityY = 0f;

    private readonly ContactFilter2D filter = new ContactFilter2D().NoFilter(); 

	// Update is called once per frame
	void Update ()
	{
	    var x = Input.GetAxis("Horizontal");
		var velocity = player.velocity;
        velocity.x = Time.deltaTime * speed * x;
        if((-Mathf.Sign(Physics2D.gravity.y) * (velocity.y - previousVelocityY) > 1f || Mathf.Approximately(velocity.y, 0f)) && player.IsTouching(filter))
	        canJump = true;
	    if (Input.GetButtonDown("Jump") && canJump)
	    {
	        canJump = false;
            velocity.y = jump * -Mathf.Sign(Physics2D.gravity.y);
	    }   
	    player.velocity = velocity;
	    previousVelocityY = velocity.y;
	}
}
