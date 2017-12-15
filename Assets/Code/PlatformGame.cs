using UnityEngine;

public class PlatformGame : Game
{
    public Rigidbody2D player;
    public int speed = 300;
    public int jump = 7;
    private bool canJump = false;
    private float previousVelocityY = 0f;

    private readonly ContactFilter2D filter = new ContactFilter2D().NoFilter(); 

	// Update is called once per frame
	protected virtual void Update ()
	{
	    var x = Input.GetAxis("Horizontal");
		var velocity = player.velocity;
        velocity.x = Time.deltaTime * speed * x;
        if (Input.GetButtonDown("Jump") && canJump && Mathf.Abs(velocity.y) < 1f)
        {
            canJump = false;
            velocity.y = jump * -Mathf.Sign(Physics2D.gravity.y);
        } 
        else if (velocity.y - previousVelocityY > -Mathf.Sign(Physics2D.gravity.y) && player.IsTouching(filter))
	        canJump = true;
	    
	    player.velocity = velocity;
	    previousVelocityY = velocity.y;
	}
}
