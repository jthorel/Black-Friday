using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour {

    private GameObject attachedTo;
    private Collider2D myCollider;
    private PlayerController playerClass;
    private Rigidbody2D rb;
    private GameController gameController;
    public Sprite[] objectiveSprites;
    public SpriteRenderer spriteRenderer;

	//Möjliga vapenstats (melee)
	private float swingReach;
	private float swingSpeed;


    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void setSprite(int number){
        spriteRenderer.sprite = objectiveSprites[number];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
		//if (myCollider.tag == "Objective") {
		if(other.tag == "Finish" && !(attachedTo == null))
		{
			gameController.endRound(playerClass);
            other.GetComponent<AudioSource>().Play();
			Destroy(this.gameObject);
		}
		//}

        if(other.tag == "Player" && (attachedTo == null))
        {
			attachToPlayer (other);
            
        }

        
        //myCollider.isTrigger = false;

    }


	// Use this for initialization
	void Start () {
        // HÅRDKODAD PLACEHOLDER
        //gameController = this.GetComponentInParent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {

        // Don't move if not attached
        if (!(this.attachedTo == null))
        {
            Vector3 attachedTo_pos = attachedTo.transform.position;
            transform.position = new Vector3(attachedTo_pos.x, attachedTo_pos.y, 0) + (attachedTo.transform.right* 0.2f);
        }
        

	}

    public void release()
    {
        // Reset carriedItem on the carrying player
        playerClass.carriedItem = null;

        // "Jump" away from player, also adding force from player movement
        rb.AddForce((attachedTo.transform.right * 500 + new Vector3(playerClass.rb.velocity.x, playerClass.rb.velocity.y, 0)*70));

        // Reset attachedTo on the item
        this.attachedTo = null;

		// Enable non-trigger collider
		gameObject.GetComponents<BoxCollider2D> () [1].enabled = true;
        

    }

	public void attack()
	{
		Debug.Log ("Attack");
	}

    // Set reference to gamecontroller (called by gamecontroller)
    public void setGameController(GameController controller)
    {
        this.gameController = controller;
    }

	private void attachToPlayer(Collider2D player){
		attachedTo = player.gameObject;
        GameObject weapon = attachedTo.GetComponentInChildren<WeaponManager>().carriedWeapon;
        if (weapon != null)
        {
            weapon.GetComponentInChildren<WeaponScript>().drop();
        }
        playerClass = attachedTo.GetComponent<PlayerController>();
		playerClass.carriedItem = gameObject.GetComponent<ItemScript>();

		// Deactivate non-trigger collider (better performance?)
		gameObject.GetComponents<BoxCollider2D> () [1].enabled = false;
	}
}
