using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering.HighDefinition;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[Header("Balancing")]
	public int baseHP;
	public float defaultTime = 100;
	public float difficulty;
	public float damageMultiplier = 1;
	public float policeOdds = 0;

	public float slowDuration;
	public float slowPercent;
	public float freezeDuration;
	public float poisonDuration;
	public float poisonDamage;



	[Header("Settings/debug")]
	public GameState gameState = GameState.LawVoting;
	public float currentTime;
	public float gameSpeed;
	public float mobSpeedMultiplier = 1;
	public List<EnemyBehavior> currentEnemies;
	public bool raining;
	public float rainProbability;
	public float solarMultiplier = 1;
	public int day = 1;

	[Header("Families")]
	public List<float> familiesScores;
	public List<float> spawningAvoidance;
	public List<EnemyFamily> families;

	[Header("References")]
	public TMP_Text hpBaseTextDisplay;
	public TMP_Text dayTextDisplay;
	public TMP_Text moneyTextDisplay;
	public Text timeTextDisplay;
	public List<MonsterSpawner> spawners;
	public List<MonsterSpawner> grassSpawners;
	Camera mainCam;
	public Animator gameUIAnimator;
	public List<GameObject> policeCarSpawner;
	public GameObject policeCar;
	public Light lightRef;


	[Header("Economy")]
	public float moneyPerDay = 1000;
	public float currentMoney = 1000;
	public Action<float> moneyVaritation;
	[HideInInspector]public Mb_Tower currentSelectionedTowerType;

	//unlock and tower part
	public List<GameObject> turrets;
	public List<Mb_Tower> prefabTurrets;


	public enum GameState { InFight, Preparation, LawVoting, dayEnd, gameIntro, defeat }

	[Serializable]
	public struct EnemyFamily
	{
		public GameObject prefab;
		public EnemyBehavior.EnemyType type;
		public float basicDelay;
	}





	void Awake ()
	{
		Instance = this;
		Init();
		//StartLawVotting();
		mainCam = Camera.main;
		gameState = GameState.gameIntro;
		CameraHandler.Instance.GetComponent<Animator>().Play("Intro");

		lightHD = lightRef.GetComponent<HDAdditionalLightData>();
	}



	HDAdditionalLightData lightHD;
	void UpdateLight ()
	{
		if (raining)
		{
			solarMultiplier = 0.11f;
		}
		else
		{
			if (lightRef.colorTemperature < 20000)
			{
				lightRef.colorTemperature++;
				solarMultiplier = 1f;
			}
			else
			{
				solarMultiplier = 0.2f;
			}
			if (lightHD.intensity > 20)
			{
				lightHD.intensity -= 0.02f;
				solarMultiplier = 1f;
			}
			else
			{
				solarMultiplier = 0.2f;
			}
		}


	}

	void ResetLight ()
	{
		if (!raining)
		{
			lightRef.colorTemperature = 5500;
			lightHD.intensity = 40;
		}
		else
		{
			lightRef.colorTemperature = 20000;
			lightHD.intensity = 20;
		}

	}

	public void StartLawVotting ()
	{
		raining = false;
		ResetLight();
		day++;
		CameraHandler.Instance.GetComponent<Animator>().Play("LawVotting");
		gameUIAnimator.Play("LawVotting");
		LawManager.Instance.SetUpEventLaw(LawManager.Instance.PickRandomEvent());
		for (int i = 0; i < familiesScores.Capacity; i++)
		{
			if (familiesScores[i] < 0)
			{
				familiesScores[i] = 0;
			}
			if (familiesScores[i] > 1)
			{
				familiesScores[i] = 1;
			}

		}

		if (damageMultiplier <= 0.1f)
		{
			damageMultiplier = 0.1f;
		}

		if (policeOdds < 0)
		{
			policeOdds = 0;
		}
		if (policeOdds > 2)
		{
			policeOdds = 2;
		}

	}

	public void StartPreparation ()
	{
		if(UnityEngine.Random.Range(0,1f) < rainProbability)
        {
			raining = true;
        }
		else
        {
			raining = false;
        }
		CameraHandler.Instance.GetComponent<Animator>().Play("DefaultGame");
		ResetLight();
		gameUIAnimator.Play("Preparation");
		currentTime = defaultTime;
		gameState = GameState.Preparation;
		currentMoney += moneyPerDay;
		moneyVaritation += AddMoney;
		DeclareWaves();
	}

	public void StartFight ()
	{
		gameUIAnimator.Play("Combat");
		gameState = GameState.InFight;
	}

	public void DayEnds ()
	{
		gameState = GameState.dayEnd;
		StartLawVotting();
	}

	void Init ()
	{

	}

	void NexusLost()
    {
		mobSpeedMultiplier = 0;
		gameState = GameState.defeat;
		CameraHandler.Instance.GetComponent<Animator>().Play("NexusLost");
		gameUIAnimator.Play("NoneDisplayed");
		Destroy(this);

	}


	void FixedUpdate ()
	{
		if(baseHP ==0)
        {
			NexusLost();
        }
		if (gameState == GameState.InFight)
		{
			currentTime -= gameSpeed / 50;

			


			UpdateLight();



			if (currentTime < 0 && currentEnemies.Count == 0)
			{
				DayEnds();
			}

			if (UnityEngine.Random.Range(0, 1000) < policeOdds)
			{
				Instantiate(policeCar, policeCarSpawner[UnityEngine.Random.Range(0, 2)].transform);
			}
		}


	}

	public void DeclareWaves ()
	{
		List<MonsterSpawner> _spawners = spawners;

		if (LawManager.Instance.grassSpawners)
		{
			foreach (var item in grassSpawners)
			{
				_spawners.Add(item);
			}
		}

		List<int> _indexs = new List<int>();

		for (int j = 0; j < familiesScores.Count; j++)
		{
			if (familiesScores[j] > 0)
			{
				_indexs.Add(j);
			}
		}

		for (int i = 0; i < _spawners.Count; i++)
		{
			int _index = 0;

			for (int j = 0; j < 10; j++)
			{
				int _rand = UnityEngine.Random.Range(0, familiesScores.Capacity);
				if (familiesScores[_rand] == 0)
				{

				}
				else
				{
					if (0 == 1)
					{

					}
					else
					{
						_index = _rand;
						break;
					}

				}
			}

			_spawners[i].mob2 = families[_index].prefab;

		}

		for (int i = 0; i < _spawners.Count; i++)
		{
			int _index = 0;

			for (int j = 0; j < 10; j++)
			{
				int _rand = UnityEngine.Random.Range(0, familiesScores.Capacity);
				if (familiesScores[_rand] == 0)
				{

				}
				else
				{
					if (0 == 1) //spawningAvoidance[_rand] > 0)
					{

					}
					else
					{
						_index = _rand;
						break;
					}

				}
			}

			//print(_index);
			//int _index = _indexs[UnityEngine.Random.Range(0, _indexs.Capacity)];
			_spawners[i].mob = families[_index].prefab;
			_spawners[i].delayBetweenSpawn = families[_index].basicDelay;
			_spawners[i].familyScore = familiesScores[_index];
			_spawners[i].Init();
			_spawners[i].associatedTelegraph.theOne = _index;

		}


		//spawningAvoidance = new List<float>();

	}



	void AddMoney ( float _moneyAdded )
	{
		currentMoney += _moneyAdded;
	}


	// Update is called once per frame
	void Update ()
	{
		dayTextDisplay.text = "DAY " + day.ToString();
		moneyTextDisplay.text = currentMoney.ToString();
		hpBaseTextDisplay.text = baseHP.ToString() + " / 50";
		timeTextDisplay.text = currentTime.ToString();
		if (Input.GetKeyDown(KeyCode.Mouse0))
			RayForInterraction();

		if(gameState== GameState.gameIntro)
        {
			if (Input.anyKey)
			{
				StartPreparation();
			}
		}
		if (Input.GetKeyDown(KeyCode.K))
		{
			NexusLost();
		}

	}

	void RayForInterraction ()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6))
		{
			hit.collider.GetComponent<Interactible>().Interract();
		}
	}
}


