using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class Mb_Tower : MonoBehaviour, PopUp
{

	TowerData liveDatas;
	[SerializeField] GameObject projectilePrefab;
	float reloadTime;
	List<EnemyBehavior> enemyInRange;
	// Update is called once per frame
	void Update()
    {
        
    }

	private void FixedUpdate ()
	{
		
	}

	public void DisplayPopUp ()
	{

	}

	public void HidePopUp ()
	{

	}

	RaycastHit[] enemiesInRange()
	{
		List<RaycastHit> _temp = Physics.SphereCastAll(transform.position, liveDatas.range, Vector3.zero, 7).ToList();
		List<EnemyBehavior> _allEnemies = new List<EnemyBehavior>();

		foreach(RaycastHit _hit in _temp)
		{
			_allEnemies.Add(_hit.collider.GetComponent<EnemyBehavior>());
		}
		
		return 
	}
}

public interface PopUp
{
	void DisplayPopUp ();
	void HidePopUp ();
}

[System.Serializable]
public struct TowerData
{
	public float price, range, damage, numberOfTarget;
}

public enum TowerType
{
	direct, aoe
}