using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

sealed class GameManager : MonoBehaviour
{
	#region Singleton
	public static GameManager instance;
	void Awake()
	{
		if (instance != null)
		{
			GameObject.Destroy(this.gameObject);
			return;
		}

		instance = this;
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
	#endregion

	public List<Transform> spawnPositions;
	public List<GameObject> players;
	public float maxRoundTime;
	public float currentRoundTime;
	public Material[] playerMats;
	public GameObject[] playerPrefab;
	public int numberOfPlayers;
	public bool waveMode = false;
	public bool hockeyMode;
	public GameObject puckSpawner;
	public GameObject puckPrefab;
	public GameObject ragdollPlayerPrefab;

	public Camera cam;

	GameObject playerManager;

	UIManager uiManager;

	void Start()
	{
		//if (waveMode) { maxLevelTime = Mathf.Infinity - 1f; }
		//else { GameObject.Find("WavePannel").SetActive(false); }
		playerManager = GameObject.Find("PlayerManager");
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

		if (hockeyMode)
		{
			cam = Camera.main;
			SpawnPlayers();
			gameObject.AddComponent<HockeyManager>();
			currentRoundTime = HockeyManager.instance.maxRoundTime;
			uiManager.ActivateHockeyMode();
			uiManager.PopulateWeaponText();
		}
		else
		{
			currentRoundTime = maxRoundTime;
			cam = Camera.main;
			SpawnPlayers();
			uiManager.PopulateWeaponText();
		}

	}

	void Update()
	{
		currentRoundTime -= Time.deltaTime;

		if (currentRoundTime <= 0f)
		{
			GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().CompareScore();
		}

		if (players.Count <= 1 && !waveMode)
		{
			currentRoundTime = 0f;
		}
	}

	public void EnablePlayer(GameObject _player)
	{
		_player.GetComponent<PlayerController>().enabled = true;
		_player.GetComponent<PlayerMotor>().enabled = true;
		_player.GetComponent<Shooting>().enabled = true;
		MeshRenderer[] meshRends = _player.GetComponentsInChildren<MeshRenderer>();
		SkinnedMeshRenderer[] skinRends = _player.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < meshRends.Length; i++)
		{
			meshRends[i].enabled = true;
		}
		for (int i = 0; i < skinRends.Length; i++)
		{
			skinRends[i].enabled = true;
		}
		_player.GetComponentInChildren<CapsuleCollider>().enabled = true;
	}

	public void DisablePlayer(GameObject _player)
	{
		_player.GetComponent<PlayerController>().enabled = false;
		_player.GetComponent<PlayerMotor>().enabled = false;
		_player.GetComponent<Shooting>().enabled = false;
		MeshRenderer[] meshRends = _player.GetComponentsInChildren<MeshRenderer>();
		SkinnedMeshRenderer[] skinRends = _player.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < meshRends.Length; i++)
		{
			meshRends[i].enabled = false;
		}
		for (int i = 0; i < skinRends.Length; i++)
		{
			skinRends[i].enabled = false;
		}
		_player.GetComponentInChildren<MeshRenderer>().enabled = false;
		_player.GetComponentInChildren<CapsuleCollider>().enabled = false;
	}

	void SpawnPlayers()
	{

		spawnPositions.Clear();

		GameObject[] spawnArray = GameObject.FindGameObjectsWithTag("SpawnPos");

		for (int i = 0; i < spawnArray.Length; i++)
		{
			spawnPositions.Add(spawnArray[i].transform);
		}

		players.Clear();
		if (playerManager == null)
		{
			for (int i = 0; i < numberOfPlayers; i++)
			{
				GameObject ins = Instantiate(playerPrefab[i], spawnPositions[i].transform.position, Quaternion.identity);
				ins.GetComponent<PlayerController>().playerID = i;
				ins.GetComponent<PlayerHealth>().ResetHealth();
				ins.name = playerPrefab[i].name; // "player" + i;
				players.Add(ins);
				uiManager.UpdateLives(i);
				uiManager.UpdateScore(i);
			}
		}
		else if (playerManager != null)
		{
			for (int i = 0; i < PlayerManager.instance.playerList.Count; i++)
			{
				GameObject ins = Instantiate(playerPrefab[i], spawnPositions[i].transform.position, Quaternion.identity);
				ins.GetComponent<PlayerController>().playerID = PlayerManager.instance.playerList[i].rewiredPlayerID;
				ins.name = playerPrefab[i].name; //"player" + i;
				players.Add(ins);
				uiManager.UpdateLives(i);
				uiManager.UpdateScore(i);
				uiManager.SetPortrait(i, PlayerManager.instance.RetrievePortrait(i));

			}
		}

		cam.GetComponent<MultipleTargetCamera>().GetPlayers();
	}

	public void LevelReset()
	{
		for (int i = 0; i < players.Count; i++)
		{
			Destroy(players[i]);
		}
		DestroyBullets();
		DestroyPickUps();
		SpawnPlayers();
		uiManager.SetUI();
		currentRoundTime = maxRoundTime;
	}

	public GameObject RetrievePlayer(int _playerID)
	{
		GameObject gameObject = null;

		for (int i = 0; i < GameManager.instance.players.Count; i++)
		{
			if (GameManager.instance.players[i].GetComponent<PlayerController>() != null)
			{
				if (GameManager.instance.players[i].GetComponent<PlayerController>().playerID == _playerID)
				{
					gameObject = GameManager.instance.players[i];
				}
			}
		}

		return gameObject;
	}

	void DestroyBullets()
	{
		GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
		for (int i = 0; i < bullets.Length; i++)
		{
			Destroy(bullets[i]);
		}
	}

	void DestroyPickUps()
	{
		GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
		for (int i = 0; i < pickUps.Length; i++)
		{
			Destroy(pickUps[i]);
		}
	}
}
