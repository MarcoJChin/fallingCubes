using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class detectDeath : MonoBehaviour {

	public GameObject gameCon;
	public GameObject cam;

	public GameObject restart;



	public Text deathText;
	public Text scoreText; 

	public void restartGame(){

		//UI
		restart.SetActive(false);

		deathText.enabled = false;
		scoreText.enabled = false;


	}


	void checkDeathBlock(Vector3 location){
		//print ("checking");
		location = location - Vector3.up;
		Collider[] hitColliders = Physics.OverlapSphere(location, 0.1f);
		int i = 0;
		while (i < hitColliders.Length)
		{
			//print (hitColliders [i].name);
			if (hitColliders [i].name == "death_Block(Clone)" && gameObject.transform.position== gameObject.GetComponent<playerController>().endPos) {
				//print ("hitColliders [i].nam");
				death ();	
			}
			i++;
		}
	}

	void Start(){
		restartGame ();
		//restart.SetActive(true);
	}

	IEnumerator waitReset(){
		yield return new WaitForSeconds(3);
		restart.SetActive(true);
		gameObject.GetComponent<playerController>().resetGame();
		gameCon.GetComponent<gameController> ().resetGame();
		//restart.SetActive(false);
		cam.GetComponent<turnCamera> ().resetGame();

	}

	void death(){
		//gameCon.GetComponent<

		deathText.text = "You died";
		scoreText.text = "" + gameObject.GetComponent<playerController> ().maxHeight;

		gameObject.GetComponent<BoxCollider> ().enabled = false;

		gameObject.GetComponent<playerController>().gameStarted = false;
		gameCon.GetComponent<gameController> ().gameStarted = false;
		//restart.SetActive(false);
		cam.GetComponent<turnCamera> ().gameStarted = false;

		StartCoroutine (waitReset());
		//restart.SetActive(true);




		//cam.GetComponent<turnCamera> ().resetGame ();




		deathText.enabled = true;
		scoreText.enabled = true;


	}


	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "falling_Cube") {

			death ();
		}
	}

	void Update(){
		checkDeathBlock(gameObject.transform.position);

	}
}
