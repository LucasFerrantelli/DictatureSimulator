using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterSpawner : MonoBehaviour
{
    public Telegraph associatedTelegraph;


    public bool spawnMonster;

    public GameObject mob;
    public GameObject mob2;
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
        if(GameManager.Instance.gameState == GameManager.GameState.Preparation && mob != null)
        {
            associatedTelegraph.gameObject.SetActive(true);
        }
        else
        {
            associatedTelegraph.gameObject.SetActive(false);
        }


        if(GameManager.Instance.currentTime > 0 && GameManager.Instance.gameState == GameManager.GameState.InFight && mob!= null)
        {
            currentDelayBetweenSpawn = currentDelayBetweenSpawn - 0.02f;
            if (currentDelayBetweenSpawn <= 0)
            {
                DelayGeneration();
                if(Random.Range(0f,1f) > 0.3f)
                {
                    Instantiate(mob, transform);
                }
                else
                {
                    Instantiate(mob2, transform);
                }
                
            }
        }
        
    }

    void DelayGeneration()
    {
        currentDelayBetweenSpawn = (delayBetweenSpawn + delayBetweenSpawn * Random.Range(-0.5f, 0.5f))
                * (1 - familyScore) + (1-GameManager.Instance.difficulty/5);
        if(currentDelayBetweenSpawn < 0.5)
        {
            currentDelayBetweenSpawn = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
