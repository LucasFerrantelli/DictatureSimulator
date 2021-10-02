using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Mb_Tower : MonoBehaviour, PopUp
{

	TowerData liveDatas;
	[SerializeField] GameObject projectilePrefab;
	float reloadTime;
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