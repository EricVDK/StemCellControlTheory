using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

	public Slider removalSlider;
	public Slider maturingSlider;
	public Slider sensitivitySlider;

	public Text removalText;
	public Text maturingText;
	public Text sensitivityText;

//	public Text diff;

	public GameObject manager;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		removalText.text = "Amount of Cells to Remove: " + removalSlider.value.ToString();
		maturingText.text = "Maturation Delay (seconds): " + maturingSlider.value.ToString("#.0");
		sensitivityText.text = "Apoptosis Threshold (collisions/second): " + sensitivitySlider.value.ToString("#.00");

	//	diff.text = (manager.GetComponent<managerScriptFeedback> ().timer - manager.GetComponent<ManagerScriptStatic> ().timer).ToString ();
	}
}
