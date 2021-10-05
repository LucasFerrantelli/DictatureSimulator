using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_Spot : MonoBehaviour, Interactible
{
	[HideInInspector] public Mb_Tower myTower = null;

	public GameObject rangeDisplayer;

	public void Interract ()
	{
		if (GameManager.Instance.currentSelectionedTowerType != null && myTower == null)
			if (GameManager.Instance.currentMoney - GameManager.Instance.currentSelectionedTowerType.liveDatas.price >= 0)
			{
				print("I interract");

				//creer la tour
				Mb_Tower tower = Instantiate(GameManager.Instance.currentSelectionedTowerType.gameObject, transform.position + new Vector3(0,1,0), Quaternion.identity, transform).GetComponent<Mb_Tower>();
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

	void OnMouseOver()
    {
		if(GameManager.Instance.currentSelectionedTowerType)
        {
			rangeDisplayer.SetActive(true);

			rangeDisplayer.transform.localScale = new Vector3(1, 1, 1) * GameManager.Instance.currentSelectionedTowerType.liveDatas.range * 2;

		}
    }

	void OnMouseExit()
    {
		rangeDisplayer.SetActive(false);
	}

    private void Update()
    {
		if(GameManager.Instance.currentSelectionedTowerType == null)
		rangeDisplayer.SetActive(false);
	}
}


public interface Interactible
{
	void Interract ();
}