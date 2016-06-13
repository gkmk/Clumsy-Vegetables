using UnityEngine;
using System.Collections;

public class SaveMe : MonoBehaviour {

	void OnLeftClick()
	{
		GlavnaSkripta.BtnClickSnd();
		GameObject.FindWithTag("GameController").GetComponent<GlavnaSkripta>().saveMe();
	}
}
