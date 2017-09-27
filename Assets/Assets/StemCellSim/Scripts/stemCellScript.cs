using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stemCellScript : basicCellScript {

	Rigidbody rb;
	managerScript m;



	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		container = GameObject.Find ("ColonCryptContainer");
		//manager = GameObject.Find ("Manager");
		m = manager.GetComponent<managerScript> ();

		maxVelocity = m.maxVelocity;
		frequency = m.creationRate;
		transform.SetParent (container.transform);
	}
	
	// Update is called once per frame
	void Update () {
		clampVelocity (rb);
	

	}

	public void divide(){
		
			
			
			GameObject s = Instantiate (tacCell);

			s.transform.parent = container.transform;
			Vector3 daughter = Random.insideUnitSphere.normalized;
		daughter = new Vector3 (daughter.x, Mathf.Abs(daughter.y), daughter.z);
		s.transform.localPosition = new Vector3 (gameObject.transform.localPosition.x + daughter.x/10f, gameObject.transform.localPosition.y + daughter.y/10f, gameObject.transform.localPosition.z + daughter.z/10f);

			m.tacList.Add (s);

	
}

}