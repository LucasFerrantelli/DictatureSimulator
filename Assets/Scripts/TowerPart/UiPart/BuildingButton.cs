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

	private void OnEnable ()
	{
		Init();
		button = GetComponent<Button>();
		GameManager.Instance.moneyVaritation += CheckMoneyAvailability;
		GameManager.Instance.lawsAreApplied += Init;
	}

    private void OnDisable()
    {
		GameManager.Instance.moneyVaritation -= CheckMoneyAvailability;
		GameManager.Instance.lawsAreApplied -= Init;

	}

	void Init ()
	{
		print("I m setup");
		myIcon.sprite = towerPrefab.baseDatas.towerIcon;
		myCostDisplayed.text = towerPrefab.liveDatas.price.ToString();
		myInfos.description.text = towerPrefab.baseDatas.towerDescription;
		myInfos.name.text = towerPrefab.baseDatas.towerName;
		//CheckMoneyAvailability(0);
	}

    private void Update()
    {
		CheckMoneyAvailability(0);
	}

    void CheckMoneyAvailability ( float _useless )
	{
		if (GameManager.Instance.currentMoney >= towerPrefab.liveDatas.price)
		{
			myCostDisplayed.color = Color.white;
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
