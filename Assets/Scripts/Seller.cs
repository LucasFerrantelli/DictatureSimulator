using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seller : MonoBehaviour, Interactible
{
    public Mb_Tower linkedTower;
    public void Interract()
    {
        linkedTower.SellTower();
    }
}
