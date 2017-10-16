using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	// Singleton
	public static SoundManager instance = null;

	// Two players, one for fx clips and one for continous music
	public AudioSource musicSource;

	// Sound databases
	// Clips should probably be in object prefabs instead for easier future implementations
	public AudioClip[] deathSounds;
	public AudioClip[] uiSounds;
	public AudioClip[] attackSounds;
	public AudioClip[] hitSounds;



	void Awake()
	{
		// Singleton pattern
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

	}	


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public AudioClip getDeath(){
		return (deathSounds[Random.Range(0,deathSounds.Length)]);
	}

	public AudioClip getHit(){
		return hitSounds[Random.Range(0,hitSounds.Length)];
	}

	public AudioClip getAttack(){
		return (attackSounds[Random.Range(0,attackSounds.Length)]);
	}


}
