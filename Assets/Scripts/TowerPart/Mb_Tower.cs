using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine.Events;
using DG.Tweening;

public class Mb_Tower : MonoBehaviour
{
    [InlineEditor] public Sc_TowerInfos baseDatas;
    public TowerData liveDatas;
    [SerializeField] GameObject projectilePrefab;
    public Mb_Spot mySpot;
    //shoot part
    List<EnemyBehavior> enemyInRange;
    float reloadTime, annonciationTime = 0;
    bool shooting;
    public UnityEvent shootCanalisation, shootRealisation;
    public Animator anim;
    public Transform pivot;
    public GameObject damageFx;
    public GameObject canon;

    public GameObject setActiveWhenAttacking;
    public GameObject buttonSell;
    public bool allowAttackCancel = true;
    public bool isSolar;

    //public ParticleSystem additionalParticles;
    private void OnEnable()
    {
        Init(this);
    }

    public void Init(Mb_Tower _towerTolink)
    {
        if (_towerTolink == this)
        {
            liveDatas = new TowerData();
            liveDatas.damage = baseDatas.towerBaseDatas.damage;
            liveDatas.range = baseDatas.towerBaseDatas.range;
            liveDatas.price = baseDatas.towerBaseDatas.price;
            liveDatas.annonciationTime = baseDatas.towerBaseDatas.annonciationTime;
            liveDatas.reloadTime = baseDatas.towerBaseDatas.reloadTime;
            liveDatas.numberOfTarget = baseDatas.towerBaseDatas.numberOfTarget;
            liveDatas.towerProjectileSpeed = baseDatas.towerBaseDatas.towerProjectileSpeed;
            liveDatas.towerDealingType = baseDatas.towerBaseDatas.towerDealingType;
        }
        else
            liveDatas = _towerTolink.liveDatas;

        annonciationTime = liveDatas.annonciationTime;
    }

    int frameWithoutAttacking;

    private void FixedUpdate()
    {
        if (reloadTime > 0)
        {
            reloadTime -= Time.fixedDeltaTime;

        }
        else if (enemiesInRange(liveDatas.range).Length > 0 && !shooting)
        {
            if(isSolar && GameManager.Instance.solarMultiplier <0.8f)
            {

            }
            else
            {
                StartShooting();

            }

        }
        else if (enemiesInRange(liveDatas.range).Length == 0)
        {
            frameWithoutAttacking--;
            if (frameWithoutAttacking < 20)
            {

            }

        }

        if (shooting && annonciationTime > 0)
        {
            annonciationTime -= Time.fixedDeltaTime;
        }
        else if (shooting)
        {
            Shoot();
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (setActiveWhenAttacking != null)
                setActiveWhenAttacking.SetActive(false);
        }
        else
        {
            if (setActiveWhenAttacking != null)
                if (!setActiveWhenAttacking.activeInHierarchy)
                    setActiveWhenAttacking.SetActive(true);

        }

    }

    void StartShooting()
    {
        shootCanalisation?.Invoke();


        //anim.SetTrigger("Shoot");
        shooting = true;
    }

    void Shoot()
    {
        shooting = false;
        if (allowAttackCancel)
        {
            anim.Play("Attack", -1, 0f);
        }
        else
        {
            anim.Play("Attack");
        }
        reloadTime = liveDatas.reloadTime;
        annonciationTime = liveDatas.annonciationTime;

        if (liveDatas.towerDealingType == TowerDamageType.aoe)
        {
            foreach (EnemyBehavior _en in enemiesInRange(liveDatas.range))
            {
                _en.TakeDamage(liveDatas.damage);
                _en.ApplyEffect(liveDatas.effectToApply);
                anim.transform.LookAt(new Vector3(_en.transform.position.x, transform.position.y, _en.transform.position.z));
            }
        }
        else if (liveDatas.towerDealingType == TowerDamageType.direct)
        {
            List<EnemyBehavior> _listOfTarget = enemiesInRange(liveDatas.range + 1).ToList();

            if (liveDatas.effectToApply != AdditionalEffect.None)
            {
                List<EnemyBehavior> _tempList = new List<EnemyBehavior>();

                if (liveDatas.effectToApply == AdditionalEffect.Poison)
                {
                    foreach (EnemyBehavior _en in _listOfTarget)
                    {
                        if (_en.currentState.HasFlag(CurrentState.IsPoisonned))
                            _tempList.Add(_en);
                    }
                }

                if (liveDatas.effectToApply == AdditionalEffect.Slow)
                {
                    foreach (EnemyBehavior _en in _listOfTarget)
                    {
                        if (_en.currentState.HasFlag(CurrentState.IsSlowed))
                            _tempList.Add(_en);
                    }
                }

                if (liveDatas.effectToApply == AdditionalEffect.Stun)
                {
                    foreach (EnemyBehavior _en in _listOfTarget)
                    {
                        if (_en.currentState.HasFlag(CurrentState.IsPoisonned))
                            _tempList.Add(_en);
                    }
                }

                if (_listOfTarget.Count > _tempList.Count)
                {
                    foreach (EnemyBehavior _en in _tempList)
                    {
                        _listOfTarget.Remove(_en);
                    }
                }
            }

            //clean des targets pas interessante

            if (_listOfTarget.Count > 0)
            {
               pivot.LookAt(new Vector3(_listOfTarget[0].transform.position.x, pivot.transform.position.y, _listOfTarget[0].transform.position.z));

                for (int i = 0; i < liveDatas.numberOfTarget; i++)
                {
                    if (i >= _listOfTarget.Count)
                        break;
                    //Impact
                    if(projectilePrefab != null)
                    {
                        GameObject _proj = Instantiate(projectilePrefab, canon.transform.position, Quaternion.identity);

                        _proj.transform.DOMove(_listOfTarget[i].transform.position, .1f);

                    }
                    //Instantiate(damageFx, _listOfTarget[i].transform.position, Quaternion.identity));
                    //damages
                    _listOfTarget[i].TakeDamage(liveDatas.damage);
                    _listOfTarget[i].ApplyEffect(liveDatas.effectToApply);

                }
            }
            else
                return;


        }
        else
        {
            EnemyBehavior[] _listOfTargetFlame = enemiesInRange(liveDatas.range, typeOfAoe.flame);
            if (_listOfTargetFlame.Length > 0)
            {
                pivot.LookAt(new Vector3(_listOfTargetFlame[0].transform.position.x, transform.position.y, _listOfTargetFlame[0].transform.position.z));

                foreach (EnemyBehavior _en in _listOfTargetFlame)
                {

                    _en.TakeDamage(liveDatas.damage);
                    _en.ApplyEffect(liveDatas.effectToApply);
                    //anim.transform.LookAt(_en.transform);
                }
            }
        }
    }

    enum typeOfAoe { classic, flame }

    EnemyBehavior[] enemiesInRange(float range, typeOfAoe _aoeType = typeOfAoe.classic)
    {
        List<Collider> _temp = new List<Collider>();

        if (_aoeType == typeOfAoe.classic)
            _temp = Physics.OverlapSphere(transform.position, range, 1 << 7).ToList();
        else
            _temp = Physics.OverlapCapsule(transform.position, transform.position + pivot.forward * liveDatas.range, 3, 1 << 7).ToList();

        List<EnemyBehavior> _allEnemies = new List<EnemyBehavior>();
        foreach (Collider _hit in _temp)
        {
            EnemyBehavior _enToTest = _hit.GetComponent<EnemyBehavior>();
            if (_enToTest.hp > 0)
                _allEnemies.Add(_enToTest);
        }
        _allEnemies = _allEnemies.OrderBy(z => z.transform.position.z).ToList<EnemyBehavior>();
        _allEnemies.Reverse();
        return _allEnemies.ToArray();
    }

    public void SellTower()
    {
        GameManager.Instance.moneyVaritation?.Invoke(liveDatas.price * .6f);
        mySpot.myTower = null;
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, liveDatas.range);
    }

    public void ShowSellButton()
    {
        buttonSell.SetActive(true);
    }

    public void HideSellButton()
    {
        buttonSell.SetActive(false);
    }
}


[System.Serializable]
public struct TowerData
{
    public float price, range, damage, numberOfTarget, reloadTime, annonciationTime, towerProjectileSpeed;
    public TowerDamageType towerDealingType;
    public AdditionalEffect effectToApply;
}

public enum TowerDamageType
{
    direct, aoe, lanceFlamme
}