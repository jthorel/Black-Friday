using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Onactivation : MonoBehaviour {

    public Text numberOfPlayersText;

    void Start(){
        numberOfPlayersText.text = GameManager.instance.numberOfPlayers.ToString();
        Cursor.visible = true;
    }
    public void addPlayer(){
        GameManager.instance.addPlayer();
        numberOfPlayersText.text = GameManager.instance.numberOfPlayers.ToString();
    }

    public void removePlayer(){
        GameManager.instance.removePlayer();
        numberOfPlayersText.text = GameManager.instance.numberOfPlayers.ToString();
    }

	public void activation(int scene)
    {
		Cursor.visible = false;
        SceneManager.LoadScene(scene);
    }

    public void Quit()
    {

            Application.Quit();

    }
}
