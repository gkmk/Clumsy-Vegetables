using UnityEngine;
using System.Collections;
using GooglePlayGames;

public class ShowLeaders : MonoBehaviour {

	void OnLeftClick () {
		GlavnaSkripta.BtnClickSnd();
		if (Social.localUser.authenticated&&(PlayerPrefs.GetInt ("AutoLogin") == 1))
		{
			// show achievements  UI
			((PlayGamesPlatform) Social.Active).ShowLeaderboardUI("CgkIzZ7Nn4gBEAIQAQ");
		}else {
			GameObject.FindWithTag("GameController").GetComponent<GlavnaSkripta>().GPGsignIn( () => {((PlayGamesPlatform) Social.Active).ShowLeaderboardUI("CgkIzZ7Nn4gBEAIQAQ");} );
		}
	}

}
