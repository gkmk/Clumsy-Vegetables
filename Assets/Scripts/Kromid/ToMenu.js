#pragma strict

function OnLeftClick()
{
	GlavnaSkripta.myGuiStat = guiStatus.inMenu;
	Application.LoadLevel("MainLevel");
}