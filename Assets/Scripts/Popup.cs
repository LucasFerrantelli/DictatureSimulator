using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Popup : MonoBehaviour
{
	public TextMeshProUGUI name, description;

	public void SetDescription(string popupName, string popupText)
	{
		name.SetText(popupName);
		description.SetText(popupText);
	}
}
