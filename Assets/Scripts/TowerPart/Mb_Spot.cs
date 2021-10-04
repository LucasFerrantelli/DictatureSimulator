using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Spot : MonoBehaviour, Interactible
{
	[HideInInspector] public Mb_Tower myTower = null;

	public void Interract ()
	{
		if (GameManager.Instance.currentSelectionedTowerType != null && myTower == null)
			if (GameManager.Instance.currentMoney - GameManager.Instance.currentSelectionedTowerType.liveDatas.price >= 0)
			{
				print("I interract");

				//creer la tour
				Mb_Tower tower = Instantiate(GameManager.Instance.currentSelectionedTowerType.gameObject, transform.position, Quaternion.identity, transform).GetComponent<Mb_Tower>();
				tower.Init(GameManager.Instance.currentSelectionedTowerType);
				//virer le prix
				GameManager.Instance.moneyVaritation?.Invoke(-GameManager.Instance.currentSelectionedTowerType.liveDatas.price);
				//assigner le spot a la tour
				tower.mySpot = this;
				myTower = tower;

				GameManager.Instance.currentSelectionedTowerType = null;
			}
			else
			{
				GameManager.Instance.NotEnoughMoneyFeedback();
				GameManager.Instance.currentSelectionedTowerType = null;
			}
	}
}

public interface Interactible
{
	void Interract ();
}