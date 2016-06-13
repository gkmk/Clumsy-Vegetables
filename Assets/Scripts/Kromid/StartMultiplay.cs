using UnityEngine;
using System.Collections;

public class StartMultiplay : MonoBehaviour {

	//public Transform kromidPrefab;
	
	public void OnLeftClick()
	{
	/*	Transform tmp = Object.Instantiate (kromidPrefab, new Vector3(Camera.main.transform.position.x+5f, 4.0f, 0f), Quaternion.identity) as Transform ;
		tmp.parent = Camera.main.transform;
		foreach (SpriteRenderer go in tmp.GetComponentsInChildren<SpriteRenderer>()) {
			go.color = Color.cyan;
		}*/

		if (MultiplayerControler.inRoom) {
			MultiplayerControler.instantiateOthers();
			//GameObject.FindWithTag("Respawn").GetComponent("PreprekaSpawn").enabled = true;
			//GameObject.FindWithTag("Player").animation.Play("tasak3");
			
			GlavnaSkripta.myGuiStat=guiStatus.Idle;
			GlavnaSkripta.isPaused=false;
			GlavnaSkripta.myGameType = gameType.Multi;
			GameObject.FindWithTag("GameController").GetComponent<GlavnaSkripta>().resetGui();
			
			Parallax.forwardSpeed=4;
			
			if (MultiplayerControler.myself.ParticipantId == MultiplayerControler.participants[0].ParticipantId)
				GameObject.FindWithTag("Respawn").GetComponent<PreprekaSpawn>().enabled = true;
			
			GlavnaSkripta.BtnClickSnd();
			return;
		}
		if (Social.localUser.authenticated&&(PlayerPrefs.GetInt ("AutoLogin") == 1))
			MultiplayerControler.StartQuickGame();
		else {
			GameObject.FindWithTag("GameController").GetComponent<GlavnaSkripta>().GPGsignIn( () => {MultiplayerControler.StartQuickGame();} );
		}
	}
	
}
