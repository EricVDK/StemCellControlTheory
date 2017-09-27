using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ManagerScriptStatic: MonoBehaviour {

	public Text timerText;

	bool isTiming = false;
	public float timer = 0f;

	List<GameObject> B = new List<GameObject>();
	List<GameObject> C = new List<GameObject>();

	public Slider destroy;
	public Slider maturing;
	public Slider target;

	public ParticleSystem removal;

	bool[] gridB = new bool[100];
	bool[] gridC = new bool[100];

	public GameObject maturingPlate;
	public GameObject storagePlate;
	public GameObject droplet;

	int gridCountB = 0;
	int gridCountC = 0;

	public float creationRate;
	float creationTimer = 0f;


	public float maturingDelay;

	public float storageCount;
	public float storageMax;


	float apoptosisRate;

	public float demandRate;

	float exitTimer = 0f;
	float apoptosisTimer = 0f;

	public float targetPercent = 0.7f;
	public float disasterRemove = 10f;
	public Text bCount;
	public float b_Count;
	public Text cCount;
	public float c_Count;



	// Use this for initialization
	void Start () {
		for (int i = 0; i < gridB.Length; i++) {
			gridB [i] = false;
			gridC [i] = false;
		}
	}



	// Update is called once per frame
	void Update () {
		targetPercent = target.value / 100;
		maturingDelay = maturing.value;
		disasterRemove = destroy.value;

		b_Count = B.Count;
		c_Count = C.Count;
		bCount.text = "# of Droplets: " + b_Count.ToString();
		cCount.text = "# of Droplets: " + c_Count.ToString();

		createDroplet ();
		matureDroplets ();
		dropletExit ();
		timerUpdate ();
		timerText.text = "Reactionary Strategy: " + timer.ToString();
	}


	void createDroplet (){
		creationTimer += Time.deltaTime;
		if (creationTimer > creationRate && (B.Count+C.Count < targetPercent*storageMax-1f)) {
			creationTimer = 0;

			for (int i = gridCountB; i < gridB.Length; i++) {
				if (gridB [i]) {
					gridCountB++;
					if (gridCountB > 99) {
						gridCountB = 0;
					}
				} else {

					break;
				}
			}



			Vector3 position = new Vector3 ((100f/10f)*(gridCountB%10), 1f, -(100f/10f)*(gridCountB-gridCountB%10)/10f);


			position = position + maturingPlate.transform.position;
			position = position - new Vector3 (100f / 2f, 0f, -100f / 2f);
			position = position + new Vector3 (100f / 20f, 0f, -100f / 20f);

			GameObject d = Instantiate (droplet);
			d.transform.position = position;
			d.GetComponent<Droplet> ().delay = maturingDelay;
			d.GetComponent<Droplet> ().gridSpotB = gridCountB;
			B.Add (d);

			gridCountB++;
			if (gridCountB > 99) {
				gridCountB = 0;
			}

		}

	}

	void matureDroplets(){
		for (int i = 0; i < B.Count; i++) {
			GameObject droplet = B [i];

			droplet.GetComponent<Droplet> ().currentTime += Time.deltaTime;

			float progress = droplet.GetComponent<Droplet> ().currentTime/ maturingDelay;
			if (progress > 1) {
				progress = 1;
			}
			droplet.GetComponent<Droplet> ().changeColor (progress);


			if(droplet.GetComponent<Droplet>().currentTime > droplet.GetComponent<Droplet>().delay){

				C.Add (droplet);
			//	droplet.transform.position = new Vector3(1000f,1000f,1000f);
				gridB [B [i].GetComponent<Droplet> ().gridSpotB] = false;
				B.RemoveAt (i);








				for (int j= gridCountC; j < gridC.Length; j++) {
					if (gridC [j]) {
						gridCountC++;
						if (gridCountC > 99) {
							gridCountC = 0;
						}
					} else {
						break;
					}
				}


			
			

				Vector3 position = new Vector3 ((100f/10f)*(gridCountC%10), 1f, -(100f/10f)*(gridCountC-gridCountC%10)/10f);


				position = position + storagePlate.transform.position;
				position = position - new Vector3 (100f / 2f, 0f, -100f / 2f);
				position = position + new Vector3 (100f / 20f, 0f, -100f / 20f);
				droplet.GetComponent<Droplet> ().gridSpotC = gridCountC;


				gridC [gridCountC] = true;
				droplet.transform.position = position;
		
				gridCountC++;

				if (gridCountC > 99) {
					gridCountC = 0;
				}



			}


		}
	}

	void dropletExit(){


		if (exitTimer < demandRate) {
			exitTimer += Time.deltaTime;
		}

		if (C.Count > 0) {
			if (exitTimer >= demandRate) {
				exitTimer = 0;
				int rand = UnityEngine.Random.Range (0, C.Count);
				GameObject droplet = C [rand];

				gridC [C [rand].GetComponent<Droplet> ().gridSpotC] = false;
				C.RemoveAt (rand);
				Destroy (droplet);
			}
		}
	}

	void apoptosis(){
		apoptosisTimer += Time.deltaTime;
		if (apoptosisTimer > apoptosisRate) {
			
			apoptosisTimer = 0;
			if (C.Count > 0) {
				

				int rand = UnityEngine.Random.Range (0, C.Count);
				GameObject droplet = C [rand];

				gridC [C [rand].GetComponent<Droplet> ().gridSpotC] = false;
				C.RemoveAt (rand);
				Destroy (droplet);





			}
		}
	}
	void updateApoptosisRate(){
		//apoptosisRate = apoptosisRateBase / ((storageMax - C.Count));
		//apoptosisRate = (storageMax - C.Count);
		float x = (C.Count)/storageMax;


		float apoptosisEquilibrium = 1f/creationRate - 1f/demandRate;

		float error = targetPercent - x;

		//float ratioMap = 2f / (error + 1f) - 1f;
		float ratioMap = Mathf.Pow(-1f*(error-1f),29f);
		apoptosisRate = 1 / (apoptosisEquilibrium * ratioMap);

	}

	public void disaster(){
		isTiming = true;
		timer = 0f;
		for (int i = 0; i < disasterRemove; i++) {
			if (C.Count > 0) {
				int rand = UnityEngine.Random.Range (0, C.Count);
				GameObject droplet = C [rand];

				gridC [C [rand].GetComponent<Droplet> ().gridSpotC] = false;
				C.RemoveAt (rand);
				ParticleSystem ps = Instantiate (removal);
				ps.transform.position = droplet.transform.position;
				Destroy (droplet);
			} else {
				return;
			}
		}
	}

	void timerUpdate(){
		if(isTiming){
			timer += Time.deltaTime;
		}
		if (C.Count > (targetPercent * storageMax) - 3f) {
			isTiming = false;
		}
	}

}
