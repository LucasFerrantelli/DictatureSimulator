using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autokill : MonoBehaviour
{
	public float time= 2;
    void Start()
    {
		StartCoroutine("StartDeathCountDown");
    }

    IEnumerator StartDeathCountDown()
	{
		yield return new WaitForSeconds(time);
		Destroy(gameObject);
	}
}
