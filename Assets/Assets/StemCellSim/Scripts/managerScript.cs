using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class managerScript : MonoBehaviour {
	// why type of sim is it managing?
	public bool apoptosisSim;


	// cell behaviour variables
	public float maxVelocity;
	public float creationRate;
	public float ageDelay;
	public float removalRate;
	public float maxStorage;
	public float targetNumber;
	public float disasterRemove;

	public float max;

	float removalTimer = 0f;
	float creationTimer = 0f;

	float apoptosisTimer = 0f;
	float apoptosisRate;

	// cell list management
	public List<GameObject> stemList;
	public List<GameObject> tacList;
	public List<GameObject> DCList;

	//particle effects
	public ParticleSystem apoptosisEffect;

	//timing
	public bool isTiming;
	public float timer;

	//vars from ui
	public Slider removalSlider;
	public Slider maturationSlider;
	public Slider sensitivitySlider;

	public Text recoveryText;
	// Use this for initialization
	void Start () {
		
		//stemList = new List<GameObject> ();
		tacList = new List<GameObject> ();
		DCList = new List<GameObject> ();

		//timing setUp
		isTiming = false;
		timer = 0;

	}
	
	// Update is called once per frame
	void Update () {
		disasterRemove = removalSlider.value;
		ageDelay = maturationSlider.value;
		max = sensitivitySlider.value;


		checkAge ();
		if (apoptosisSim) {
			passiveCreation ();
			collisionBasedRemoval ();
		} else {
			staticCreation ();
		}

		timerUpdate ();
		recoveryText.text = "Time to Recover (seconds): " + timer.ToString();
		//passiveRemoval ();
		//updateApoptosisRate ();
		//apoptosis ();

	}



	void checkAge(){
		for (int i = 0; i < tacList.Count; i++) {
			if (tacList [i].GetComponent<tacScript> ().isDifferentiated) {
				GameObject c = tacList [i];
				tacList.RemoveAt (i);
				c.layer = 12;
				DCList.Add (c);
			}
		}
	}

	void passiveCreation(){
		creationTimer += Time.deltaTime;
		if (creationTimer > creationRate && stemList.Count > 0) {
			creationTimer = 0f;

			for (int i = 0; i < stemList.Count; i++) {
				stemList [i].GetComponent<stemCellScript> ().divide ();
				}

						
		}
	}

	void passiveRemoval(){
		removalTimer += Time.deltaTime;

		if (removalTimer > removalRate && DCList.Count > 0) {
			
			removalTimer = 0;
			int r = Random.Range (0,DCList.Count);
			GameObject c = DCList [r];
			DCList.RemoveAt (r);
			c.GetComponent<SphereCollider> ().isTrigger = true;
			c.GetComponent<tacScript> ().isLeaving = true;
			c.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 5f, 0f);
			//Destroy (c);
			Debug.Log (DCList.Count);

		}
	}

	void updateApoptosisRate(){
		


		float apoptosisEquilibrium = 1f/creationRate - 1f/removalRate;


		float error = (targetNumber - DCList.Count)/targetNumber;

		float ratioMap = Mathf.Pow(-1f*(error-1f),3f);
		apoptosisRate = 1 / (apoptosisEquilibrium * ratioMap);
		//Debug.Log (apoptosisRate);
	}



	void apoptosis(){
		apoptosisTimer += Time.deltaTime;
		if (apoptosisTimer > apoptosisRate) {

			apoptosisTimer = 0;
			if (DCList.Count > 0) {

				int rand = UnityEngine.Random.Range (0, DCList.Count);
				GameObject cell = DCList [rand];

				DCList.RemoveAt (rand);
				//ParticleSystem ps = Instantiate (apop);
				//ps.transform.position = droplet.transform.position;
				ParticleSystem ps = Instantiate (apoptosisEffect);
				ps.transform.position = cell.transform.position;
				Destroy (cell);





			}
		}
	}

	void collisionBasedRemoval(){
		//destroy cells above limit
		for (int i = 0; i < tacList.Count; i++) {
			if (tacList [i].GetComponent<tacScript> ().collisionCount > max) {
				GameObject cell = tacList [i];
				tacList.RemoveAt (i);
				ParticleSystem ps = Instantiate (apoptosisEffect);
				ps.transform.position = cell.transform.position;
				Destroy (cell);
			}
		}

		for (int i = 0; i < DCList.Count; i++) {
			if (DCList [i].GetComponent<tacScript> ().collisionCount > max) {
				GameObject cell = DCList [i];
				DCList.RemoveAt (i);
				ParticleSystem ps = Instantiate (apoptosisEffect);
				ps.transform.position = cell.transform.position;
				Destroy (cell);
			}
		}



/////////////////////subtract cells collision value over time

		for (int i = 0; i < tacList.Count; i++) {
			if (tacList [i].GetComponent<tacScript> ().collisionCount > 0) {
				tacList [i].GetComponent<tacScript> ().collisionCount -= Time.deltaTime;
			}
		}

		for (int i = 0; i < DCList.Count; i++) {
			if (DCList [i].GetComponent<tacScript> ().collisionCount > 0) {
				DCList [i].GetComponent<tacScript> ().collisionCount -= Time.deltaTime;
			}
		}


	}



	public void staticCreation(){
		creationTimer += Time.deltaTime;
		if (creationTimer > creationRate && stemList.Count > 0 && (DCList.Count + tacList.Count < targetNumber)) {
			creationTimer = 0f;

			for (int i = 0; i < stemList.Count; i++) {
				stemList [i].GetComponent<stemCellScript> ().divide ();
			}


		}
	}

	public void disaster(){
		isTiming = true;
		timer = 0f;
		targetNumber = DCList.Count;

		for (int i = 0; i < disasterRemove; i++) {
			if (DCList.Count > 0) {
				int rand = UnityEngine.Random.Range (0, DCList.Count);
				GameObject droplet = DCList [rand];
				DCList.RemoveAt (rand);
				ParticleSystem ps = Instantiate (apoptosisEffect);
				ps.transform.position = droplet.transform.position;
				Destroy (droplet);
				Debug.Log ("ran");
			}
		}
	}

	void timerUpdate(){
		if(isTiming){
			timer += Time.deltaTime;
		}
		if (DCList.Count > (targetNumber*0.8)) {
			isTiming = false;
		}
	}







}
