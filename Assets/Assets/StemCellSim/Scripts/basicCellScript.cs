using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class basicCellScript: MonoBehaviour{
	
	public float maxVelocity;
	public float frequency;

	public GameObject tacCell;
	public GameObject container;
	public GameObject manager;

	public float collisionCount;

	public void clampVelocity(Rigidbody rb){
		if (rb.velocity.magnitude > 3) {
			rb.velocity = rb.velocity.normalized * maxVelocity;
		}
	}
}
