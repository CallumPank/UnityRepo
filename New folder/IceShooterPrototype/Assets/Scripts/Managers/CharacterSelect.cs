// Player presses A to join game
//Can then scroll down/up to go through characters
//Once decided on Character, hit A to ready up
//Character is then blocked for other players
//Need transitions in Animator for all possible scenarios (Hockey -> Polar with Figure taken for e.g)
//Once at least 2 players are readied up, one can press A to start game
//Players can hit B whilst readied up to change character


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
	public int maxPlayers = 4;

	int currentPlayerCount = 0;

	[SerializeField]
	Button startButton;
	[SerializeField]
	GameObject[] joinText;
	[SerializeField]
	GameObject[] readyText;
	[SerializeField]
	GameObject[] playerImgs;
	List<Animator> imgsAnims = new List<Animator>();

	void Start()
	{
		startButton.enabled = false;

		for (int i = 0; i < playerImgs.Length; i++)
		{
			imgsAnims.Add(playerImgs[i].GetComponent<Animator>());
		}
	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < ReInput.players.playerCount; i++)
		{
			if (ReInput.players.GetPlayer(i).GetButtonDown("JoinGame"))
			{
				AssignNextPlayer(i);
			}

			if (playerImgs[i].activeSelf && !readyText[i].activeSelf)
			{
				CheckForSelection(i);

				if (ReInput.players.GetPlayer(i).GetAxis("MoveVertical") > 0.1f)
				{
					ScrollUpImgs(i);
				}
				else if (ReInput.players.GetPlayer(i).GetAxis("MoveVertical") < -0.1f)
				{
					ScrollDownImgs(i);
				}
				else if (ReInput.players.GetPlayer(i).GetAxis("MoveVertical") == 0f)
				{
					CancelScroll(i);
				}
			}
		}

		CheckAllPlayersReady();
	}

	void AssignNextPlayer(int rewiredPlayerID)
	{
		if (PlayerManager.instance.playerList.Count >= maxPlayers)
		{
			Debug.LogError("Max player limit already reached");
			return;
		}

		foreach (Player p in PlayerManager.instance.playerList)
		{
			if (p.rewiredPlayerID == rewiredPlayerID)
			{
				return;
			}
		}

		int gamePlayerID = GetNextGamePlayerID();

		Player player = new Player(rewiredPlayerID, gamePlayerID);

		PlayerManager.instance.playerList.Add(player);

		Rewired.Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerID);
		rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Assignment");
		rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default");

		Debug.Log("Added Rewired Player id " + rewiredPlayerID + " to game player " + gamePlayerID);
		Debug.Log(PlayerManager.instance.playerList[0]);

		PlayerJoin(rewiredPlayerID);
	}

	void PlayerJoin(int _ID)
	{
		joinText[_ID].SetActive(false);
		playerImgs[_ID].SetActive(true);
	}

	private int GetNextGamePlayerID()
	{
		return currentPlayerCount++;
	}

	public void StartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	void ScrollUpImgs(int playerID)
	{
		if (imgsAnims[playerID].GetBool("up") != true)
		{
			imgsAnims[playerID].SetBool("up", true);
		}
	}

	void ScrollDownImgs(int playerID)
	{
		if (imgsAnims[playerID].GetBool("down") != true)
		{
			imgsAnims[playerID].SetBool("down", true);
		}
	}

	void CancelScroll(int playerID)
	{
		if (imgsAnims[playerID].GetBool("up") == true)
		{
			imgsAnims[playerID].SetBool("up", false);
		}

		if (imgsAnims[playerID].GetBool("down") == true)
		{
			imgsAnims[playerID].SetBool("down", false);
		}
	}

	void CheckForSelection(int playerID)
	{
		List<GameObject> images = new List<GameObject>();

		foreach (Transform transform in playerImgs[playerID].transform)
		{
			if (transform.GetComponent<Image>() != null)
			{
				images.Add(transform.gameObject);
			}
		}

		if (ReInput.players.GetPlayer(playerID).GetButtonDown("UISubmit"))
		{
			Character character;
			for (int i = 0; i < images.Count; i++ )
			{
				if (images[i].activeSelf)
				{
					switch (i)
					{
						case 0:
							character = new Character(playerID, 0);
							PlayerManager.instance.charactersList.Add(character);
							break;
						case 1:
							character = new Character(playerID, 1);
							PlayerManager.instance.charactersList.Add(character);
							break;
						case 2:
							character = new Character(playerID, 2);
							PlayerManager.instance.charactersList.Add(character);
							break;
						case 3:
							character = new Character(playerID, 3);
							PlayerManager.instance.charactersList.Add(character);
							break;
					}
				}
			}
			readyText[playerID].SetActive(true);
		}
	}

	void CheckAllPlayersReady()
	{
		int amountActive = 0;

		for (int i = 0; i < readyText.Length; i++)
		{
			if (readyText[i].activeSelf)
			{
				amountActive++;
			}
		}

		if (amountActive == PlayerManager.instance.playerList.Count && PlayerManager.instance.playerList.Count > 0)
		{
			//StartCoroutine("EnableStartButton");

			startButton.enabled = true;
		}
	}

	IEnumerator EnableStartButton()
	{
		yield return new WaitForSeconds(0.2f);
		startButton.enabled = true;
	}
}
