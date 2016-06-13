using UnityEngine;
using System.Collections;

public class AddRigidBody : MonoBehaviour {

	void OnTriggerEnter2D ( Collider2D col) 
	{
		//Debug.Log("trigrre triger tgirer "+col.gameObject.tag+" "+col.gameObject);
		if (col.gameObject.tag == "Obstacle") {
			Transform obsParent = col.transform.parent;
			for (var i=0; i<obsParent.childCount; i++) {
				Transform currChild = obsParent.GetChild (i);

				if (!currChild.gameObject.GetComponent ("Rigidbody2D")) {			
						Rigidbody2D rigBody = currChild.gameObject.AddComponent ("Rigidbody2D") as Rigidbody2D;
						rigBody.collider2D.isTrigger = false;
						rigBody.mass = 0.05f;
						//Debug.Log("added rigid2d to "+currChild);
				} else {
						//Debug.Log("already has rigid2d to "+currChild);
						break;
				}
			}
		}
	}
}
