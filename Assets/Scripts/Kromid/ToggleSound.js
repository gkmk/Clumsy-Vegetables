#pragma strict

var gs:GlavnaSkripta;

function Start()
{
	gs = GameObject.FindWithTag("GameController").GetComponent(GlavnaSkripta);
}

function OnLeftClick()
{
	gs.toggleSound();
	GlavnaSkripta.BtnClickSnd();
}