using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeController : MonoBehaviour {

	public Rigidbody rb;

	Vector3 startPos;
	Vector3 endPos;

	//bool set = false;

	void setFinalLocation(){
		rb.velocity = new Vector3 (0, 0, 0);
		Vector3 setLocation = new Vector3(Mathf.Round (transform.position.x),
			Mathf.Floor (transform.position.y)+0.5f,
			Mathf.Round (transform.position.z));

		gameObject.transform.position = setLocation;
		gameObject.transform.rotation = Quaternion.identity;
		gameObject.tag = "set_Cube";
		rb.isKinematic = true;


	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.name == "base_Block" ||col.gameObject.name == "death_Block" || col.gameObject.name == "Ground" ) {
			
			setFinalLocation();
		}
	}
	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody> ();
		rb.velocity = new Vector3 (0, -0.5f, 0);
		rb.isKinematic = false;
		gameObject.tag = "falling_Cube";
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 setFallingLocation = new Vector3(Mathf.Round (transform.position.x),
			(transform.position.y),
			Mathf.Round (transform.position.z));
		gameObject.transform.position = setFallingLocation;
		gameObject.transform.rotation = Quaternion.identity;

		if (Mathf.Abs (rb.velocity.y) < 0.5) {//redundent set
			
			setFinalLocation ();
		}

	}
}
