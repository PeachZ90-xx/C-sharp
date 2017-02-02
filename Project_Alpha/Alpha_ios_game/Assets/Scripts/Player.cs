/*
This code and all other parts of the game Earth, INC. is licensed and copyrighted by
Clockwork Void and it's affiliates Alan Treviño, Raul Eddie Elizondo, and Braeden Wright.


*/

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (BoxCollider2D))]
public class Player : MonoBehaviour {


    private Rigidbody2D myRigidbody;

	//This is from the old wall sliding code.
	//unknown if it still works.
	Vector2 directionalInput;
	bool wallSliding;
	int wallDirX;

	//movement and sprite facing
	[SerializeField]
	private float movementSpeed; 
	private bool facingRight;

	//jump code
	[SerializeField]
	private Transform[] groundPoints;
	[SerializeField]
	private float groundRadius;
	[SerializeField]
	private LayerMask whatIsGround;
	private bool jump;
	private bool isGrounded;
	[SerializeField]
	private bool airControl;
	[SerializeField]
	private float jumpForce;

	public bool grounded;
	public bool doubleJump;

	void Start() {
        myRigidbody = GetComponent<Rigidbody2D>();
		facingRight = true;
        
       }

	//this checks for everything else that is not in the
	//FixedUpdate function. i.e. other inputs
	void Update() {

		HandleInput (); 
	}

    //this will set a constant update for the game.
	//This update runs 50 times per second
	void FixedUpdate() {

		float horizontal= Input.GetAxis ("Horizontal");
		Debug.Log (horizontal);

		isGrounded = IsGrounded();

		HandleMovement (horizontal);

		Flip (horizontal);

		ResetValues ();

    }


	//reset of values
	private void ResetValues(){
		jump = false;
	}


    //Player movement
	private void HandleMovement(float horizontal)
    {


		myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
		if (isGrounded && jump) {

			isGrounded = false;
			myRigidbody.AddForce (new Vector2 (0, jumpForce));
		}
	}


	private void HandleInput()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			jump = true;
			doubleJump = true;
		} else {

			if (doubleJump) {
				//double jump
				doubleJump = false;
				myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, 0);
				myRigidbody.AddForce (Vector2.up * jumpForce);

			}
		}


	}

    
	//this flips the sprite and checks the value of the relative scale.
	private void Flip (float horizontal)
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {

			//opposite value creator
			facingRight = !facingRight;

			//this flips the sprite
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

	}

	private bool IsGrounded(){
		if (myRigidbody.velocity.y <= 0) {
			foreach (Transform point in groundPoints) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);


				for (int i = 0; i < colliders.Length; i++) {
					if (colliders [i].gameObject != gameObject) {
						//this checks to see if we are colliding with the ground
						return true;
					}
				}
			
			}
		}
		//if we are not colliding with something.
		return false;
	}

}