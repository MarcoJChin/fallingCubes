

/*
So far...
Controls the player with arrow keys and movement direction depends on the perspective of the camera angle that I change 
with "a" and "d". 
Checks bounds to make sure palyer stays within them
Lerps from location to location using a sin function to simulate jumping


*/
using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	//Audio
	public AudioClip landedBlock;
	public float volume;
	AudioSource audio;

	//Lerp variabls
	float moveTime = 0.1f; //time it takes to move
	float currentMoveTime; //current time in movement
	float base_perc = 1; //percent through movement
	bool jumped; //check if jumped

	//start/end pos
	Vector3 startPos;
	Vector3 endPos;

	float floorWidth;
	float playerWidth;
	Vector3 playerDim;

	//help find end pos
	Vector3 perspUp;
	Vector3 perspRight;
	bool up = false;
	bool down = false;
	bool right = false;
	bool left = false;
	bool throughJump = false;

	//find camera component
	public GameObject camera; 
	turnCamera turnCamScript;

	//Helper Functions
	void startJump(){
		moveTime = 0.1f;
		currentMoveTime = 0;
		jumped = true;			
	}

	bool checkBounds(Vector3 location){
		print("abs");
		print((Mathf.Abs(location.x)));
		return ((Mathf.Abs(location.x) <= (floorWidth/2f)) && (Mathf.Abs(location.z) <= (floorWidth/2f)));
	}

	//check if block is blocking the path
	bool checkBlock(Vector3 location){
		Collider[] hitColliders = Physics.OverlapSphere(location, 0.1f);
		int i = 0;
		while (i < hitColliders.Length)
		{
			if(hitColliders[i].tag =="set_Cube")
				return true;	
				
			i++;
		}
		return false;
	}

	//Check for surface to land on
	bool checkSurface(Vector3 location){ //alligned with whole numbers
		location.y = Mathf.Floor(location.y);
		Collider[] hitColliders = Physics.OverlapSphere(location, 0.1f);
		int i = 0;
		while (i < hitColliders.Length)
		{
			if (hitColliders [i].tag == "set_Cube" || hitColliders [i].name == "Ground")
				return true;	

			i++;
		}
		return false;
	}

	//decide what action to do at nex location
	void decideJumpAction(Vector3 nextLocation){
			endPos = nextLocation;
			if (checkBlock (nextLocation)) {//there is a block in front
				endPos = nextLocation + Vector3.up;
				if (checkBlock (nextLocation + Vector3.up)) {//there is a block infront and above that
					endPos = gameObject.transform.position;
				}
			} else if (!checkBlock (nextLocation)) {//there is no block in front
				if (!checkSurface (nextLocation)) {//there is no surface to jump on infront
					if (checkSurface (nextLocation - Vector3.up)) {// there is a surface to land on one down
						endPos = nextLocation - Vector3.up;
					} else if (checkSurface (nextLocation - Vector3.up * 2f)) {// there is a surface to land on two down
						endPos = nextLocation - Vector3.up * 2f;
					} else if (!checkSurface (nextLocation - Vector3.up * 2f)) {//there is no surface 2 down to land on
						endPos = gameObject.transform.position;
					}
				} else { //there is a surface to jump on
					endPos = nextLocation;
				}
			}
	}
	/* else {//not within bounds
			throughJump = true;
			if (up) {
				endPos = gameObject.transform.position - perspUp * 2f;
			}else if (down) {
				endPos = gameObject.transform.position + perspUp * 2f;
			}else if (left) {
				endPos = gameObject.transform.position + perspRight * 2f;
			}else if (right) {
				endPos = gameObject.transform.position - perspRight * 2f;
			}
		}*/

	bool xUpDown;
	bool zUpDown;

	void inputDecide(){ //restart input 
		//print (base_perc);
		if(Input.GetButtonDown("Up") || 
			Input.GetButtonDown("Down") ||
			Input.GetButtonDown("Left") || 
			Input.GetButtonDown("Right")){
			if(base_perc == 1){ //last jump ended
				//play sound
				audio.PlayOneShot (landedBlock,volume);
				startPos = gameObject.transform.position;
				startJump ();
			}

			perspUp = turnCamScript.perspectiveUp;//update perspective on input
			perspRight = turnCamScript.perspectiveRight;//update perspective right

			if (perspUp.x == 1 || perspUp.x == -1)
				xUpDown = true;
			else if (perspUp.z == 1 || perspUp.z == -1)
				zUpDown = true;

			up = false;
			down = false;
			right = false;
			left = false;

			//input and decide
			Vector3 nextLocation = gameObject.transform.position;

			//set next location to check
			if(gameObject.transform.position == endPos){
				if (Input.GetButtonDown ("Up")) {//at correct location
					nextLocation =new Vector3 (transform.position.x, transform.position.y, transform.position.z) + perspUp;
					up = true;
				}
				if (Input.GetButtonDown ("Down")){
					nextLocation =new Vector3 (transform.position.x, transform.position.y, transform.position.z) - perspUp;
					down = true;
				}
				if (Input.GetButtonDown ("Left")){
					nextLocation =new Vector3 (transform.position.x, transform.position.y, transform.position.z) - perspRight;
					left = true;
				}
				if (Input.GetButtonDown ("Right")){
					nextLocation =new Vector3 (transform.position.x, transform.position.y, transform.position.z) + perspRight;
					right = true;
				}
				//jump through 
				if (!checkBounds (nextLocation)) {//not within bounds
					throughJump = true;
					if (up) {
						nextLocation = gameObject.transform.position - (perspUp * (floorWidth-1));
					} else if (down) {
						nextLocation = gameObject.transform.position + (perspUp * (floorWidth-1));
					} else if (left) {
						nextLocation = gameObject.transform.position + (perspRight * (floorWidth-1));
					} else if (right) {
						nextLocation = gameObject.transform.position - (perspRight * (floorWidth-1));
					}
				}
				//decide
				decideJumpAction(nextLocation);
			}

			//action

			//base_jump ();




		}





	}
	
	//Major functions
	void base_jump(){

		if (jumped == true) {

			currentMoveTime += Time.deltaTime * 0.5f;
			base_perc = currentMoveTime / moveTime;

			float hopHeight =gameObject.transform.localScale.y/2f;

			//print (base_perc);

			var height = Mathf.Sin(Mathf.PI * base_perc) * hopHeight;


			if (throughJump) {
				//bool jumpedThroughWall = false;

				//scale lerp

				if (up || down) {
					if (base_perc < 0.5f) { //shrink
						if (xUpDown)
							gameObject.transform.localScale = Vector3.Lerp (playerDim, playerWidth *  new Vector3(0,1,1), base_perc);
						else if (zUpDown)
							gameObject.transform.localScale = Vector3.Lerp (playerDim, playerWidth *  new Vector3(1,1,0), base_perc); 
					}
					else //expand
						if(xUpDown)
							gameObject.transform.localScale = Vector3.Lerp (playerWidth *  new Vector3(0,1,1), playerDim, base_perc); 
						else if (zUpDown)
							gameObject.transform.localScale = Vector3.Lerp (playerWidth *  new Vector3(1,1,0), playerDim, base_perc);
				} else if (right || left) {
					if (base_perc < 0.5f) {//shrink
						if(xUpDown)
							gameObject.transform.localScale = Vector3.Lerp (playerDim, playerWidth *  new Vector3(1,1,0) , base_perc); 
						else if (zUpDown)
							gameObject.transform.localScale = Vector3.Lerp (playerDim, playerWidth *  new Vector3(0,1,1), base_perc);
					}
					else//expand
						if(xUpDown)
							gameObject.transform.localScale = Vector3.Lerp (playerWidth *  new Vector3(1,1,0), playerDim , base_perc);
						else if (zUpDown)
							gameObject.transform.localScale= Vector3.Lerp (playerWidth *  new Vector3(0,1,1), playerDim , base_perc); 
				}

				//position lerp
				if (up) {
					if (base_perc < 0.5f) {
						gameObject.transform.position = Vector3.Lerp (startPos, startPos + perspUp, base_perc) + Vector3.up * height;
					}else{
						gameObject.transform.position = Vector3.Lerp (endPos - perspUp, endPos, base_perc) + Vector3.up * height;
					}

				} else if (down) {
					if (base_perc < 0.5f) {
						gameObject.transform.position = Vector3.Lerp (startPos, startPos - perspUp, base_perc) + Vector3.up * height;
					}else{
						gameObject.transform.position = Vector3.Lerp (endPos + perspUp, endPos, base_perc) + Vector3.up * height;
					}
				} else if (left) {
					if (base_perc < 0.5f) {
						gameObject.transform.position = Vector3.Lerp (startPos, startPos - perspRight, base_perc) + Vector3.up * height;
					}else{
						gameObject.transform.position = Vector3.Lerp (endPos + perspRight, endPos, base_perc) + Vector3.up * height;
					}
				} else if (right) {
					if (base_perc < 0.5f) {
						gameObject.transform.position = Vector3.Lerp (startPos, startPos + perspRight, base_perc) + Vector3.up * height;
					}else{
						gameObject.transform.position = Vector3.Lerp (endPos - perspRight, endPos, base_perc) + Vector3.up * height;
					}
				}

			} else {
				gameObject.transform.position = Vector3.Lerp (startPos, endPos, base_perc) + Vector3.up * height;
			}
			if (base_perc >= 1) {
				base_perc = 1;
				//self correct
				float x = Mathf.Round(endPos.x);
				float y = Mathf.Round(endPos.y) + gameObject.transform.localScale.y/2f;
				float z = Mathf.Round(endPos.z);
				gameObject.transform.position = new Vector3 (x, y, z);
				gameObject.transform.rotation = Quaternion.identity;

				xUpDown = false;
				zUpDown = false;

				jumped = false;
				throughJump = false;
			}
		}
	}

	//Main Functions
	void Start(){
		//set audio source
		audio = GetComponent<AudioSource> ();

		//ground object
		GameObject ground = GameObject.Find("Ground");
		floorWidth = ground.transform.localScale.x * 10f;
		playerDim = gameObject.transform.localScale;
		playerWidth = gameObject.transform.localScale.x;

		//camera object
		turnCamScript = camera.GetComponent<turnCamera>();

		//start player location
		gameObject.transform.position = new Vector3 (0.0F, gameObject.transform.localScale.y/2f, 0.0F); //start position
		endPos = gameObject.transform.position;
	}

	void Update() {
		inputDecide();
		base_jump();
	}
}
