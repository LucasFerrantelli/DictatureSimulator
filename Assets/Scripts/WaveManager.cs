using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DeclareWave(List<FamilySwarm> swarms)
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

 public  struct FamilySwarm
{
    EnemyBehavior.EnemyType enemyType;
    public int force;
    public GameObject prefabToSpawn;
}


