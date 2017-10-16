using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {


    // These are set in the inspector
    public GameObject objective;
    public float playerRespawnTime;
    public GameObject playerPrefab;

    [HeaderAttribute("Spawns")]
    public GameObject[] playerSpawns;
    public GameObject[] objectiveSpawns;

    [HeaderAttribute("UI")]
    public Text UIcenterText;
    public Text[] UIplayerScores;
    public Image sideBarObjectiveImage;

	
    // Not these
    // Hämtas från gamemanager
    [HideInInspector]public int numberOfPlayers;
    private Color[] playerColors;

    [HideInInspector]public GameObject[] playerList;
	private bool gameEnded = false;
    private GameObject objectiveInstance;
    private float countDownTime = 4f;
    private bool countDown = true; 
    private bool gameStarted = false;
    private Sprite activeObjective;



    void Awake(){

        playerList = new GameObject[numberOfPlayers];
    }


	void Start () {
        for(int i = 0; i < numberOfPlayers; i++){
            UIplayerScores[i].text = GameManager.instance.scores[i].ToString();
        }
        
        numberOfPlayers = GameManager.instance.numberOfPlayers;
        playerColors = GameManager.instance.playerColors;
		
        spawnObjective();
        spawnPlayers();
        UIcenterText.GetComponentInChildren<Image>().sprite = activeObjective;
        sideBarObjectiveImage.sprite = activeObjective;
        
	}
	
	// Update is called once per frame
	void Update () {
		
		// TESTA VILKEN KNAPP SOM ÄR TRYCKT
		/*foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(kcode))
				Debug.Log("KeyCode down: " + kcode);
		}*/


        // Countdown timer and disable players when round starts
        if(countDown){ 
            countDownTime -= Time.deltaTime;

            if(countDownTime > 3){
                UIcenterText.text = "Ready!";
            }
            if(1 < countDownTime && countDownTime <= 3){
                UIcenterText.text = countDownTime.ToString("f0");
            }

            if(-1 < countDownTime && countDownTime <= 1 ){
                UIcenterText.text = "Get the ";
                UIcenterText.GetComponentInChildren<Image>().enabled = true;
                enablePlayers();
            }

            if(countDownTime <= -1){
                UIcenterText.text = "";
                UIcenterText.GetComponentInChildren<Image>().enabled = false;
                countDown = false;
            }
        }


        // If game is ended, press space to restart (not needed as of now) 
		if(gameEnded){
			if(Input.GetKeyDown(KeyCode.Space)){
				newRound();
			}
		}

        if(Input.GetKeyDown(KeyCode.Backspace)){
            newRound();
        }

        if(Input.GetKeyDown(KeyCode.N)){
            Destroy(GameManager.instance.gameObject);
            Destroy(SoundManager.instance.gameObject);
            SceneManager.LoadScene(0);

        }

        if(Input.GetKeyDown(KeyCode.Q)){
            SceneManager.LoadScene(1);
        }  

        if(Input.GetKeyDown(KeyCode.W)){
            SceneManager.LoadScene(2);
        }  
	}


    // Makes players not movable
    private void disablePlayers(){
        for(int i = 0; i<numberOfPlayers; i++){
            playerList[i].GetComponent<PlayerController>().movable = false;
        }       
    }


    // Makes players movable
    private void enablePlayers(){
        for(int i = 0; i<numberOfPlayers; i++){
            playerList[i].GetComponent<PlayerController>().movable = true;
        }
    }


    // Start a new round/game
    // (reload the current scene)
    public void newRound()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // Round is finished
    public void endRound(PlayerController winner)
    {
        // Get id of winning player
        int playerId = winner.GetComponent<PlayerController>().player;
		
        // Win Text
		UIcenterText.text = "Player " + playerId + " got the item!";
        
        // Update the global score
        GameManager.instance.scores[playerId-1] += 1;
		gameEnded = true;
        
        // Force UI update
        for(int i = 0; i < numberOfPlayers; i++){
            UIplayerScores[i].text = GameManager.instance.scores[i].ToString();
        }

        // Start a new round after 3 seconds
        Invoke("newRound", 3f);
    }



    // Spawn a random objective
    private void spawnObjective()
    {
        int randomObjective = Random.Range(0, objectiveSpawns.Length);
        Vector3 pos = objectiveSpawns[randomObjective].transform.position;

		objectiveInstance = Instantiate (objective, pos, Quaternion.identity); // 2 parametrar till kan bestämma position och rotation, spawnpunkt kan bero på typen på objective
		ItemScript instanceClass = objectiveInstance.GetComponent<ItemScript>();

        instanceClass.setSprite(randomObjective);
        instanceClass.setGameController (this);
        activeObjective = instanceClass.spriteRenderer.sprite; 
        
    }


    // Spawn players at designated spawnlocations
    // (set in playerSpawns array)
    private void spawnPlayers()
    {
        for(int i = 0; i < numberOfPlayers; i++)
        {
            // Create new instance and save it in array
            playerList[i] = Instantiate(playerPrefab, playerSpawns[i].transform.position, playerSpawns[i].transform.rotation);

            // Get the player class
            PlayerController newPlayer = playerList[i].GetComponent<PlayerController>();

            // Set player number
            newPlayer.player = i+1;

            // Respawn time
            newPlayer.respawnTime = this.playerRespawnTime;

            // Set color, color delegated to correct variables in PlayerController
            newPlayer.playerColor = playerColors[i];

            // Set controls
            newPlayer.leftHorizontalInput = "P"+(i+1)+"_LeftThumbX";
            newPlayer.leftVerticalInput = "P"+(i+1)+"_LeftThumbY";
            newPlayer.rightHorizontalInput = "P"+(i+1)+"_RightThumbX";
            newPlayer.rightVerticalInput = "P"+(i+1)+"_RightThumbY";
            newPlayer.releaseInput = "P"+(i+1)+"_Release";
            newPlayer.attackInput = "P"+(i+1)+"_Attack";

            // Set the gamecontroller (this)
            newPlayer.gameController = this;


        }
    }
}
