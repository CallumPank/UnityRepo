using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HockeyGoal : MonoBehaviour
{
	[SerializeField]
	int teamID;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Puck")
		{
			HockeyManager.instance.IncreaseScore(teamID);

			//Explode puck
			//Throw players back
			//Reset for new puck drop
		}
	}
}
