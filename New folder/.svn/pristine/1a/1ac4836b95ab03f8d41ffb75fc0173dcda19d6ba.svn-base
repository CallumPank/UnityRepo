using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
	[SerializeField]
	float upForce;
	[SerializeField]
	float forwardForce;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Rigidbody rb = other.transform.root.GetComponent<Rigidbody>();

			if (rb != null)
			{

				//Vector3 force = new Vector3(0, upForce, forwardForce);
				Vector3 forwardVector = transform.forward * forwardForce;
				Vector3 upVector = transform.up * upForce;
				Vector3 force = forwardVector + upVector;
				rb.AddForce(force * Time.deltaTime, ForceMode.Impulse);
			}
		}
	}
}
