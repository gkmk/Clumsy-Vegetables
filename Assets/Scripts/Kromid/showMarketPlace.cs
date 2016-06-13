using UnityEngine;
using System.Collections;

public class showMarketPlace : MonoBehaviour {

	// Use this for initialization
	void OnLeftClick()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		Soomla.StoreController.StartIabServiceInBg();
		#endif
		GlavnaSkripta.BtnClickSnd();
		GlavnaSkripta.myGuiStat = guiStatus.showMarket;
		GoogleAnalytics.instance.LogScreen("In Store");
	}

}
