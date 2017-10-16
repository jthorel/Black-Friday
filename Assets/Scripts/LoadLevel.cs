using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {


	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("P1_Attack"))
        {
            SceneManager.LoadScene(1);
        }
		
	}
}
