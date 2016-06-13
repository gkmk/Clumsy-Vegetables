#pragma strict

var speed : float=0.01f;

function Update () {
	transform.Rotate(Vector3(0,0,speed*-1));
}