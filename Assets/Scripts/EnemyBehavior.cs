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

    public GameObject ice;
    public GameObject poison;
    public GameObject hurt;
    public GameObject reward;

    [Header("Modifiers")]
    public float hpAdded;
    public float speedAdditioner;


    //Enum declarations
    public enum EnemyType {None, Hippie, KKK, Biker, Nudist, Army};
    public enum State { Walking, Dead};
	[HideInInspector] public CurrentState currentState;


    void OnMouseDown()
    {
        if(LawManager.Instance.allowSelfDefense)
        {
            //CameraHandler.Instance.remainingShakeDuration = 4;
            //ApplyEffect(AdditionalEffect.Stun);
            TakeDamage(10);
            //if(Random.Range(0,10) < 2)
            //{
            //    ApplyEffect(AdditionalEffect.Poison);
            //}
            //else if (Random.Range(0, 10) < 6)
            //{
            //    ApplyEffect(AdditionalEffect.Stun);
            //}
            //else if (Random.Range(0, 10) < 8)
            //{
            //    TakeDamage(0.01f);
            //}
            //else
            //{
            //    TakeDamage(200);
            //}
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponentInChildren<MeshRenderer>().material = variance[Random.Range(0, 3)];
        GameManager.Instance.currentEnemies.Add(this);
        movementspeed += speedAdditioner;
        hp += hpAdded;
		currentState = CurrentState.None;

		if (movementspeed < 1)
        {
            movementspeed = 1;
        }
    }

    public void TakeDamage(float damage)
    {
        Instantiate(hurt, transform);
        hp -= damage * GameManager.Instance.damageMultiplier;
        
        if (hp <= 0)
        {
            Die();
        }
        else
        {
            frameWithoutBeingHurt = 0;
            if(frameWithoutBeingHurt > 50)
            {
                transform.position += new Vector3(0, 0, -0.1f);
            }
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
					currentState |= CurrentState.IsSlowed;

                    break;
                case AdditionalEffect.Poison:
                    poisonDamage = GameManager.Instance.poisonDamage;
                    poisonRemainingDuration = GameManager.Instance.poisonDuration;
                    GetComponentInChildren<Animator>().Play("Poisoned");
					currentState |= CurrentState.IsPoisonned;

					break;
                case AdditionalEffect.Stun:
                    freezeRemainingDuration = GameManager.Instance.freezeDuration;
                    GetComponentInChildren<Animator>().Play("Frozen");
					currentState |= CurrentState.IsStunned;

					break;
                default:
                    break;
            }
        }
    }


    
    
    public void Die()
    {
        reward.SetActive(true);
        isAlive = false;
        GetComponentInChildren<Animator>().Play("Die");
        GameManager.Instance.stateStability += 0.01f;
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

    int frameWithoutBeingHurt;
    void FixedUpdate()
    {
        frameWithoutBeingHurt++;
        if(isAlive)
        {
            poisonRemainingDuration -= 0.02f;
            freezeRemainingDuration -= 0.02f;
            slowRemainingDuration -= 0.02f;

            if(slowRemainingDuration <0)
            {
                slowPercent = 0;
				currentState &= ~CurrentState.IsSlowed;
			}

			if (poisonRemainingDuration >= 0)
            {
                poison.SetActive(true);
                GetComponentInChildren<Animator>().SetBool("poisoned", true);
                if(poisonRemainingDuration % 10 == 0)
                {
                    TakeDamage(poisonDamage);
                }
            }
            else
            {
                GetComponentInChildren<Animator>().SetBool("poisoned", false);
				currentState &= ~CurrentState.IsPoisonned;
                poison.SetActive(false);
            }

			if (freezeRemainingDuration <= 0)
            {
                transform.position += new Vector3(0, 0, (1 - slowPercent) * GameManager.Instance.mobSpeedMultiplier * movementspeed / 50);
				currentState &= ~CurrentState.IsStunned;
                ice.SetActive(false);
            }
            else
            {
                ice.SetActive(true);
            }
		}
        
        
    }

    // Update is called once per frame

}

public enum AdditionalEffect {None, Slow, Poison, Stun }


[System.Flags]
public enum CurrentState { None = 1 << 0, IsSlowed = 1 << 1, IsPoisonned = 1 << 2, IsStunned = 1 << 3 }