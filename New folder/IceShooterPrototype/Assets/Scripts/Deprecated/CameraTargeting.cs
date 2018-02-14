using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargeting : MonoBehaviour
{
	float currentDistance;
	float largestDistance;
	Vector3 offset;
	[SerializeField]
	float smoothTime;

	Vector3 velocity;

	void Update()
	{
		Vector3 position = Vector3.zero;

		foreach (GameObject player in GameManager.instance.players)
		{
			position += player.transform.position;
		}

		position /= GameManager.instance.players.Count;

		float distance = ReturnLargestDifference();

		offset.y = Mathf.Clamp(distance, 13f, 30f);
		offset.z = -1 * (offset.y / 2.5f);

		transform.position = Vector3.SmoothDamp(transform.position, position + offset, ref velocity, smoothTime);
	}

	float ReturnLargestDifference()
	{
		currentDistance = 0f;
		largestDistance = 0f;

		for (int i = 0; i < GameManager.instance.players.Count; i++)
		{
			for (int x = 0; x < GameManager.instance.players.Count; x++)
			{
				currentDistance = Vector3.Distance(GameManager.instance.players[i].transform.position, GameManager.instance.players[x].transform.position);

				if (currentDistance > largestDistance)
				{
					largestDistance = currentDistance;
				}
			}
		}

		return largestDistance;
	}
}
