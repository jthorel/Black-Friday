using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

	public GameObject carriedWeapon = null;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setCarriedWeapon(GameObject weapon)
    {
        carriedWeapon = weapon;
    }
}
