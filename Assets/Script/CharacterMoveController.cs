using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{

	[Header("Movement")]
	public float moveAccel;
	public float maxSpeed;

	[Header("Jump")]
	public float jumpAccel;

	[Header("Ground Raycast")]
	public float groundRaycastDistance;
	public LayerMask groundLayerMask;

	private Rigidbody2D rig;
	private bool isJumping;
	private bool isOnGround;
	private Animator anim;


	// Start is called before the first frame update
	private void Start()
    {
		rig = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
    }

	private void FixedUpdate()
	{
		// raycast ground
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);

		if (hit)
		{
			if (!isOnGround && rig.velocity.y <= 0)
			{
				isOnGround = true;
			}
		}
		else
		{
			isOnGround = false;
		}

		// calculate velocity vector
		Vector2 velocityVector = rig.velocity;

		if (isJumping)
		{
			velocityVector.y += jumpAccel;
			isJumping = false;
		}

		velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);

		rig.velocity = velocityVector;
	}

	private void OnDrawGizmos()
	{
		Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
	}



	private void Update()
	{

		if (Input.GetMouseButtonDown(0))
		{
			if (isOnGround)
			{
				isJumping = true;
			}
		}

		anim.SetBool("isOnGround", isOnGround);
	}
}
