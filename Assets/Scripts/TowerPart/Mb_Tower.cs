using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class Mb_Tower : MonoBehaviour
{
	public Sc_TowerInfos baseDatas;
	public TowerData liveDatas;
	[SerializeField] GameObject projectilePrefab;
	public Mb_Spot mySpot;
	//shoot part
	float reloadTime;
	List<EnemyBehavior> enemyInRange;

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
		else if (enemiesInRange().Length > 0)
		{
			Shoot();
		}
	}

	void Shoot ()
	{
		print("PEWPEW");
	}

	EnemyBehavior[] enemiesInRange ()
	{
		List<RaycastHit> _temp = Physics.SphereCastAll(transform.position, liveDatas.range, Vector3.zero, 7).ToList();
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
	public float price, range, damage, numberOfTarget;
}

public enum TowerDamageType
{
	direct, aoe
}