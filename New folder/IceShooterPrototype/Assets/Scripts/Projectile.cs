using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float damage;
	public int playerID;
	public float forceAmount;
	public float areaOfEffect;
	public float damageDropoff;
	public GameObject explosionPrefab;

	void OnCollisionEnter(Collision other)
	{
		ContactPoint contact = other.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		Instantiate(explosionPrefab, pos, rot);
		CheckForPlayers(pos);
		Destroy(gameObject);
	}

	void CheckForPlayers(Vector3 _pos)
	{
		Collider[] colliders = Physics.OverlapSphere(_pos, areaOfEffect);

		foreach (Collider hit in colliders)
		{
			Rigidbody rb = hit.GetComponentInParent<Rigidbody>();
			
			if (rb != null)
			{
				rb.AddExplosionForce(forceAmount, _pos, areaOfEffect, 0f);
			}

			if (hit.gameObject.tag == "Player")
			{
				DamagePlayer(hit.gameObject);
			}
            if (hit.gameObject.tag == "Zombie")
            {
                Destroy(hit.gameObject);
            }
		}
	}

	void DamagePlayer(GameObject _obj)
	{
		PlayerHealth playerHealth = _obj.GetComponentInParent<PlayerHealth>();

		if (playerHealth != null)
		{
			playerHealth.TakeDamage(damage);
			Debug.Log(_obj.transform.parent.name + " has taken " + damage + " damage from Player" + playerID);

			if (playerHealth.playerHealth <= 0f)
			{
				for (int i = 0; i < GameManager.instance.players.Count; i++)
				{
					if (GameManager.instance.players[i].GetComponent<PlayerController>() != null)
					{
						if (GameManager.instance.players[i].GetComponent<PlayerController>().playerID == playerID)
						{
							if (_obj.GetComponentInParent<PlayerController>().playerID == playerID)
							{
								return;
							}

							GameManager.instance.players[i].GetComponent<PlayerController>().playerScore += 1;
							GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().UpdateScore(playerID);
						}
					}
				}
			}
		}
	}
}
