using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelection : MonoBehaviour {

    // Ship to set active
    public GameObject ship;

	public void SelectShip()
    {
        GameManager.instance.ChangeShip(ship);
    }
}
