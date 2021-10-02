using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ma_Ui : MonoBehaviour
{
	public static Ma_Ui instance;

	private void Awake ()
	{
		if (instance == null || instance == this)
			instance = this;
		else
			Destroy(this);
	}
}
