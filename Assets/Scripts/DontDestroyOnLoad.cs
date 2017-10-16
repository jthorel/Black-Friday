using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        Cursor.visible = false;
        Object.DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

// Fuck