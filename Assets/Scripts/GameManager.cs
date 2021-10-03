using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

    [Header ("Balancing")]
    public int baseHP;
    public float defaultTime = 100;
    public float difficulty;


    public List<bool> turretsUnlocked;
    public List<GameObject> turrets;

    [Header ("Settings/debug")]
    public GameState gameState;
    public float currentTime;
    public float gameSpeed;
    public float mobSpeedMultiplier = 1;
    public List<EnemyBehavior> currentEnemies;

	[Header("Families")]
	public List<float> familiesScores;
	public List<EnemyFamily> families;

	[Header("References")]
	public Text hpBaseTextDisplay;
    public Text moneyTextDisplay;
    public Text timeTextDisplay;
	public List<MonsterSpawner> spawners;
    Camera mainCam;
    public Animator gameUIAnimator;
    

	[Header("Economy")]
	public float currentMoney = 1000;
	public Action<float> moneyVaritation;
	public Mb_Tower currentSelectionedTowerType;


    public enum GameState { InFight, Preparation, LawVoting, dayEnd}

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
        StartLawVotting();
		mainCam = Camera.main;
	}

    public void StartLawVotting()
    {
        gameUIAnimator.Play("LawVotting");
    }

    public void StartPreparation()
    {
        gameUIAnimator.Play("Preparation");
        currentTime = defaultTime;
        //moneyVaritation += AddMoney;
        DeclareWaves();
    }

    public void StartFight()
    {
        gameUIAnimator.Play("Combat");
        gameState = GameState.InFight;
    }

    public void DayEnds()
    {
        gameState = GameState.dayEnd;
        StartLawVotting();
    }

	void Init ()
	{
		
	}

    void FixedUpdate()
    {
        if(gameState == GameState.InFight)
        {
            currentTime -= gameSpeed / 50;
            if (currentTime < 0 && currentEnemies.Count == 0)
            {
                DayEnds();
            }
        }
        

    }

	public void DeclareWaves ()
	{
        List<int> _indexs = new List<int>();

        for (int j = 0; j < familiesScores.Count; j++)
        {
            if (familiesScores[j] > 0)
            {
                _indexs.Add(j);
            }
        }

        for (int i = 0; i < spawners.Count; i++)
        {
            int _index = UnityEngine.Random.Range(0, familiesScores.Capacity); //i need to not generate any families but chose between available.
            //int _index = _indexs[UnityEngine.Random.Range(0, _indexs.Capacity)];
            spawners[i].mob = families[_index].prefab;
            spawners[i].delayBetweenSpawn = families[_index].basicDelay;
            spawners[i].familyScore = familiesScores[_index];
            spawners[i].Init();

        }

	}


    



    // Update is called once per frame
    void Update()
    {
        moneyTextDisplay.text = currentMoney.ToString();
        hpBaseTextDisplay.text = baseHP.ToString();
        timeTextDisplay.text = currentTime.ToString();
    }
}


