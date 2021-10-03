using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public List<Material> variance;
    Material mat;

    public float hp;
    public float movementspeed;
    public EnemyType enemyType;
    public State state;

    [Header("Modifiers")]
    public float hpAdded;
    public float speedAdditioner;

    public enum AdditionalEffect { Slow, Poison, Stun }

    //Enum declarations
    public enum EnemyType {None, Hippie, KKK, Biker, Nudist, Army};
    public enum State { Walking, Dead};

    void OnMouseDown()
    {
        if(LawManager.Instance.allowSelfDefense)
        {
            //CameraHandler.Instance.remainingShakeDuration = 4;
            //TakeDamage(10);
            if(Random.Range(0,10) < 2)
            {
                ApplyEffect(AdditionalEffect.Poison);
            }
            else if (Random.Range(0, 10) < 6)
            {
                ApplyEffect(AdditionalEffect.Stun);
            }
            else if (Random.Range(0, 10) < 8)
            {
                TakeDamage(0.01f);
            }
            else
            {
                TakeDamage(200);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponentInChildren<MeshRenderer>().material = variance[Random.Range(0, 3)];
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
        
        if (hp <= 0)
        {
            Die();
        }
        else
        {
            GetComponentInChildren<Animator>().Play("Hurt");
        }
    }

    float slowRemainingDuration;
    float slowPercent;
    float freezeRemainingDuration;
    float poisonRemainingDuration;
    float poisonDamage;

    public void ApplyEffect(AdditionalEffect effect)
    {
        if (isAlive)
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
                    GetComponentInChildren<Animator>().Play("Poisoned");

                    break;
                case AdditionalEffect.Stun:
                    freezeRemainingDuration = GameManager.Instance.freezeDuration;
                    GetComponentInChildren<Animator>().Play("Frozen");
                    break;
                default:
                    break;
            }
        }
    }


    
    
    public void Die()
    {
        isAlive = false;
        GetComponentInChildren<Animator>().Play("Die");

    }

    public void RemoveHim()
    {
        Destroy(this.gameObject);
        GameManager.Instance.currentEnemies.Remove(this);
    }

    public void Kamikaze()
    {
        if (isAlive)
        {
            GameManager.Instance.baseHP--;
            Destroy(this.gameObject);
            GameManager.Instance.currentEnemies.Remove(this);
        }
    }

    bool isAlive = true;

    void FixedUpdate()
    {
        if(isAlive)
        {
            poisonRemainingDuration -= 0.02f;
            freezeRemainingDuration -= 0.02f;
            slowRemainingDuration -= 0.02f;

            if(slowRemainingDuration <0)
            {
                slowPercent = 0;
            }

            if (poisonRemainingDuration >= 0)
            {
                GetComponentInChildren<Animator>().SetBool("poisoned", true);
                if(poisonRemainingDuration % 10 == 0)
                {
                    TakeDamage(poisonDamage);
                }
            }
            else
            {
                GetComponentInChildren<Animator>().SetBool("poisoned", false);
            }

            if (freezeRemainingDuration <= 0)
            {
                
                transform.position += new Vector3(0, 0, (1 - slowPercent) * GameManager.Instance.mobSpeedMultiplier * movementspeed / 50);
            }
        }
        
        
    }

    // Update is called once per frame

}
