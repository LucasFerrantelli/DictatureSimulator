using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public bool spawnMonster;

    public List<bool> monstersToSpawn;
    public List<float> currentdelayBetweenSpawn;

   

    // Start is called before the first frame update
    public void Init()
    {
        currentdelayBetweenSpawn.Capacity = GameManager.Instance.families.Capacity;
        for (int i = 0; i < GameManager.Instance.families.Count; i++)
        {
            currentdelayBetweenSpawn[i] = GameManager.Instance.families[i].basicDelay;
        }

    }

    void FixedUpdate()
    {
        for (int i = 0; i < currentdelayBetweenSpawn.Count; i++)
        {
            if(monstersToSpawn[i])
            {
                currentdelayBetweenSpawn[i] = currentdelayBetweenSpawn[i]-0.02f;
                if(currentdelayBetweenSpawn[i] <= 0)
                {
                    for (int j = 0; j < currentdelayBetweenSpawn.Count; j++)
                    {
                        currentdelayBetweenSpawn[j] += 0.2f;
                    }
                    currentdelayBetweenSpawn[i] = (GameManager.Instance.families[i].basicDelay + GameManager.Instance.families[i].basicDelay * Random.Range(-0.5f,0.5f))
                        * (1-GameManager.Instance.familiesScores[i]) ;
                    Instantiate(GameManager.Instance.families[i].prefab, transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
