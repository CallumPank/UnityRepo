using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
	[SerializeField]
	GameObject powerUp;

	PickUpController controller;

	// Use this for initialization
	void Start()
	{
		controller = GameObject.Find("PickUpSpawns").GetComponent<PickUpController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerController playerCont = other.transform.root.GetComponent<PlayerController>();

			if (playerCont != null && playerCont.currentPowerUp == null)
			{
				playerCont.currentPowerUp = powerUp.GetComponent<PowerUp>();
				powerUp.GetComponent<PowerUp>().activated = false;
				controller.CurrentNumberOfPickups -= 1;
				Destroy(gameObject);
			}
		}
	}
}
