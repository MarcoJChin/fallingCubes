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
	bool justCentered;

	Vector3 startPos;
	Vector3 endPos;


	void Start () {
		camHeight = 3; //height of camera
		playerHeight = 0; //height of player

		startPos = gameObject.transform.position;
		endPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {


		playerHeight = Mathf.Floor(player.transform.position.y);
		//print(perc);
		if (((playerHeight + 3) >= camHeight + 3 || ((playerHeight + 3)) <= camHeight - 3) && gameObject.transform.position.y == endPos.y) {

			endPos = gameObject.transform.position;
			if (perc == 1) {
				moveTime = 0.5f;
				currentMoveTime = 0;

				camHeight = (int)Mathf.Round(playerHeight + 3f); 
				//print ("new cneter");

				//print (center);
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
