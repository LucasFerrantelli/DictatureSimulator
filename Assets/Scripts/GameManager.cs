using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header ("Balancing")]
    public int baseHP;
    public float defaultTime = 100;


    [Header ("Settings/debug")]
    public GameState gameState;
    public float currentTime;
    public float gameSpeed;

    [Header("Families")]
    public List<float> familiesScores;
    public List<EnemyFamily> families;

    [Header("References")]
    public Text hpBaseTextDisplay;
    public List<MonsterSpawner> spawners;


    [Serializable]
    public struct EnemyFamily
    {
        public EnemyBehavior.EnemyType enemy;
        public GameObject prefab;
        public float basicDelay;
    }

    public enum GameState { InFight, Preparation, LawVoting}







    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        Init();
    }

    void Init()
    {
        currentTime = defaultTime;
        //DeclareWaves();
    }

    void FixedUpdate()
    {
        currentTime -= gameSpeed / 50;


    }


    public void DeclareWaves()
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



    // Update is called once per frame
    void Update()
    {
        hpBaseTextDisplay.text = baseHP.ToString();
    }
}


