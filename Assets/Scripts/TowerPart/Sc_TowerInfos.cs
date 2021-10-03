using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerStats", menuName = "NewTower")]
public class Sc_TowerInfos : ScriptableObject
{
	public string towerName, towerDescription;
	public Sprite towerIcon;
	public TowerData towerBaseDatas;
}

