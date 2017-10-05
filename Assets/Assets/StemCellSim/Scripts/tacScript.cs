using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tacScript : basicCellScript {
	Rigidbody rb;
	managerScript m;
	Material c;

	public bool isDifferentiated = false;
	public bool isLeaving = false;
	float timer = 0f;
	float delay;
	public float contactTimer;



	public ParticleSystem apoptosisEffect;

	float leaveTimer = 0f;
	public float leaveDelay;
	// Use this for initialization
	void Start () {
		
		rb = GetComponent<Rigidbody> ();
		collisionCount = 0;
		manager = GameObject.Find ("Manager");
		m = manager.GetComponent<managerScript> ();
		c = GetComponent<Renderer>().material;

		maxVelocity = m.maxVelocity;
		delay = m.ageDelay;
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if (!isLeaving) {

			rb.AddForce (Random.insideUnitSphere*0.15f,ForceMode.Impulse);
			rb.AddForce (new Vector3 (0f, 0.2f, 0f), ForceMode.Impulse);
			clampVelocity (rb);
		}
		if (!isDifferentiated) {
			ageCell ();
		}
		destroyOnLeave ();


	}

	void OnCollisionEnter(Collision col){
		
		if (isDifferentiated) {
			if (col.gameObject.tag == "tac") {
			
				collisionCount++;



			}
		}
		


	}


	void ageCell (){
		if (timer > delay) {
			isDifferentiated = true;
		} else {
			timer += Time.deltaTime;

		}

		float t = timer / delay;
		if (t > 1f) {
			t = 1f;
		}

		changeColor (t);

	}

	void destroyOnLeave(){
		if (isLeaving) {
			leaveTimer += Time.deltaTime;
			if (leaveTimer > leaveDelay) {
				Destroy (gameObject);
			}
		}
	}

	public void changeColor(float t){
		if (c != null) {
			c.SetColor ("_Color", new Color (1, Mathf.Lerp (1, 0, t), Mathf.Lerp (1, 0, t), 1));
		}
	}
}
