using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour {

	public GameObject baseCube;
	public GameObject deathCube;
	public int xBound;
	public int zBound;
	public int spawnHeight;

	int camHeightOffset = 10;

	public float startWaitTime = 3f;
	public float spawnWaitTime = 0.25f;


	public GameObject camCenterHeight;
	followPlayer camFollow;
	// Use this for initialization
	void Start () {
		camFollow = camCenterHeight.GetComponent<followPlayer>();

		StartCoroutine (spawnBlocks());
	}


	IEnumerator spawnBlocks(){

		yield return new WaitForSeconds (startWaitTime);

		for (int i = 0; i < 100; i++) {
			spawnHeight = (int)Mathf.Round(camFollow.playerHeight) + (int)Mathf.Round(camHeightOffset);
			Vector3 spawnPosition = new Vector3 (Random.Range (-xBound, xBound+1), spawnHeight, Random.Range (-zBound, zBound+1));//second parameter exclusive
			print (spawnPosition);
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
		
	}
}
