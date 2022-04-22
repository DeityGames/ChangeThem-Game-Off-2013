using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

abstract public class NPCBehavior : MonoBehaviour {
	
	protected List<GameObject> nearZombies;
	protected List<GameObject> nearCivilians;
	protected List<GameObject> nearSoldiers;
	
	protected NavMeshAgent agent;
	protected Vector3 destination;
	protected Vector3 wanderTarget;
	
	protected void Start() {
		nearZombies = new List<GameObject>();
		nearCivilians = new List<GameObject>();
		nearSoldiers = new List<GameObject>();
		
		wanderTarget = generateRandomPosition(-45, 45, -45, 45);

		agent = transform.GetComponent<NavMeshAgent>();
		agent.SetDestination(wanderTarget);
	}
	
	abstract public void handleDestroy(GameObject destroyedObject);
	
	protected void OnDestroy() {		
		foreach (GameObject zombie in nearZombies) {
			if (zombie != null) zombie.transform.GetComponent<NPCBehavior>().handleDestroy(gameObject);
		}
		
		foreach (GameObject civilian in nearCivilians) {
			if (civilian != null) civilian.transform.GetComponent<NPCBehavior>().handleDestroy(gameObject);
		}
		
		foreach (GameObject soldier in nearSoldiers) {
			if (soldier != null) soldier.transform.GetComponent<NPCBehavior>().handleDestroy(gameObject);
		}
	}
	
	protected GameObject findNearestObject(List<GameObject> gameObjects) {
		GameObject nearestObject = null;
		float nearestDistance = Mathf.Infinity;
		
		foreach(GameObject gameObject in gameObjects) {
			if (gameObject != null) {
				float distance = Vector3.Distance(transform.position, gameObject.transform.position);
				if (distance < nearestDistance) {
					nearestDistance = distance;
					nearestObject = gameObject;
				}
			}
		}
		
		return nearestObject;
	}
	
	protected GameObject findFarthestObject(List<GameObject> gameObjects) {
		GameObject farthestObject = null;
		float farthestDistance = 0.0f;
		
		foreach(GameObject gameObject in gameObjects) {
			float distance = Vector3.Distance(transform.position, gameObject.transform.position);
			if (distance > farthestDistance) {
				farthestDistance = distance;
				farthestObject = gameObject;
			}
		}
		
		return farthestObject;
	}
	
	protected Vector3 calculateCentroid(List<GameObject> gameObjects) {
		Vector3 centroid = new Vector3();
		int count = 0;
		
		foreach(GameObject gameObject in gameObjects) {
			if (gameObject != null) {
				centroid += gameObject.transform.position;
				count++;	
			}
		}
		
		centroid /= count;
		
		return centroid;
	}
	
	protected Vector3 generateRandomPosition(float xMin, float xMax, float zMin, float zMax) {
		bool foundPosition = false;
		Vector3 newPosition = Vector3.zero;
		
		while (!foundPosition) {
			foundPosition = true;
			newPosition = new Vector3(Random.Range(xMin, xMax), 0.0f, Random.Range(zMin, zMax));
			Collider[] hitColliders = Physics.OverlapSphere(newPosition, 5.0f);
			for (int i = 0; i < hitColliders.Length && foundPosition; i++) {
				if (hitColliders[i].transform.tag == "Building") foundPosition = false;
			}
			
		}
		return newPosition;
	}

	protected void updateDestination(Vector3 destination)
	{
		agent.SetDestination(destination);
		this.destination = destination;
	}

	protected void wanderAround()
	{
		if (destination == null || Vector3.Distance(transform.position, destination) < 10.0f)
		{
			wanderTarget = generateRandomPosition(-45, 45, -45, 45);
			updateDestination(wanderTarget);
		}
	}

	public void addNearObject(GameObject nearObject) {
		if (nearObject != gameObject) {
			switch (nearObject.tag) {
			
			case "Player":	
				break;
			case "Zombie":
				addNearZombie(nearObject);
				break;
			case "Civilian":
				addNearCivilian(nearObject);
				break;
			case "Soldier":
				addNearSoldier(nearObject);
				break;
			default:
				break;
			}
		}
	}
	
	public void addNearZombie(GameObject zombie) {
		if (zombie != gameObject) nearZombies.Add(zombie);
	}
	
	public void addNearCivilian(GameObject civilian) {
		if (civilian != gameObject) nearCivilians.Add(civilian);	
	}
	
	public void addNearSoldier(GameObject soldier) {
		if (soldier != gameObject) nearSoldiers.Add(soldier);
	}
	
	public void removeNearObject(GameObject nearObject) {
		switch (nearObject.tag) {
		
		case "Player":	
			break;
		case "Zombie":
			removeNearZombie(nearObject);
			break;
		case "Civilian":
			removeNearCivilian(nearObject);
			break;
		case "Soldier":
			removeNearSoldier(nearObject);
			break;
		default:
			break;
		}
	}
	
	public void removeNearZombie(GameObject zombie) {
		nearZombies.Remove(zombie);
	}
	
	public void removeNearCivilian(GameObject civilian) {
		nearCivilians.Remove(civilian);
	}
	
	public void removeNearSoldier(GameObject soldier) {
		nearSoldiers.Remove(soldier);
	}
	
	public void cleanLists() {
		nearZombies.RemoveAll(x => x == null);
		nearCivilians.RemoveAll(x => x == null);
		nearSoldiers.RemoveAll(x => x == null);
	}
}
