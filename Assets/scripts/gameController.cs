using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour {


	//public GameObject player;
	//public GameObject camera;

	public GameObject baseCube;
	public GameObject deathCube;

	public bool gameStarted;

	public int xBound1;
	public int zBound1;
	public int xBound2;
	public int zBound2;
	public int spawnHeight;

	public bool routineRunning = false;

	int camHeightOffset = 10;

	public float startWaitTime = 3f;
	public float spawnWaitTime = 0.5f;

	//get camera object
	public GameObject camCenterHeight;
	followPlayer camFollow;

	public void resetGame(){
		//reset position and delete cubes
		GameObject[] setCubes = GameObject.FindGameObjectsWithTag("set_Cube");
		foreach(GameObject cube in setCubes)
			GameObject.Destroy(cube);
		GameObject[] fallingCubes = GameObject.FindGameObjectsWithTag("falling_Cube");
		foreach(GameObject cube in fallingCubes)
			GameObject.Destroy(cube);

		gameStarted = false;
		routineRunning = false;

		//startGame ();
	}

	public void startGame(){
		resetGame ();

		routineRunning = false;
		gameStarted = true;
	}

	// Use this for initialization
	void Start () {
		camFollow = camCenterHeight.GetComponent<followPlayer>();
		resetGame ();
	}


	IEnumerator spawnBlocks(){
		//gameStarted = false;
		yield return new WaitForSeconds (startWaitTime);
		//nonePlaying = false;
		while(gameStarted){
			spawnHeight = (int)Mathf.Round(camFollow.playerHeight) + (int)Mathf.Round(camHeightOffset);
			Vector3 spawnPosition = new Vector3 (Random.Range (xBound1, xBound2+1), spawnHeight, Random.Range (zBound1, zBound2+1));//second parameter exclusive
			//print (spawnPosition);
			Quaternion spawnRotation = Quaternion.identity;

			int randomCubeSpawner = Random.Range (1, 101);

			if (randomCubeSpawner <= 90) {
				Instantiate (baseCube, spawnPosition, spawnRotation);
			} else if (randomCubeSpawner > 90) {
				Instantiate (deathCube, spawnPosition, spawnRotation);
			}

			yield return new WaitForSeconds (spawnWaitTime);
		}

	}

	// Update is called once per frame
	void Update () {
		if(!routineRunning && gameStarted){
			routineRunning = true;
			StartCoroutine (spawnBlocks());
		}
	}
}