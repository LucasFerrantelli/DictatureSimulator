using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    

    public float hp;
    public float movementspeed;
    public EnemyType enemyType;
    public List<Effect> effects;
    public State state;

    //Enum declarations
    public enum EnemyType { Grunge, Hippie, Biker};
    public enum Effect { Poisoned, Slowed, Flame, Ice};
    public enum State { Walking, Dead};




    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }    


    void FixedUpdate()
    {
        transform.position += new Vector3(0, 0, movementspeed/50);
        
    }

    // Update is called once per frame

}
