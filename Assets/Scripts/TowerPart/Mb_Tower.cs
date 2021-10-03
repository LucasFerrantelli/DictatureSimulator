using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine.Events;

public class Mb_Tower : MonoBehaviour
{
	public Sc_TowerInfos baseDatas;
	[HideInInspector] public TowerData liveDatas;
	[SerializeField] GameObject projectilePrefab;
	public Mb_Spot mySpot;
	//shoot part
	List<EnemyBehavior> enemyInRange;
	float reloadTime, annonciationTime = 0;
	bool shooting;
	public UnityEvent shootCanalisation, shootRealisation;
	public Animator anim;
	public Transform pivot;

	private void OnEnable ()
	{
		Init();
	}

	void Init ()
	{
		liveDatas = new TowerData();
		/*liveDatas.damage = baseDatas.towerBaseDatas.damage;
		liveDatas.range = baseDatas.towerBaseDatas.range;
		liveDatas.price = baseDatas.towerBaseDatas.price;
		liveDatas.annonciationTime = baseDatas.towerBaseDatas.annonciationTime;
		liveDatas.reloadTime = baseDatas.towerBaseDatas.reloadTime;
		liveDatas.numberOfTarget = baseDatas.towerBaseDatas.numberOfTarget;
		liveDatas.towerProjectileSpeed = baseDatas.towerBaseDatas.towerProjectileSpeed; */
		liveDatas = baseDatas.towerBaseDatas;

		annonciationTime = liveDatas.annonciationTime;
	}

	private void FixedUpdate ()
	{
		if (reloadTime > 0)
			reloadTime -= Time.fixedDeltaTime;
		else if (enemiesInRange(liveDatas.range).Length > 0 && !shooting)
		{
			StartShooting();
		}

		if (shooting && annonciationTime > 0)
		{
			annonciationTime -= Time.fixedDeltaTime;
		}
		else if (shooting)
		{
			Shoot();
		}
	}

	void StartShooting ()
	{
		shootCanalisation?.Invoke();
		anim.SetTrigger("Shoot");
		shooting = true;
	}

	void Shoot ()
	{
		shooting = false;

		reloadTime = liveDatas.reloadTime;
		annonciationTime = liveDatas.annonciationTime;

		if (liveDatas.towerDealingType == TowerDamageType.aoe)
		{
			foreach (EnemyBehavior _en in enemiesInRange(liveDatas.range))
			{
				_en.TakeDamage(liveDatas.damage);
				_en.ApplyEffect(liveDatas.effectToApply);
				anim.transform.LookAt(_en.transform);
			}
		}
		else
		{
			//au cas ou l enemi sort de la range alors que on veut lui tirer dessus
			EnemyBehavior[] _listOfTarget = enemiesInRange(liveDatas.range + 1);
			if (_listOfTarget.Length > 0)
			{
				pivot.LookAt(new Vector3(_listOfTarget[0].transform.position.x, transform.position.y, _listOfTarget[0].transform.position.z));
			}
			else
				return;

			for (int i = 0; i < liveDatas.numberOfTarget; i++)
			{

				if (i > _listOfTarget.Length)
					break;

				print(_listOfTarget[i].name);
				_listOfTarget[i].TakeDamage(liveDatas.damage); ;
				_listOfTarget[i].ApplyEffect(liveDatas.effectToApply);
			}
		}
	}

	EnemyBehavior[] enemiesInRange ( float range )
	{
		List<Collider> _temp = Physics.OverlapSphere(transform.position, range, 1 << 7).ToList();
		List<EnemyBehavior> _allEnemies = new List<EnemyBehavior>();
		foreach (Collider _hit in _temp)
		{
			_allEnemies.Add(_hit.GetComponent<EnemyBehavior>());
		}
		_allEnemies = _allEnemies.OrderBy(z => z.transform.position.z).ToList<EnemyBehavior>();
		return _allEnemies.ToArray();
	}

	public void SellTower ()
	{
		GameManager.Instance.moneyVaritation?.Invoke(liveDatas.price * .6f);
		mySpot.myTower = null;
		Destroy(gameObject);
	}

	private void OnDrawGizmosSelected ()
	{
		Gizmos.DrawSphere(transform.position, liveDatas.range);
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
	direct, aoe
}