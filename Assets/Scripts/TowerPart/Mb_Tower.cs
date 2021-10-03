using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine.Events;

public class Mb_Tower : MonoBehaviour
{
	public Sc_TowerInfos baseDatas;
	public TowerData liveDatas;
	[SerializeField] GameObject projectilePrefab;
	public Mb_Spot mySpot;
	//shoot part
	List<EnemyBehavior> enemyInRange;
	float reloadTime, annonciaitonTime;
	bool shooting;
	public UnityEvent shootCanalisation, shootRealisation;
	void Update ()
	{

	}

	private void OnEnable ()
	{
		Init();
	}

	void Init ()
	{
		liveDatas = new TowerData();
		liveDatas.damage = baseDatas.towerBaseDatas.damage;
		liveDatas.range = baseDatas.towerBaseDatas.range;
		liveDatas.price = baseDatas.towerBaseDatas.price;
	}

	private void FixedUpdate ()
	{
		if (reloadTime > 0)
			reloadTime -= Time.fixedDeltaTime;
		else if (enemiesInRange(liveDatas.range).Length > 0)
		{
			StartShooting();
		}
	}

	void StartShooting ()
	{
		if (shooting == false)
		{
			shootCanalisation?.Invoke();
		}
		shooting = true;

		if (annonciaitonTime > liveDatas.annonciationTime)
		{
			if (liveDatas.towerDealingType == TowerDamageType.aoe)
			{
				foreach (EnemyBehavior _en in enemiesInRange(liveDatas.range))
				{
					_en.TakeDamage(liveDatas.damage);
				}
			}
			else
			{
				//au cas ou l enemi sort de la range alors que on veut lui tirer dessus
				EnemyBehavior[] _listOfTarget = enemiesInRange(liveDatas.range+1);

				for (int i = 0; i < liveDatas.numberOfTarget; i++)
				{
					if (i > _listOfTarget.Length)
						break;
					_listOfTarget[i].TakeDamage(liveDatas.damage);
				}
			}
			reloadTime = liveDatas.reloadTime;
			shooting = false;

		}
	}

	EnemyBehavior[] enemiesInRange (float range)
	{
		List<RaycastHit> _temp = Physics.SphereCastAll(transform.position, range, Vector3.zero, 7).ToList();
		List<EnemyBehavior> _allEnemies = new List<EnemyBehavior>();

		foreach (RaycastHit _hit in _temp)
		{
			_allEnemies.Add(_hit.collider.GetComponent<EnemyBehavior>());
		}

		_allEnemies = _allEnemies.OrderBy(x => x.transform.position.x).ToList<EnemyBehavior>();
		return _allEnemies.ToArray();
	}
}


[System.Serializable]
public struct TowerData
{
	public float price, range, damage, numberOfTarget, reloadTime, annonciationTime, towerProjectileSpeed;
	public TowerDamageType towerDealingType;
}

public enum TowerDamageType
{
	direct, aoe
}