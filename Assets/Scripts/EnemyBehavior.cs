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

    [Header("Modifiers")]
    public float hpMultiplier;
    public float hpAdded;
    public float speedMultiplier;
    public float speedAdditioner;

    //Enum declarations
    public enum EnemyType { Grunge, Hippie, Biker};
    public enum Effect { Poisoned, Slowed, Flame, Ice};
    public enum State { Walking, Dead};




    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.currentEnemies.Add(this);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Die();
        }
    } 
    

    void Die()
    {
        Destroy(this.gameObject);
        GameManager.Instance.currentEnemies.Remove(this);
    }

    public void Kamikaze()
    {
        GameManager.Instance.baseHP--;
        Destroy(this.gameObject);
        GameManager.Instance.currentEnemies.Remove(this);
    }


    void FixedUpdate()
    {
        transform.position += new Vector3(0, 0, GameManager.Instance.mobSpeedMultiplier * movementspeed/50);
        
    }

    // Update is called once per frame

}
