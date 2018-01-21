using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour {

	public GameObject player;

	float moveTime = 0.5f;
	float currentMoveTime;
	float perc = 1;

	public int camHeight;
	public float playerHeight;
	int playerCamOffset = 3;
	bool justCentered;

	Vector3 startPos;
	Vector3 endPos;


	public void resetGame(){
		playerHeight = 0; //height of player
		camHeight = (int)playerHeight + playerCamOffset; //height of camera

		startPos = gameObject.transform.position;
		endPos = gameObject.transform.position;
	}

	void Start () {
		resetGame ();
	}
	
	// Update is called once per frame
	void Update () {
		playerHeight = Mathf.Floor(player.transform.position.y);
		//print(perc);
		if (((camHeight - playerHeight) <= 0 || (camHeight - playerHeight) >= 2 * playerCamOffset) && gameObject.transform.position.y == endPos.y) {

			endPos = gameObject.transform.position;
			if (perc == 1) {
				moveTime = 0.5f;
				currentMoveTime = 0;

				camHeight = (int)Mathf.Round(playerHeight + playerCamOffset); 
				justCentered = true;
			}

			endPos.y = camHeight;
		}
	

		//print (height);
		startPos = gameObject.transform.position;


		if (justCentered == true) {
			currentMoveTime += Time.deltaTime;
			perc = currentMoveTime / moveTime;

			//print (startPos);
			//print (endPos);
			gameObject.transform.position = Vector3.Lerp (startPos, endPos, perc);

			if (perc >= 1) {
				perc = 1;
				gameObject.transform.position = endPos;
				justCentered = false;
			}
		}


		//print ("height");
		//print (height);
	}
}
