using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Kommer instansieras från menyn, men behöver ligga i scenen nu för testkörningar
// (dubbleras inte då den kollar om instans redan finns)
public class GameManager : MonoBehaviour {

	
	public static GameManager instance = null;	
	
	[HideInInspector]public int[] scores; 

	// Set in inspector
	public Color[] playerColors;
	public int numberOfPlayers;

	void Awake()
	{
		//Check if instance already exists
		if (instance == null)
			
			//if not, set instance to this
			instance = this;
		
		//If instance already exists and it's not this:
		else if (instance != this)
			
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    
		
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		scores = new int[numberOfPlayers];
	}


	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void addPlayer(){
		if(numberOfPlayers < 4){
			numberOfPlayers += 1;
		}
	}

	public void removePlayer(){
		if(numberOfPlayers > 2){
			numberOfPlayers -= 1;
		}
	}
}
