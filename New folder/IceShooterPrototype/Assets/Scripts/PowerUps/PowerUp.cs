using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	public float timeActive;
	[System.NonSerialized]
	public int playerID;
	[System.NonSerialized]
	public bool activated;
	public Vector3 playerPos;

	public virtual void ActivatePowerUp()
	{
		Debug.Log(gameObject + " activated");
	}

	public virtual void DeactivatePowerUp()
	{
		Debug.Log(gameObject + " deactivated");
	}
}
