using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{

	#region Variables

	Vector3 velocity = Vector3.zero;
	Vector3 rotation = Vector3.zero;

	[SerializeField]
	float maxSpeed;
	[SerializeField]
	float slerpTime;
	[SerializeField]
	GameObject gfx;

	Rigidbody rb;
	CapsuleCollider capCollider;
	Animator anim;
	#endregion

	void Start()
	{
		//Cursor.visible = false;
		rb = GetComponent<Rigidbody>();
		capCollider = GetComponent<CapsuleCollider>();
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		anim.SetFloat("velocity", rb.velocity.magnitude / maxSpeed);
	}

	//Run every physics iteration
	void FixedUpdate()
	{
		PerformMovement();

		rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
	}

	//Gets a movement vector
	public void Move(Vector3 _velocity)
	{
		velocity = _velocity;
	}

	//Perform movement based on velocity variable
	void PerformMovement()
	{

		if (velocity != Vector3.zero)
		{

			//rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
			rb.AddRelativeForce(velocity * Time.fixedDeltaTime);
		}

	}

	//Gets the rotation vector
	public void Rotate(float _xMove, float _zMove)
	{
		float angle = Mathf.Atan2(_xMove, _zMove) * Mathf.Rad2Deg;
		Quaternion target = Quaternion.Euler(0, angle, 0);

		gfx.transform.rotation = Quaternion.Slerp(gfx.transform.rotation, target, slerpTime);
	}

	public void ApplyKnockback(float _force)
	{
		rb.AddRelativeForce(-gfx.transform.forward * _force * Time.fixedDeltaTime, ForceMode.VelocityChange);
	}

}
