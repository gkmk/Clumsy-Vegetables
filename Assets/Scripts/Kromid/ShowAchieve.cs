using UnityEngine;
using System.Collections;
using GooglePlayGames;

public class ShowAchieve : MonoBehaviour {

	void OnLeftClick () {
		GlavnaSkripta.BtnClickSnd();
		if (Social.localUser.authenticated&&(PlayerPrefs.GetInt ("AutoLogin") == 1))
		{
			// show achievements  UI
			Social.ShowAchievementsUI();
		}else {
			GameObject.FindWithTag("GameController").GetComponent<GlavnaSkripta>().GPGsignIn( () => {Social.ShowAchievementsUI();} );
		}
	}

}
