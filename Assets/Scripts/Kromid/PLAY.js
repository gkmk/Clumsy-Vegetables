#pragma strict

function OnLeftClick()
{
	transform.position.y = 4f;

	GameObject.FindWithTag("Respawn").GetComponent(PreprekaSpawn).enabled = true;
	GameObject.FindWithTag("Player").animation.Play("tasak3");
	
	GlavnaSkripta.myGuiStat=guiStatus.Idle;
	GlavnaSkripta.isPaused=false;
	GlavnaSkripta.myGameType = gameType.Single;
	GameObject.FindWithTag("GameController").GetComponent(GlavnaSkripta).resetGui();

Parallax.forwardSpeed=4;

	Camera.main.transform.position.Set(0f,0f,-10f);
	
	GlavnaSkripta.BtnClickSnd();
}