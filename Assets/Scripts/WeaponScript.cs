using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

    private AudioSource audioSource;

    public GameObject attachedTo;
    private PlayerController playerClass;
    private Rigidbody2D rb;

    private bool thrown = false;
    public bool playerPrefab = false; //Används för att kolla om den sitter fast i playerprefaben eller inte pretty ghetto
    //public bool pickedUp = false;

    void Awake(){
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController collidedClass = other.GetComponent<PlayerController>();


        if (thrown == true && other.gameObject.tag == "Player" && !collidedClass.invul)
        {
            //playSound(SoundManager.instance.getHit());
            collidedClass.kill((this.transform.rotation));
        }
        else if (other.gameObject.tag == "Player" && playerPrefab && !collidedClass.invul )
        {
            playSound(SoundManager.instance.getHit());

            collidedClass.kill((this.transform.parent.gameObject.transform.rotation));
        }

        else if (other.gameObject.tag == "Player" && other.gameObject.GetComponentInChildren<WeaponManager>().carriedWeapon == null && attachedTo == null && collidedClass.carriedItem == null && !playerPrefab)
        {
            PickupWeapon(other);
        }
    }

    private void playSound(AudioClip clip){
        audioSource.clip = clip;
        audioSource.Play();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update () {
		if (attachedTo != null)
        {
            this.transform.position = attachedTo.transform.position + (attachedTo.transform.right *0.1f);
        } else if (thrown && rb.velocity.magnitude <= 1.8) //Är dödlig om farten är över 0.5 kan behöva ändras
        {
            thrown = false;
            gameObject.GetComponents<BoxCollider2D>()[1].enabled = false;
        } else if (!thrown && attachedTo == null) 
        {
            grow();
        }
	}

    public void throwWeapon()
    {
        attachedTo.GetComponentInChildren<WeaponManager>().setCarriedWeapon(null);
        GetComponent<SpriteRenderer>().enabled = true;
        rb.AddForce((attachedTo.transform.right * 500 + new Vector3(playerClass.rb.velocity.x, playerClass.rb.velocity.y, 0) * 100));
        attachedTo = null;
        thrown = true;
        gameObject.GetComponents<BoxCollider2D>()[1].enabled = true;
    }

    public void drop()
    {
        attachedTo.GetComponentInChildren<WeaponManager>().setCarriedWeapon(null);
        attachedTo = null;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void PickupWeapon(Collider2D other)
    {
        attachedTo = other.gameObject;
        playerClass = attachedTo.GetComponent<PlayerController>();
        playerClass.weaponSprite.sprite = GetComponent<SpriteRenderer>().sprite;
        other.gameObject.GetComponentInChildren<WeaponManager>().setCarriedWeapon(this.gameObject);
        GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponents<BoxCollider2D>()[1].enabled = false;
        transform.localScale = new Vector3(1.2f,1.2f,1);
    }

    public void grow()
    {
        transform.localScale=new Vector3(Mathf.PingPong(Time.time+0.3f, 0.3f)+1f, Mathf.PingPong(Time.time + 0.3f, 0.3f) + 1f, 1);
    }

}
