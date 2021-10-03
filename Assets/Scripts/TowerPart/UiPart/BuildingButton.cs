using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
	//ui
	public Image myIcon;
	public TextMeshProUGUI myCostDisplayed;
	Button button;
	public Mb_Tower towerPrefab;
	public Popup myInfos;

	private void Start ()
	{
		button = GetComponent<Button>();
		Init();
	}

	void Init ()
	{
		
		myIcon.sprite = towerPrefab.baseDatas.towerIcon;
		myCostDisplayed.text = towerPrefab.liveDatas.price.ToString();
		myInfos.description.text = towerPrefab.baseDatas.towerDescription;
		myInfos.name.text = towerPrefab.baseDatas.towerName;
		//CheckMoneyAvailability(0);
	}

	void CheckMoneyAvailability ( float _useless )
	{
		if (GameManager.Instance.currentMoney >= towerPrefab.liveDatas.price)
		{
			myCostDisplayed.color = Color.black;
			button.interactable = true;
		}
		else
		{
			myCostDisplayed.color = Color.red;
			button.interactable = false;
			if (GameManager.Instance.currentSelectionedTowerType == towerPrefab)
				GameManager.Instance.currentSelectionedTowerType = null;
		}
	}

	public void SelectTower ()
	{
		GameManager.Instance.currentSelectionedTowerType = towerPrefab;
	}

	public void SetTowerType ()
	{
		GameManager.Instance.currentSelectionedTowerType = towerPrefab;
	}
}
