﻿using UnityEngine;
using System.Collections;

public class SoldierBehavior : NPCBehavior {
	
	public float bravery = 5.0f;
	public float accuracy = 1.0f;
	public float shootingSpeed = 1.0f;
	public LayerMask obstacleMask;
	
	private enum MovementStates {Retreating, Following, Wandering};
	private MovementStates movementState;
	private GameObject target;
	private float lastShot;
	
	// Use this for initialization
	new void Start () {
		base.Start();
		
		lastShot = Time.time;
		movementState = MovementStates.Wandering;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate() {
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		updateMovementState();
		
		switch (movementState) {
			
		case MovementStates.Retreating:
			retreat();
			break;
		case MovementStates.Following:
			followZombie();
			break;
		case MovementStates.Wandering:
			wanderAround();
			break;
		default:
			wanderAround();
			break;
		}
		
		shootAtTarget();
	}
	
	private void updateMovementState() {
		if (nearZombies.Count > 0) {
			GameObject nearestZombie = findNearestObject(nearZombies);
			if (nearestZombie == null)
			{
				cleanLists();
				movementState = MovementStates.Wandering;
			}
			else if (Vector3.Distance(nearestZombie.transform.position, transform.position) > bravery)
				movementState = MovementStates.Following;
			else
				movementState = MovementStates.Retreating;
		} 
		else
			movementState = MovementStates.Wandering;
	}
	
	private void retreat() {
		GameObject nearestZombie = findNearestObject(nearZombies);
		Vector3 distanceVector = transform.position - nearestZombie.transform.position;
		
		updateDestination(transform.position + distanceVector);
		target = nearestZombie;
	}
	
	private void followZombie() {
		target = findNearestObject(nearZombies);

		if (target != null)
			updateDestination(target.transform.position);
		else
		{
			cleanLists();
			updateDestination(wanderTarget);
		}
	}
	
	private void shootAtTarget() {
		if (target != null && Time.time - lastShot > 1.0f / shootingSpeed) {
			lastShot = Time.time;
			float angle = calculateShotVariance();
			RaycastHit hit;
			Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.black, 2.0f);
			if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, 40, obstacleMask) && hit.transform == target.transform) {
				Destroy(target);
			}
		}
	}
	
	private float calculateShotVariance() {
			return 0.0f;
	}
	
	override public void handleDestroy(GameObject destroyedObject) {
		removeNearObject(destroyedObject);
		if (target == destroyedObject)
			target = null;
	}
}
