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
    public List<EnemyBehavior> currentEnemies;

	[Header("Families")]
	public List<float> familiesScores;
	public List<EnemyFamily> families;

	[Header("References")]
	public Text hpBaseTextDisplay;
	public List<MonsterSpawner> spawners;
    Camera mainCam;

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

    void StartLawVotting()
    {

    }

    void StartPreparation()
    {
        currentTime = defaultTime;
        //moneyVaritation += AddMoney;
        DeclareWaves();
    }

    void StartFight()
    {
        gameState = GameState.InFight;
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
                gameState = GameState.dayEnd;
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
        hpBaseTextDisplay.text = baseHP.ToString();
    }
}


