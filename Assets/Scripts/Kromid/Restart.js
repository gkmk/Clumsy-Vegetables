#pragma strict

function OnLeftClick()
{
	GlavnaSkripta.BtnClickSnd();
	GlavnaSkripta.myGuiStat = guiStatus.Idle;
	Parallax.forwardSpeed=4f;
	Application.LoadLevel("MainLevel");
}