using UnityEngine;

public class PlatformGame : Game
{
    public int speed = 300;
    public int jump = 7;
    private bool canJump = true;
    private float previousVelocityY = 0f;

    private readonly ContactFilter2D filter = new ContactFilter2D().NoFilter();

    // Update is called once per frame
	protected virtual void Update ()
	{
	    var scale = player.transform.localScale;
	    scale.y = -Mathf.Sign(Physics2D.gravity.y);
	    player.transform.localScale = scale;
	    var x = Input.GetAxis("Horizontal");
		var velocity = player.velocity;
        velocity.x = Time.deltaTime * speed * x;
	    player.GetComponent<Animator>().SetBool("Running", !Mathf.Approximately(velocity.x, 0f));
	    var isTouching = player.IsTouching(filter);
        if (Input.GetButtonDown("Jump") && canJump && Mathf.Abs(velocity.y) < 1f && isTouching)
        {
            canJump = false;
            velocity.y = jump * -Mathf.Sign(Physics2D.gravity.y);
            player.GetComponent<Animator>().SetTrigger("Jumping");
        } 
        else if (velocity.y - previousVelocityY > -Mathf.Sign(Physics2D.gravity.y) || Mathf.Approximately(velocity.y, 0f))
	        canJump = true;
	    
	    player.velocity = velocity;
	    previousVelocityY = velocity.y;
	}
}
