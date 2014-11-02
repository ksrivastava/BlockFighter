using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.

	public bool pickedUpObject = false;
	
	public float moveForce = 100f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 700f;			// Amount of force added when the player jumps.
	
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	public bool grounded = false;			// Whether or not the player is grounded.
	
	public bool isSecondPlayer = false;

	bool onPlayer = false;

	void Awake()
	{
		// Setting up references.
		groundCheck = GameObject.Find(transform.parent.name + "/Body/groundCheck").transform;
		if (!isSecondPlayer) {
			facingRight = !facingRight;
		}
	}
	
	
	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		bool onGround = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		string otherPlayerLayer = (isSecondPlayer) ? "PlayerOne" : "PlayerTwo";
		onPlayer = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer(otherPlayerLayer));

		grounded = onGround || onPlayer;

		// If the jump button is pressed and the player is grounded then the player should 	.
		string jumpInput = isSecondPlayer ? "Jump2" : "Jump";
		if(Input.GetButtonDown(jumpInput) && grounded)
			jump = true;
	}
	
	
	void FixedUpdate ()
	{
		// Cache the horizontal input.
		float h = isSecondPlayer? Input.GetAxis("Horizontal2") : Input.GetAxis("Horizontal");
		
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * rigidbody2D.velocity.x < maxSpeed)
			// ... add a force to the player.
			rigidbody2D.AddForce(Vector2.right * h * moveForce);
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		
		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();
		
		// If the player should jump...
		if(jump)
		{
			var j = (Physics2D.gravity.y > 0)? -jumpForce : jumpForce;

			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, j));
			
			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
	}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
