#pragma strict

function OnTriggerEnter2D(other: Collider2D) {
	if (other.gameObject.tag == "Obstacle")
		Destroy (other.transform.parent.gameObject);
	if (other.gameObject.tag == "Crate")
		Destroy (other.gameObject);
}
