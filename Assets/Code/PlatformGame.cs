using UnityEngine;

public class PlatformGame : Game
{
    public Rigidbody2D player;
    public int speed = 300;
    public int jump = 7;
    private bool canJump = true;
    private float previousVelocityY = 0f;

    private readonly ContactFilter2D filter = new ContactFilter2D().NoFilter(); 

	// Update is called once per frame
	protected virtual void Update ()
	{
	    var x = Input.GetAxis("Horizontal");
		var velocity = player.velocity;
        velocity.x = Time.deltaTime * speed * x;
	    var isTouching = player.IsTouching(filter);
        if (Input.GetButtonDown("Jump") && canJump && Mathf.Abs(velocity.y) < 1f && isTouching)
        {
            canJump = false;
            velocity.y = jump * -Mathf.Sign(Physics2D.gravity.y);
        } 
        else if (velocity.y - previousVelocityY > -Mathf.Sign(Physics2D.gravity.y) || Mathf.Approximately(velocity.y, 0f))
	        canJump = true;
	    
	    player.velocity = velocity;
	    previousVelocityY = velocity.y;
	}
}
