using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLights : MonoBehaviour
{
	[SerializeField]
	int playerID;
	GameObject player;
	[SerializeField]
	float smoothTime;
	// Update is called once per frame
	void Update()
	{
		player = GameManager.instance.RetrievePlayer(playerID);
		if (GameManager.instance.RetrievePlayer(playerID) == null)
		{
			gameObject.SetActive(false);
			return;
		}

		Vector3 pos = player.transform.position;
		pos.y += 20;
		transform.position = Vector3.Lerp(transform.position, pos, smoothTime);
	}
}
