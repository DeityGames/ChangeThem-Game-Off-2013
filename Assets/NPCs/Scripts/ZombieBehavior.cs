﻿using UnityEngine;
using System.Collections;

public class ZombieBehavior : NPCBehavior {
	
	public Transform zombiePrefab;
	public float soldierHate; 
	
	private enum MovementStates {Chasing, Wandering};
	private MovementStates movementState;
	
	// Use this for initialization
	new void Start() {
		base.Start();
		
		movementState = MovementStates.Wandering;
		transform.parent = GameObject.Find("Zombies").transform;
	}
	
	void FixedUpdate() {
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		updateMovementState();
		
		switch (movementState) {
			
		case MovementStates.Chasing:
			chase();
			break;
		case MovementStates.Wandering:
			wanderAround();
			break;
		default:
			wanderAround();
			break;
		}
	}
	
	private void updateMovementState() {
		if (nearSoldiers.Count > 0 || nearCivilians.Count > 0) 
			movementState = MovementStates.Chasing;
		else
			movementState = MovementStates.Wandering;
	}
	
	private void chase() {
		GameObject nearestSoldier = findNearestObject(nearSoldiers);
		GameObject nearestCivilian = findNearestObject(nearCivilians);

		if (nearestSoldier != null && nearestCivilian == null)
			updateDestination(nearestSoldier.transform.position);
		else if (nearestCivilian != null && nearestSoldier == null)
			updateDestination(nearestCivilian.transform.position);
		else if (nearestCivilian == null && nearestCivilian == null)
		{
			cleanLists();
			updateDestination(wanderTarget);
		}
		else if (Vector3.Distance(nearestSoldier.transform.position, transform.position) < soldierHate * Vector3.Distance(nearestCivilian.transform.position, transform.position))
			updateDestination(nearestSoldier.transform.position);
		else
			updateDestination(nearestCivilian.transform.position);
	}
	
	void OnCollisionEnter(Collision collision) {
		Collider other = collision.collider;
		if(other.gameObject.tag == "Civilian" || other.gameObject.tag == "Soldier") {
			Vector3 position = other.transform.position;
			Quaternion rotation = other.transform.rotation;
			
			if (other.gameObject.transform.position == destination)
				updateDestination(wanderTarget);
			Destroy(other.gameObject);
			Instantiate(zombiePrefab, position, rotation);
		}
	}
	
	override public void handleDestroy(GameObject destroyedObject) {
		removeNearObject(destroyedObject);
	}
}
