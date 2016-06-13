#pragma strict

var gs:GlavnaSkripta;

function Start()
{
	gs = GameObject.FindWithTag("GameController").GetComponent(GlavnaSkripta);
}

function OnLeftClick()
{
if (GlavnaSkripta.myGameType == gameType.Multi) return;

if (!GlavnaSkripta.isOver && GlavnaSkripta.myGuiStat != guiStatus.newGameOn)
	gs.Pauziraj();
	
	GlavnaSkripta.BtnClickSnd();
}