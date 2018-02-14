using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float damage;
	public int playerID;
	public float forceAmount;
	public int maxCollisions;

	private int currentCollisions;

	void Start()
	{
		currentCollisions = 0;
	}

	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.tag == "Bullet" || other.gameObject.tag == "PickUp" || other.gameObject.tag == "PickUpSpawn")
			return;

		Rigidbody rb = other.gameObject.GetComponentInParent<Rigidbody>();

		if (rb != null)
		{
			ApplyForce(rb);
		}

		if (other.gameObject.tag == "Puck")
		{
			Destroy(gameObject);
		}

		if (other.gameObject.tag == "Player")
		{
			DamagePlayer(other.gameObject);
			Destroy(gameObject);
			return;
		}

		if (other.gameObject.tag == "Zombie")
		{
            DamageEnemy(other.gameObject);
            //Destroy(other.gameObject);
			Destroy(gameObject);
			return;
		}
		
		currentCollisions++;
		CheckCollisions();
	}

	void DamagePlayer (GameObject _obj)
	{
		PlayerHealth playerHealth = _obj.GetComponentInParent<PlayerHealth> ();

		if (playerHealth != null)
		{
			playerHealth.TakeDamage (damage);

			if (playerHealth.playerHealth <= 0f)
			{
				for (int i = 0; i < GameManager.instance.players.Count; i++)
				{
					if (GameManager.instance.players[i].GetComponent<PlayerController> () != null)
					{
						if (GameManager.instance.players[i].GetComponent<PlayerController> ().playerID == playerID)
						{
							if (_obj.GetComponentInParent<PlayerController>().playerID == playerID)
							{
								return;
							}

							GameManager.instance.players[i].GetComponent<PlayerController> ().playerScore += 1;
							GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().UpdateScore(playerID);
						}
					}
				}
			}
		}
	}

    void DamageEnemy(GameObject _obj)
    {
        ZombieHealth zombieHealth = _obj.GetComponentInParent<ZombieHealth>();

        if (zombieHealth != null)
        {
            zombieHealth.TakeDamage(damage * 2.5f);

            if (zombieHealth.health <= 0f)
            {
                for (int i = 0; i < GameManager.instance.players.Count; i++)
                {
                    if (GameManager.instance.players[i].GetComponent<PlayerController>() != null)
                    {
                        if (GameManager.instance.players[i].GetComponent<PlayerController>().playerID == playerID)
                        {
                            //print("player " + i + " killed enemy");
                            GameManager.instance.players[i].GetComponent<PlayerController>().playerScore += 100;
                            GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().UpdateScore(playerID);
                            print(GameManager.instance.players[i].GetComponent<PlayerController>().playerScore);
                            if (_obj.GetComponentInParent<PlayerController>().playerID == playerID)
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    void CheckCollisions()
	{
		if (currentCollisions >= maxCollisions)
		{
			Destroy(gameObject);
		}
	}

	void ApplyForce(Rigidbody _rb)
	{
		_rb.AddForce(transform.forward * forceAmount);
	}

}
