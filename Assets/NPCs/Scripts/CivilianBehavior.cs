﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CivilianBehavior : NPCBehavior {
	
	public int gatherLimit = 5;
	public float soldierConfidence = 5.0f;
	public Material[] materials;
	
	private enum MovementStates {Fleeing, Gathering, Hiding, Separating, Wandering};
	private MovementStates movementState;
	
	// Use this for initialization
	new void Start () {
		base.Start();
		
		movementState = MovementStates.Wandering;
		GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate() {
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		updateMovementState();
		
		switch (movementState) {
			
		case MovementStates.Fleeing:
			fleeZombies();
			break;
		case MovementStates.Gathering:
			gatherNearCivilans();
			break;
		case MovementStates.Hiding:
			hideNearSoldiers();
			break;
		case MovementStates.Separating:
			avoidCivilians();
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
		if (nearZombies.Count > 0) 
			movementState = MovementStates.Fleeing;
		else if (nearSoldiers.Count > 1 && nearCivilians.Count <= (nearSoldiers.Count * soldierConfidence) + gatherLimit) 
			movementState = MovementStates.Hiding;
		else if (nearCivilians.Count > 1 && nearCivilians.Count <= gatherLimit) 
			movementState = MovementStates.Gathering;
		else if (nearCivilians.Count > (nearSoldiers.Count * soldierConfidence) + gatherLimit)
			movementState = MovementStates.Separating;
		else
			movementState = MovementStates.Wandering;
	}
	
	private void fleeZombies() {
		GameObject nearestZombie = findNearestObject(nearZombies);
		
		if (nearestZombie == null)
			cleanLists();
		else {
			Vector3 distanceVector = transform.position - nearestZombie.transform.position;
			updateDestination(transform.position + distanceVector);	
		}
	}
	
	private void gatherNearCivilans() {
		Vector3 centroid = calculateCentroid(nearCivilians);
		float distance = Vector3.Distance(centroid, transform.position);
		if (distance > 0.0f) {
			updateDestination(centroid);
		}
	}
	
	private void hideNearSoldiers() {
		updateDestination(findFarthestObject(nearSoldiers).transform.position);
	}
	
	private void avoidCivilians() {
		Vector3 centroid = calculateCentroid(nearCivilians);
		Vector3 distanceVector = transform.position - centroid;

		updateDestination(transform.position + distanceVector);
	}
	
	override public void handleDestroy(GameObject destroyedObject) {
		removeNearObject(destroyedObject);
	}
}
