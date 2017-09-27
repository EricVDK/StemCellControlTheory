using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : MonoBehaviour {
	public float delay;
	public float currentTime;
	public int gridSpotB;
	public int gridSpotC;

	Material m;
	// Use this for initialization
	void Start () {
		m = GetComponent<Renderer>().material;

	}
		
	public void changeColor(float t){
		if (m != null) {
			m.SetColor ("_Color", new Color (Mathf.Lerp (1, 0, t), Mathf.Lerp (0, 1, t), 0, 1));
		}
	}

}
