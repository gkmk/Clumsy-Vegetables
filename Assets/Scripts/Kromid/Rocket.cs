using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour 
{
	public GameObject explosion;		// Prefab of explosion effect.
	public float bombRadius = 10f;			// Radius within which enemies are killed.
	public float bombForce = 100f;			// Force that enemies are thrown from the blast.

	void Start () 
	{
		// Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
		Destroy(gameObject, 2);
	}


	void OnExplode()
	{
		// Find all the colliders on the Enemies layer within the bombRadius.
		Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Prepreki"));
		
		// For each collider...
		foreach(Collider2D en in enemies)
		{
			// Check if it has a rigidbody (since there is only one per enemy, on the parent).
			Rigidbody2D rb = en.rigidbody2D;
			//en.isTrigger = false;
			if(rb != null && rb.tag == "Obstacle")
			{
				// Find the Enemy script and set the enemy's health to zero.
				//rb.gameObject.GetComponent<Enemy>().HP = 0;
				
				// Find a vector from the bomb to the enemy.
				Vector3 deltaPos = rb.transform.position - transform.position;
				
				// Apply a force in this direction with a magnitude of bombForce.
				Vector3 force = deltaPos.normalized * bombForce;
				rb.AddForce(force);
			}
		}

		// Create a quaternion with a random rotation in the z-axis.
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// Instantiate the explosion where the rocket is with the random rotation.
		Instantiate(explosion, transform.position, randomRotation);
	}


	void OnTriggerEnter2D (Collider2D col) 
	{
		if (col.gameObject.tag == "Obstacle") {
			Transform obsParent = col.transform.parent;
			for (var i=0; i<obsParent.childCount; i++) {
				Transform currChild = obsParent.GetChild(i);
				
				if (!currChild.gameObject.GetComponent("Rigidbody2D")) {			
					Rigidbody2D rigBody = currChild.gameObject.AddComponent ("Rigidbody2D") as Rigidbody2D;
					rigBody.collider2D.isTrigger = false;
					rigBody.mass = 0.05f;
					//Debug.Log("added rigid2d to "+currChild);
				} else {
					//Debug.Log("already has rigid2d to "+currChild);
					break;
				}
			}
			// Call the explosion instantiation.
			OnExplode();
			
			// Destroy the rocket.
			Destroy (gameObject);
		}
	}
}
