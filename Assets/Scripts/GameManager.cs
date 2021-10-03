using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[Header("Balancing")]
	public int baseHP;
	public float defaultTime = 100;


	[Header("Settings/debug")]
	public GameState gameState;
	public float currentTime;
	public float gameSpeed;
	Camera mainCam;

	[Header("Families")]
	public List<float> familiesScores;
	public List<EnemyFamily> families;

	[Header("References")]
	public Text hpBaseTextDisplay;
	public List<MonsterSpawner> spawners;

	[Header("Economy")]
	public float currentMoney = 1000;
	public Action<float> moneyVaritation;
	public Mb_Tower currentSelectionedTowerType;


	[Serializable]
	public struct EnemyFamily
	{
		public EnemyBehavior.EnemyType enemy;
		public GameObject prefab;
		public float basicDelay;
	}

	public enum GameState { InFight, Preparation, LawVoting }

	void Awake ()
	{
		Instance = this;
		Init();
		mainCam = Camera.main;
	}

	void Init ()
	{
		currentTime = defaultTime;
		moneyVaritation += AddMoney;
		//DeclareWaves();
	}

	void FixedUpdate ()
	{
		currentTime -= gameSpeed / 50;


	}


	public void DeclareWaves ()
	{
		List<int> spawnableFamilies = new List<int>();


		for (int i = 0; i < familiesScores.Count; i++)
		{

			if (familiesScores[i] > 0)
			{
				spawnableFamilies.Add(0);

			}
		}

		List<int> _subspawnableFamilies = spawnableFamilies;

		for (int i = 0; i < spawners.Count; i++)
		{

			spawners[i].monstersToSpawn.Capacity = familiesScores.Capacity;
			//spawners[i].monstersToSpawn[_spawnableFamilies[0]] = true;
			//_spawnableFamilies.Remove(0);

			//if(_spawnableFamilies.Capacity == 0)
			//{
			//    _spawnableFamilies = spawnableFamilies;
			//}
			//spawners[i].Init();
		}

	}

	private void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 10000, 1 << 6))
			{
				hit.collider.GetComponent<Interactible>().Interract();
			}
		}
	}

	void AddMoney ( float _moneyToAdd )
	{
		currentMoney += _moneyToAdd;
	}
}


