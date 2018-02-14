using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HockeyManager : MonoBehaviour
{
	#region Singleton 
	public static HockeyManager instance;

	void Awake()
	{
		instance = this;
	}
	#endregion

	public float maxRoundTime = 60f;
	public List<GameObject> teamOne = new List<GameObject>();
	public List<GameObject> teamTwo = new List<GameObject>();
	public int teamOneScore;
	public int teamTwoScore;
	UIManager uiManager;

	Transform puckSpawner;
	GameObject puckPrefab;

	// Use this for initialization
	void Start()
	{
		puckSpawner = GameManager.instance.puckSpawner.transform;
		puckPrefab = GameManager.instance.puckPrefab;
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
		teamOne.Add(GameManager.instance.players[0]);
		teamTwo.Add(GameManager.instance.players[1]);
		PuckDrop();
	}

	public void IncreaseScore(int i)
	{
		if (i == 1)
		{
			teamOneScore++;
			uiManager.UpdateTeamHockeyScore(i);
		}
		else if (i == 2)
		{
			teamTwoScore++;
			uiManager.UpdateTeamHockeyScore(i);
		}
	}

	void PuckDrop()
	{
		Instantiate(puckPrefab, puckSpawner.position, Quaternion.identity);
	}
}
