using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    

    public float hp;
    public float movementspeed;
    public EnemyType enemyType;
    public State state;

    [Header("Modifiers")]
    public float hpAdded;
    public float speedAdditioner;


    //Enum declarations
    public enum EnemyType {None, Hippie, KKK, Biker, Nudist, Army};
    public enum State { Walking, Dead};

    void OnMouseDown()
    {
        if(LawManager.Instance.allowSelfDefense)
        {
            CameraHandler.Instance.remainingShakeDuration = 4;
            TakeDamage(10);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.currentEnemies.Add(this);
        movementspeed += speedAdditioner;
        hp += hpAdded;
        if(movementspeed < 1)
        {
            movementspeed = 1;
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage * GameManager.Instance.damageMultiplier;
        if(hp <= 0)
        {
            Die();
        }
    }

    float slowRemainingDuration;
    float slowPercent;
    float freezeRemainingDuration;
    float poisonRemainingDuration;
    float poisonDamage;

    public void ApplyEffect(AdditionalEffect effect)
    {
        switch (effect)
        {
            case AdditionalEffect.Slow:
                slowPercent = GameManager.Instance.slowPercent;
                slowRemainingDuration = GameManager.Instance.slowDuration;

                break;
            case AdditionalEffect.Poison:
                poisonDamage = GameManager.Instance.poisonDamage;
                poisonRemainingDuration = GameManager.Instance.poisonDuration;
                break;
            case AdditionalEffect.Stun:
                freezeRemainingDuration = GameManager.Instance.freezeDuration;

                break;
            default:
                break;
        }
    }


    

    public void Die()
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
        poisonRemainingDuration -= 0.02f;
        freezeRemainingDuration -= 0.02f;
        slowRemainingDuration -= 0.02f;

        if (poisonRemainingDuration >= 0)
            TakeDamage(poisonDamage);

        if (freezeRemainingDuration <= 0)
            transform.position += new Vector3(0, 0, ( 1-slowPercent) * GameManager.Instance.mobSpeedMultiplier * movementspeed/50);
        
    }

    // Update is called once per frame

}

public enum AdditionalEffect {None, Slow, Poison, Stun }

