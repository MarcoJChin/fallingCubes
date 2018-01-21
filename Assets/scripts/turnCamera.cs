
/*
Contols camera angle in 1 of four possible locations
Lerps positions and rotations for smooth transitions
Keeps track of which persective (direction of arrow keys) for player controller
*/

using UnityEngine;
using System.Collections;



public class turnCamera : MonoBehaviour {

	public bool gameStarted =false;

	float moveTime = 0.5f; //time it takes to move
	float currentMoveTime; //current time in movement
	float perc = 1; //percent through movement

	Vector3 startPos;
	Vector3 startRot;

	Vector3 endPos;
	Vector3 endRot;

	float camHeight;

	bool rotatedCamera;


	int camIndex = 0;


	Vector3[] camPositions = new [] { new Vector3 (1f, 3f,1f), 
									new Vector3 (-1f, 3f,1f), 
									new Vector3 (-1f, 3f,-1f), 
									new Vector3 (1f, 3f,-1f), };

	Vector3[] perspectiveUps = new [] { new Vector3 (-1f, 0f, 0f), //up
											new Vector3 (0f, 0f, -1f),
											new Vector3 (1f, 0f, 0f), 
											new Vector3 (0f, 0f, 1f)};
	
	Vector3[] perspectiveRights = new [] { new Vector3 (0f, 0f, 1f), //right
											new Vector3 (-1f, 0f, 0f),
											new Vector3 (0f, 0f, -1f), 
											new Vector3 (1f, 0f, 0f)};
	
	public Vector3 perspectiveUp;
	public Vector3 perspectiveRight;

	public GameObject camCenterHeight;
	followPlayer camFollow;


	public void resetGame(){
		//rest cam location/rotation
		camIndex = 0; //set default cam agnle

		gameObject.transform.position = camPositions [camIndex];
		gameObject.transform.eulerAngles = new Vector3 (45f, 225f, 0f);

		updateCameraPerspectives(camIndex);

		endPos = gameObject.transform.position;
		endRot = gameObject.transform.eulerAngles;

		gameStarted = false;
	}

	public void startGame(){

		resetGame ();
		gameStarted = true;
	}

	void updateCameraPerspectives(int camIndex){
		perspectiveUp = perspectiveUps [camIndex];
		perspectiveRight = perspectiveRights [camIndex];
	}

	void Start(){

		camFollow = camCenterHeight.GetComponent<followPlayer>();

		resetGame ();
	}

	void Update() {

		if (gameStarted) {
			camHeight = Mathf.Round (camFollow.camHeight);

			if (Input.GetButtonDown ("moveCamLeft") ||
			  Input.GetButtonDown ("moveCamRight")) {
				if (perc == 1) {
					moveTime = 0.4f;
					currentMoveTime = 0;
					rotatedCamera = true;
				}
			}
			startPos = gameObject.transform.position;
			startRot = gameObject.transform.eulerAngles;

			if (Input.GetButtonDown ("moveCamLeft") && gameObject.transform.position.x == endPos.x && gameObject.transform.position.z == endPos.z) { //left...clockwise..only check level
				//print("left");

				camIndex--;
				if (camIndex <= -1) { //loop
					camIndex = 3;
				}
				endRot.y += 90; //90 degrees


			}
			if (Input.GetButtonDown ("moveCamRight") && gameObject.transform.position.x == endPos.x && gameObject.transform.position.z == endPos.z) { //right...counter clockwise
				//print("right");

				camIndex++;
				if (camIndex >= 4) {//loop
					camIndex = 0;
				}
				endRot.y -= 90; //90 degrees

			}
			endPos = camPositions [camIndex];
			endPos.y = (camHeight);
			updateCameraPerspectives (camIndex);

			if (rotatedCamera == true) {
				currentMoveTime += Time.deltaTime;
				perc = currentMoveTime / moveTime;

				gameObject.transform.position = Vector3.Lerp (startPos, endPos, perc);
				gameObject.transform.rotation = Quaternion.Lerp (gameObject.transform.rotation, Quaternion.Euler (45, endRot.y, 0), perc);

				if (perc >= 1) {
					perc = 1;

					//self correct
					gameObject.transform.position = endPos;
					gameObject.transform.eulerAngles = endRot;
					//print (endRot);
					rotatedCamera = false;
				}
			}
		}
	}
}
