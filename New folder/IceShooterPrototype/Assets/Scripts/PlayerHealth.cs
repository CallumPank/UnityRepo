using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	public float playerHealth;
	public float maxHealth;
	PlayerArmour playerArmour;

	[SerializeField]
	float respawnTime;
	[SerializeField]
	float invincibilityTimer;
	[SerializeField]
	float invincibilityTime;
	[SerializeField]
	int maxLives;

	ParticleSystem bloodSpray;

	public int playerLives;

	bool respawning = false;

	MeshRenderer[] rends;

	[SerializeField]
	AudioClip injuredClip;
	[SerializeField]
	AudioClip deathClip;
	[SerializeField]
	AudioSource source;

	int playerID;

	UIManager uiManager;

	public bool alive;

	void Awake()
	{
		alive = true;
		playerHealth = maxHealth;
		playerLives = maxLives;
		bloodSpray = GetComponentInChildren<ParticleSystem>();
		invincibilityTimer = invincibilityTime;
		rends = GetComponentsInChildren<MeshRenderer>();

		//		// Get reference to player amour
		//		GameObject thePlayer = GameObject.FindGameObjectWithTag("Player");
		//		PlayerArmour playerArmour = thePlayer.GetComponent<PlayerArmour>();
		//		print (playerArmour.ArmourAmount);

	}

	void Start()
	{
		playerID = gameObject.GetComponent<PlayerController>().playerID;
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
	}



	// Update is called once per frame
	void Update()
	{
		invincibilityTimer += Time.deltaTime;

		if (playerHealth <= 0f && !respawning)
		{
			PlayDeathAudio();
			respawning = true;
			GameManager.instance.DisablePlayer(gameObject);
			alive = false;
			GameManager.instance.cam.GetComponent<MultipleTargetCamera>().GetPlayers();

			if (GameManager.instance.hockeyMode)
			{
				StartCoroutine("Respawn");
			}
			else
			{
				playerLives -= 1;
				uiManager.UpdateLives(playerID);
				if (playerLives > 0)
				{
					StartCoroutine("Respawn");
				}
				else if (playerLives <= 0)
				{
					GameManager.instance.DisablePlayer(gameObject);
					GameManager.instance.players.Remove(this.gameObject);
					GameManager.instance.cam.GetComponent<MultipleTargetCamera>().GetPlayers();
				}
			}

		}
	}

	public void TakeDamage(float _damage)
	{
		if (invincibilityTimer >= invincibilityTime)
		{
			PlayerArmour playerArmour = GetComponent<PlayerArmour>();
			//print("Damaged Player Armour = " + playerArmour.armorAmount);
			if (playerArmour.armorAmount > 0.0f)
			{
				playerArmour.armorAmount -= _damage;
				uiManager.UpdateArmor(playerID);
				// if armour goes below 0 take that from health 
				if (playerArmour.armorAmount < 0.0f)
				{
					playerHealth += playerArmour.armorAmount;
					// Reset Armour back to 0
					playerArmour.armorAmount = 0.0f;
				}
			}
			else
			{
				playerHealth -= _damage;
				PlayInjuredAudio();
				uiManager.UpdateHealth(playerID);
			}

			GetComponent<PlayerController>().ControllerVibrate(0.5f, 0.5f);
			bloodSpray.Play();

		}

		if (playerHealth <= 0f)
		{
			GameObject ragdollIns = Instantiate(GameManager.instance.ragdollPlayerPrefab, transform.position, transform.rotation);
			ragdollIns.GetComponentInChildren<SkinnedMeshRenderer>().material = GameManager.instance.playerMats[playerID];
			StartCoroutine(DestroyRagdoll(ragdollIns));
		}
	}

	public void AddHealth(float _health)
	{
		playerHealth += _health;
		playerHealth = Mathf.Clamp(playerHealth, 0, maxHealth);
	}

	IEnumerator Respawn()
	{
		bool hasRespawned = false;
		yield return new WaitForSeconds(respawnTime);
		if (!hasRespawned)
		{
			alive = true;
			int rand = Random.Range(0, GameManager.instance.spawnPositions.Count - 1);
			transform.position = GameManager.instance.spawnPositions[rand].transform.position;
			playerHealth = maxHealth;
			GetComponent<Shooting>().EquipDefaultWeapon();
			GetComponent<Shooting>().CancelInvoke();
			GameManager.instance.EnablePlayer(gameObject);
			hasRespawned = true;
			respawning = false;
			invincibilityTimer = 0f;
			uiManager.UpdateHealth(playerID);
			uiManager.UpdateArmor(playerID);
			uiManager.UpdateAmmo(playerID);
			GameManager.instance.cam.GetComponent<MultipleTargetCamera>().GetPlayers();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Spikes")
		{
			TakeDamage(50);
		}

		if (other.tag == "Sea")
		{
			TakeDamage(200);
		}
	}

	void PlayInjuredAudio()
	{
		source.clip = injuredClip;
		source.Play();
	}

	void PlayDeathAudio()
	{
		source.Stop();
		source.clip = deathClip;
		source.Play();
	}

	public void ResetHealth()
	{
		playerHealth = maxHealth;
	}

	IEnumerator DestroyRagdoll(GameObject ins)
	{
		yield return new WaitForSeconds(respawnTime - 0.2f);
		Destroy(ins);
	}
}
