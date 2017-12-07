using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeDiff : MonoBehaviour {
	public Text diff;
	public GameObject m1;
	public GameObject m2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		diff.text = (m2.GetComponent<managerScript> ().timer - m1.GetComponent<managerScript> ().timer).ToString ();
	}
}
