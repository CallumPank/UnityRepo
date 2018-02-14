using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
	public float speed = 5f;
	[SerializeField]
	float lookSensitivity = 3f;
	public int playerID;
	public int playerScore;
	[SerializeField]
	AudioSource source;
	[SerializeField]
	float skateAudioDelayMin;
	[SerializeField]
	float skateAudioDelayMax;
	[SerializeField]
	AudioClip[] skateClips;

	[SerializeField]
	bool skateAudioPlaying;

	Rewired.Player player;

	public PowerUp currentPowerUp;

	[SerializeField]
	float noPowerUpVibrate;
	[SerializeField]
	float powerUpVibrate;
	[SerializeField]
	float powerUpVibrateDuration;

	//Component Caching
	PlayerMotor motor;

	void Start()
	{
		motor = GetComponent<PlayerMotor>();
		player = ReInput.players.GetPlayer(playerID);
		SetNameAndMat();
	}

	void Update()
	{

		//Calculate movement velocity as a 3D vector
		float _xMov = player.GetAxis("MoveHorizontal");
		float _zMov = player.GetAxis("MoveVertical");
		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		//Final movement vector
		Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

		//Apply movement
		motor.Move(_velocity);

		float _xRot = player.GetAxis("LookHorizontal");
		float _zRot = player.GetAxis("LookVertical");

		if (_xRot != 0 || _zRot != 0)
		{
			motor.Rotate(_xRot, _zRot);
		}

		if (_xMov != 0 && !skateAudioPlaying || _zMov != 0 && !skateAudioPlaying)
		{
			skateAudioPlaying = true;
			InvokeRepeating("SkateNoise", 0f, Random.Range(skateAudioDelayMin, skateAudioDelayMax));
		}
		else if (_zMov == 0 || _xMov == 0)
		{
			skateAudioPlaying = false;
			CancelInvoke("SkateNoise");
		}

		if (player.GetButtonDown("PowerUp"))
		{
			if (currentPowerUp == null)
			{
				Debug.Log("No current power up");
				ControllerVibrate(noPowerUpVibrate, powerUpVibrateDuration);
				return;
			}

			ControllerVibrate(powerUpVibrate, powerUpVibrateDuration);
			currentPowerUp.playerID = playerID;
			currentPowerUp.playerPos = transform.position;
			currentPowerUp.playerPos.y += 1f;
			currentPowerUp.ActivatePowerUp();
			StartCoroutine("DeactivatePowerUp");
		}
	}

	void SetNameAndMat()
	{
		gameObject.name = "player" + playerID + "(" + name + ")";
		gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = GameManager.instance.playerMats[playerID];
	}

	public void ControllerVibrate(float _intensity, float _duration)
	{
		player.SetVibration(0, _intensity, _duration);
	}

	void SkateNoise()
	{
		int rand = Random.Range(0, skateClips.Length);
		source.clip = skateClips[rand];
		source.Play();
	}

	IEnumerator DeactivatePowerUp()
	{
		yield return new WaitForSeconds(currentPowerUp.timeActive);
		if (currentPowerUp != null)
		{
			Debug.Log("deactivating");
			currentPowerUp.DeactivatePowerUp();
			currentPowerUp = null;
		}
	}
}
