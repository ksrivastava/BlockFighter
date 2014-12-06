using UnityEngine;
using System.Collections;
using InControl;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
	
	bool leftDash = false;
	bool rightDash = false;

	public int playerNum;
	public string playerName;
	public bool pickedUpObject = false;
	
	private float moveForce = 400f;			// Amount of force added to move the player left and right.
	private float maxSpeed = 6f;				// The fastest the player can travel in the x axis.
	private float jumpForce = 1000f;			// Amount of force added when the player jumps.
	
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	public bool grounded = false;			// Whether or not the player is grounded.
	private bool onPlayer = false;
	string jumpButton, leftDashButton, rightDashButton;

	private Animator anim;

	PlayerBehavior behavior;

	public HealthBar healthBar;
	[HideInInspector]
	public bool allowMovement = true;
	
	[HideInInspector]
	public bool allowHitting = true;


	// DASHING
	public Movement dashMovement = null;
	float dashDuration = 0.05f;
	float dashXDist = 10f;
	int dashDamage = 10;

	void Awake()
	{
		// Setting up references.
		healthBar = GetComponent<HealthBar> ();
		behavior = GetComponent<PlayerBehavior> ();
		groundCheck = GameObject.Find(transform.parent.name + "/Body/groundCheck").transform;
		jumpButton = "joystick " + playerNum + " button 16";
		leftDashButton = "joystick " + playerNum + " button 13";
		rightDashButton = "joystick " + playerNum + " button 14";
	}

	void Start() 
	{
		anim = this.GetComponent<Animator> ();
	}

	public void SetInfinityMaxSpeed(){
		maxSpeed = float.MaxValue;
	}

	public void ResetMaxSpeed(){
		maxSpeed = 6f;
	}
	
	void Update()
	{
		if (dashMovement != null) {
			dashMovement.Update();
		}


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

//		// XBOX
//		if(Input.GetKeyDown (jumpButton) && grounded)
//			jump = true;
//			
//		if (Input.GetKeyDown(leftDashButton) && (healthBar.Dash >= 0.5f)) {
//			leftDash = true;
//		}
//		else if (Input.GetKeyDown(rightDashButton) && (healthBar.Dash >= 0.5f)) {
//			rightDash = true;
//		}
//

		// KEYBOARD

		if(Input.GetButtonDown("Jump" + playerNum) && grounded) {
			jump = true;
		}
		if (Input.GetButtonDown("LeftDash" + playerNum) && (healthBar.Dash >= 0.5f)) {
			leftDash = true;
		}
		else if (Input.GetButtonDown("RightDash" + playerNum) && (healthBar.Dash >= 0.5f)) {
			rightDash = true;
		}


		// turn on collisions between ground and self if velocity is ever < zero.
		if (rigidbody2D.velocity.y < 0) {
			Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Ground"),this.gameObject.layer, false);
		}

		
		
		var inputDevice = (InputManager.Devices.Count > playerNum-1) ? InputManager.Devices[playerNum-1] : null;
		if (inputDevice == null)
			return;
		if (inputDevice.LeftBumper.WasPressed && (healthBar.Dash >= 0.5f)) {
			leftDash = true;
		}
		
		if (inputDevice.RightBumper.WasPressed && (healthBar.Dash >= 0.5f)) {
			rightDash = true;
		}
		
		if (inputDevice.Action1.WasPressed && grounded) {
			jump = true;
		}
	}
	
	float dashMultiplier = 30f;

	GameObject getTopParent(GameObject input){
		Transform t = input.transform;
		Transform p = input.transform.parent;
		
		while (p != null) {
			t = p;
			p = p.transform.parent;
		}
		
		return t.gameObject;
	}

	IEnumerator Trail(float startTime, float endTime) {
		while (startTime < endTime) {
			GameObject f = new GameObject();
			f.transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 1);
			f.transform.localScale = transform.localScale;
			f.AddComponent<SpriteRenderer> ();
			SpriteRenderer faderSprite = f.GetComponent<SpriteRenderer>();
			faderSprite.sprite = GetComponent<SpriteRenderer>().sprite;
			f.AddComponent<SelfDestruct> ();
			startTime += Time.deltaTime;
			yield return null;
		}
	}


	void FixedUpdate ()
	{
		if (allowMovement) {

			// Cache the horizontal input.
			float h = Input.GetAxis ("Horizontal" + playerNum);

			var inputDevice = (InputManager.Devices.Count > playerNum-1) ? InputManager.Devices[playerNum-1] : null;
			if (inputDevice != null) {
				inputDevice.LeftStickX.Sensitivity = 1f;
				h = inputDevice.LeftStickX.Value;

				if (Mathf.Abs(h) <= 0.65f) {
					h = 0f;
				}
			}

			if(h != 0f && anim != null) {
				anim.SetBool ("isMoving", true);
			} else if (anim != null) {
				anim.SetBool ("isMoving", false);
			}

			// If the player's horizontal velocity is greater than the maxSpeed...
			if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
				// ... set the player's velocity to the maxSpeed in the x axis.
				rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		

			if (leftDash) {
				//				if (facingRight) Flip ();
				rigidbody2D.AddForce(Vector2.right * -1 * moveForce * dashMultiplier);
				healthBar.Dash -= 0.5f;
				StartCoroutine(Trail(Time.time, Time.time + dashDuration));
			}
			
			else if (rightDash) {
				//				if (!facingRight) Flip ();
				rigidbody2D.AddForce(Vector2.right * moveForce * dashMultiplier);
				healthBar.Dash -= 0.5f;
				StartCoroutine(Trail(Time.time, Time.time + dashDuration));
			}
		

			
//			if (leftDash) {
//
//				this.rigidbody2D.velocity = Vector2.zero;
//				this.rigidbody2D.angularVelocity = 0;
//				var startPosition = transform.position;
//				var endPosition = startPosition;
//				var startTime = Time.time;
//
//				endPosition.x -= dashXDist;
//
//				
//				var cast = Physics2D.Linecast( (Vector2)this.transform.position - new Vector2(2,0),endPosition);
//				if(cast.collider != null && cast.collider.gameObject.name != null){
//					if(cast.collider.gameObject.name.Equals("BouncyWall")){
//						endPosition.x = cast.collider.bounds.max.x + 2;
//					} else if(LayerMask.LayerToName(cast.collider.gameObject.layer).Contains("Player")){
////						getTopParent(cast.collider.gameObject).GetComponentInChildren<PlayerBehavior>().ReduceHealth(dashDamage);
////						PlayerEvents.RecordAttack(getTopParent(cast.collider.gameObject),getTopParent(this.gameObject),dashDamage);
//					} else if (!cast.collider.gameObject.name.Contains("Rock") 
//					           && !cast.collider.gameObject.tag.Equals("PointLightSpawnPoint") 
//					           && !cast.collider.gameObject.name.Contains("PointLight")) {
//						endPosition = cast.point + new Vector2(2,0);
//					} else {
//					}
//				}
//
//
//				dashMovement  = new Movement(this.gameObject);
//				dashMovement.AddLine(startPosition,endPosition,dashDuration);
//				dashMovement.Start();
//				StartCoroutine(Trail(Time.time, Time.time + dashDuration));
//
//				healthBar.Dash -= 0.5f;
//
//			}
//			
//			else if (rightDash) {
//
//				var startPosition = transform.position;
//				var endPosition = startPosition;
//				var startTime = Time.time;
//
//				
//				endPosition.x += dashXDist;
//
//				
//				var cast = Physics2D.Linecast( (Vector2)this.transform.position + new Vector2(2,0),endPosition);
//				if(cast.collider != null && cast.collider.gameObject.name != null){
//					if(cast.collider.gameObject.name.Equals("BouncyWall")){
//						endPosition.x = cast.collider.bounds.min.x - 2;
//					} else if(LayerMask.LayerToName(cast.collider.gameObject.layer).Contains("Player")){
////						getTopParent(cast.collider.gameObject).GetComponentInChildren<PlayerBehavior>().ReduceHealth(dashDamage);
////						PlayerEvents.RecordAttack(getTopParent(cast.collider.gameObject),getTopParent(this.gameObject),dashDamage);
//					} else if (!cast.collider.gameObject.name.Contains("Rock") 
//					              && !cast.collider.gameObject.tag.Equals("PointLightSpawnPoint") 
//					              && !cast.collider.gameObject.name.Contains("PointLight")) {
//						endPosition = cast.point - new Vector2(2,0);
//					} else {
//
//					}
//				}
//
//				dashMovement  = new Movement(this.gameObject);
//				dashMovement.AddLine(startPosition,endPosition, dashDuration);
//				dashMovement.Start();
//				StartCoroutine(Trail(Time.time, Time.time + dashDuration));
//
//				healthBar.Dash -= 0.5f;

//			}
			
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if(h * rigidbody2D.velocity.x < maxSpeed)
				// ... add a force to the player.
				rigidbody2D.AddForce(Vector2.right * h * moveForce);
			
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
		
		leftDash = false;
		rightDash = false;
		jump = false;
	}


	public void Jump(){
		var j = (Physics2D.gravity.y > 0)? -jumpForce : jumpForce;
		
		// Add a vertical force to the player.
		rigidbody2D.AddForce(new Vector2(0f, j));

	}

	public void TogglePointLight(){
		if (EventController.currentEvent != RunnableEventType.PointLights)
						return;

		var light = GetComponentInChildren<Light> ();
		light.enabled = !light.enabled;
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

	public Vector2 GetPosition() {
		return transform.position;
	}

	public string GetName() {
		return playerName;
	}
}
