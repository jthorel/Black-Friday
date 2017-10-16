using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;

    public ItemScript carriedItem = null;
    public GameObject carriedWeapon = null;

    public Rigidbody2D rb;
    public GameObject controllerObject;
    public GameObject corpse;
    public int player;
    public Light playerLight;
    public GameObject playerMarker;
    public Color playerColor; // Set when instantiated from gamecontroller's colorlist
    private AudioSource audioSource;
    public bool movable = false;
    public GameObject weaponSpriteObject;
    [HideInInspector]public SpriteRenderer weaponSprite;

    public bool invul = false;
    public float invulTime;
    private float stopInvul = 0.0F;

    private Quaternion fixedRotation;
    
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;

    // sätts av gamecontroller
    public float respawnTime;

    private Animator animator;
    public GameController gameController;


    private string axis1;
    private string axis2;

    // Input settings
	// Dessa ändras sätts när player instansieras, till rätt player nummer
	[Header("Input Settings")]
	public string leftHorizontalInput; 
	public string leftVerticalInput;
	public string rightHorizontalInput;
	public string rightVerticalInput;
    public string releaseInput;
    public string attackInput;


    void Awake() {
        fixedRotation = this.gameObject.transform.rotation;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    // Use this for initialization
    void Start() {
		rb.freezeRotation = true;
        weaponSprite = weaponSpriteObject.GetComponent<SpriteRenderer>();
        
        //gameController = controllerObject.GetComponent<GameController>();
        // set in GameController class directly instead

        //GetComponent<SpriteRenderer>().color = this.playerColor;
        playerMarker.GetComponent<SpriteRenderer>().color = this.playerColor;
        //playerLight.color = this.playerColor;




        // Läsa och hämta spelkontroller
        /*
        inputDevices = Input.GetJoystickNames();
         foreach (var device in inputDevices)
        {
            Debug.Log(device);
        }


        if (inputDevices[1] == "Wireless Controller")
        {
            axis1 = "PS4_LeftThumbX";
            axis2 = "PS4_LeftThumbY";
        }
        else if (inputDevices[1] == "Controller (XBOX 360 For Windows)")
        {
            axis1 = "XBOX_LeftThumbX";
            axis2 = "XBOX_LeftThumbY";
        }
        else
        {
            axis1 = "Horizontal";
            axis2 = "Vertical";
        }*/

        
	}


    private void playSound(AudioClip clip){
        audioSource.clip = clip;
        audioSource.Play();
    }



    // Called when player hits the attack button and is not carrying the objective
    void attack()
    {
        animator.SetTrigger("Attack");
        playSound(SoundManager.instance.getAttack());
    }




    // Called when a player gets hit/is killed
    public void kill(Quaternion rotation)
    {
        playSound(SoundManager.instance.getDeath());

        GameObject weapon = gameObject.GetComponentInChildren<WeaponManager>().carriedWeapon;
        if (weapon != null)
        {
            weapon.GetComponentInChildren<WeaponScript>().drop();
            weapon = null;
            print(gameObject.GetComponentInChildren<WeaponManager>().carriedWeapon);
            //gameObject.GetComponentInChildren<WeaponManager>().carriedWeapon = null;
        }

        if (this.carriedItem != null)
        {
            this.carriedItem.release();
        }


        // Instantiate död-sprite med animation
        // Timer med co-routine för att sedan flytta spelare till en spawn (transform.position)  
        Instantiate(corpse, this.transform.position+rotation*(new Vector3(0.5f,0,0)), rotation);
        this.transform.position = gameController.playerSpawns[player - 1].transform.position;
        this.enabled = false;
        this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        
        //TESTKOD        
        IEnumerator coroutine = respawnPlayer(this.respawnTime);
        StartCoroutine(coroutine);
        
    }

    private IEnumerator respawnPlayer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        this.gameObject.GetComponent<CircleCollider2D>().enabled = true;      
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.enabled = true;

    }



    void playerMoving(float horizontal, float vertical)
    {

        animator.Play("PlayerWalking");

        // Add force to the rigidbody
        Vector2 move = new Vector2(horizontal, vertical);
        rb.AddForce(move * speed);
    }


	void FixedUpdate () {

        // Get joystick positions
		float horizontal = Input.GetAxis(leftHorizontalInput);
		float vertical = Input.GetAxis(leftVerticalInput);

        if ((horizontal != 0 || vertical != 0) && movable)
        {
            playerMoving(horizontal, vertical);
        }
        else
        {
            animator.Play("Idle");
        }
        


        // Get right joystick position
		float xAngle = Input.GetAxis (rightHorizontalInput);
		float yAngle = Input.GetAxis (rightVerticalInput);

        // Rotation on the sprite (internal rotations)
		//Genom att ändra på dead till rätt värde i inputeditorn så kan vi slippa dessa if satser och ha != 0 istället som i horizontal och vertical
		if (xAngle != 0 || yAngle != 0) {
			float angle = (Mathf.Atan2 (yAngle, xAngle) * Mathf.Rad2Deg);
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle ));
		} else if (horizontal != 0 || vertical != 0){
			float angle = (Mathf.Atan2 (vertical, horizontal) * Mathf.Rad2Deg);
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
		}


    }



    void Update()
    {
        if (Input.GetButtonDown(releaseInput) || Input.GetKey(KeyCode.Space))
        {
            //rb.velocity = new Vector3(0, 0, 0);
            if (this.carriedItem != null)
            {
                carriedItem.release();
            } else { }
            GameObject weapon = gameObject.GetComponentInChildren<WeaponManager>().carriedWeapon;
            if (weapon != null)
            {
                //weapon.GetComponentInChildren<WeaponScript>().drop();
                weapon.GetComponentInChildren<WeaponScript>().throwWeapon();
            }

        //GetButtonDown behövs här eftersom vanliga GetButton Aktiveras flera gånger medan GetButton endast en gång
        // Flyttat till Update() för att det ska fungera, samma med releaseInput /Johan
        }
        else if (this.gameObject.GetComponentInChildren<WeaponManager>().carriedWeapon != null && carriedItem == null && Input.GetButtonDown(attackInput) && Time.time > nextFire )
        {
            nextFire = Time.time + fireRate;
            attack();
        }



    }

    void LateUpdate(){
        playerMarker.transform.rotation = fixedRotation;
        playerMarker.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y+0.5f, this.gameObject.transform.position.z);

    }

}
