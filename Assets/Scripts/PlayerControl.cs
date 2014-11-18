using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.

	bool leftDash = false;	
	bool rightDash = false;	

	public int playerNum;
	public bool pickedUpObject = false;
	
	private float moveForce = 400f;			// Amount of force added to move the player left and right.
	private float maxSpeed = 6f;				// The fastest the player can travel in the x axis.
	private float jumpForce = 1000f;			// Amount of force added when the player jumps.
	
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	public bool grounded = false;			// Whether or not the player is grounded.
	private bool onPlayer = false;
	string jumpButton;

	public HealthBar healthBar;

	void Awake()
	{
		// Setting up references.
		healthBar = GetComponent<HealthBar> ();
		groundCheck = GameObject.Find(transform.parent.name + "/Body/groundCheck").transform;
		jumpButton = "joystick " + playerNum + " button 16";
	}
	
	
	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		Vector3 groundPos = groundCheck.position;
		groundPos.x = groundCheck.collider2D.bounds.min.x;
		bool onGroundLeft = Physics2D.Linecast(transform.position, groundPos, 1 << LayerMask.NameToLayer("Ground"));
		groundPos.x = groundCheck.collider2D.bounds.max.x;
		bool onGroundRight = Physics2D.Linecast(transform.position, groundPos, 1 << LayerMask.NameToLayer("Ground"));

		onPlayer = false;
		for (int player = 1; player <= 4 && !onPlayer; player++) {
			if(player != playerNum) {
				groundPos = groundCheck.position;
				onPlayer = Physics2D.Linecast(transform.position, groundPos, 1 << LayerMask.NameToLayer("Player" + player));
				// Check the left of the player if necessary
				if(!onPlayer) {
					groundPos.x = groundCheck.collider2D.bounds.min.x;
					onPlayer = Physics2D.Linecast(transform.position, groundPos, 1 << LayerMask.NameToLayer("Player" + player));
				}
				// Check the right of the player if necessary
				if(!onPlayer) {
					groundPos.x = groundCheck.collider2D.bounds.max.x;
					onPlayer = Physics2D.Linecast(transform.position, groundPos, 1 << LayerMask.NameToLayer("Player" + player));
				}
			}
		}
		grounded = onGroundLeft || onGroundRight || onPlayer;

//		if(Input.GetKeyDown (jumpButton) && grounded)
//			jump = true;
//			
//		if (Input.GetKeyDown(KeyCode.LeftBracket)) {
//			leftDash = true;
//		}
//		if (Input.GetKeyDown(KeyCode.RightBracket)) {
//			rightDash = true;
//		}

		// If the jump button is pressed and the player is grounded then the player should.
		if(Input.GetButtonDown("Jump" + playerNum) && grounded) {
			jump = true;
		}
		if (Input.GetButtonDown("LeftDash" + playerNum) && healthBar.Dash >= 0.5f) {
			leftDash = true;
		}
		else if (Input.GetButtonDown("RightDash" + playerNum) && healthBar.Dash >= 0.5f) {
			rightDash = true;
		}

	}
	
	
	void FixedUpdate ()
	{
		if (leftDash) {
			rigidbody2D.AddForce(Vector2.right * -1 * moveForce * 20);
			leftDash = false;
			healthBar.Dash -= 0.5f;
		}
		
		else if (rightDash) {
			rigidbody2D.AddForce(Vector2.right * moveForce * 20);
			rightDash = false;
			healthBar.Dash -= 0.5f;
		}

		// Cache the horizontal input.
		float h = Input.GetAxis ("Horizontal" + playerNum);
		
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
			Jump();
		}
	}

	public void Jump(){
		var j = (Physics2D.gravity.y > 0)? -jumpForce : jumpForce;
		
		// Add a vertical force to the player.
		rigidbody2D.AddForce(new Vector2(0f, j));
		
		// Make sure the player can't jump again until the jump conditions from Update are satisfied.
		jump = false;
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

	public int GetPlayerNum() {
		return playerNum;
	}

	public bool IsGrounded() {
		return grounded;
	}
}
