using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterSpawner : MonoBehaviour
{
    public bool spawnMonster;

    public GameObject mob;
    public float delayBetweenSpawn;
    public float currentDelayBetweenSpawn;
    public float familyScore;

   

    // Start is called before the first frame update
    public void Init()
    {
        DelayGeneration();


    }

    void FixedUpdate()
    {
        if(GameManager.Instance.currentTime > 0)
        {
            currentDelayBetweenSpawn = currentDelayBetweenSpawn - 0.02f;
            if (currentDelayBetweenSpawn <= 0)
            {
                DelayGeneration();
                Instantiate(mob, transform);
            }
        }
        
    }

    void DelayGeneration()
    {
        currentDelayBetweenSpawn = (delayBetweenSpawn + delayBetweenSpawn * Random.Range(-0.5f, 0.5f))
                * (1 - familyScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
