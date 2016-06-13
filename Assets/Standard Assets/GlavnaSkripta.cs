using System; 
using UnityEngine;
using System.Collections;
using GooglePlayGames;
using Soomla;
using System.Collections.Generic;

public enum guiStatus { Idle, ExitGuiOn, inMenu, newGameOn, showMarket, inMultiplayer, googlePlusLogin, inAchievements };
public enum gameType { Single, Multi, Idle }; 

public class GlavnaSkripta : MonoBehaviour
{
		public Sprite[]muteBtn;
		public AudioClip buttonClip;
		public static GameObject selfSkriptGO;

		// params
		private bool mWaitingForAuth;
		public static  int Poeni = 0;
		public static int ImaRaketi;
		public static bool isPaused = true;
		public static bool isOver;
		public static  bool saveMeOn;
		private int maxPoeni = 0;
		private int playerCoins = 0;
		private bool enableSounds = true;
		private bool allCached = false;
		private GameObject kromiradro;
	
		//	pause Menu
		private GameObject thePauseMenu;
		private GameObject headerTxt;
		private GameObject highScore;
		private GameObject playerCoinsGUI;
		private GameObject score;
		private GameObject sounds;
	
		//	gui
		
		public static guiStatus myGuiStat;
		public static gameType myGameType;
		
		/*static public bool ExitGuiOn = false;
		static public bool newGameOn = false;
		static public bool showMarket = false;
		public static bool inMenu = true;
		*/
		private GameObject theMainMenu;
		private GameObject inGameGUI;
		private GameObject poeniGUI;
		private GUIText poeniGUIShadow;
		private GameObject raketiGUI;
		private GUIText raketiGUIShadow;
		private GameObject rocketBtn;
		private GameObject pauseBtn;
		private GameObject Overlay;
		private GameObject MuteBtn;
		private GameObject SaveMeBtn;
	
		//	consts
		static public float FontSizeMultSmall = 0.042f;
		static public float FontSizeMultNormal = 0.05f;
		static public float FontSizeMultBig = 0.056f;
		static public float FontSizeMultLarge = 0.07f;
		static public float FontSizeMultXLarge = 0.08f;
		private float PosResOffset = 1f;
		private string gameAspectRatio;
		
		private static GlavnaSkripta instance = null;

		// cloud save callbacks
		public static CloudManager CM;
		
		//	Online multiplayer
		//public static Multiplayer OnlineGame;
		//-------------------------------------------------------------------------------
	
	
		void  Awake ()
		{
		//	sredi fontovite
		initFonts ();
		
		if (instance == null) { 	// making sure we only initialize one instance.
						instance = this;
						GameObject.DontDestroyOnLoad (this.gameObject);
						CM = new CloudManager ();
						Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;						
				} else {				// Destroying unused instances.
						GameObject.Destroy (this.gameObject);
				}

				AudioListener.pause = true;
				AudioListener.volume = 0.0F;
				
				
				//	IZBRISI ZA FINAL RELEASE
				//PlayerPrefs.SetInt ("Player Coins", 10000);
				//PlayerPrefs.SetInt ("First Flight", 0);
		}
		//-------------------------------------------------------------------------------
	
		/*
**
*	Resetiranje na nekoi stvari pred da pocne da igra
**
*/
		void  Start ()
		{		
		Application.targetFrameRate = 30;
		myGameType=gameType.Idle;
		myGuiStat = guiStatus.inMenu;
				PlayGamesPlatform.Activate ();
				//OnlineGame = new Multiplayer();
			
				if (PlayerPrefs.GetInt ("AutoLogin") == 1) {
						try {
								GPGsignIn (null);
						} catch (Exception e) {
								Debug.Log (e.Message);
						}
				}
				//	ucituvaj igrata
				Application.LoadLevel ("MainLevel");

				selfSkriptGO = GameObject.FindWithTag ("GameController");
				
			audio.Play();
	//	GlavnaSkripta.myGuiStat = guiStatus.googlePlusLogin;

				//	IN APP STORE
				//StoreController.Initialize(new CVStore());	
		
	}
		//-------------------------------------------------------------------------------

		/*
**
*	Na kopce klik zvuk
**
*/
		public static void BtnClickSnd ()
		{
				selfSkriptGO.audio.PlayOneShot (selfSkriptGO.GetComponent<GlavnaSkripta> ().buttonClip);
		}
		//-------------------------------------------------------------------------------


		/*
**
*	Resetiranje na nekoi stvari pred da pocne da igra
**
*/
		void OnApplicationFocus (bool focusStatus)
		{
				if (!enableSounds) {
						AudioListener.pause = true;
						AudioListener.volume = 0.0F;
				}
				selfSkriptGO = GameObject.FindWithTag ("GameController");
		}
		//-------------------------------------------------------------------------------

		void  OnLevelWasLoaded (int level)
		{
				//	sredi fontovite
				initFonts ();
				
				//	sregi gi variablite
				initVars ();
		
				//	kesiraj gi objektite od narednio level
				cacheAllObjects ();
		
				//	sredi rezolucija
				doAspectAdjust ();	
		
				//	sredi fontovi na objektite
				applyFontSize ();
		
				//	resetiraj gi ubavite raboti :D
				resetGui ();
				
		}
		//-------------------------------------------------------------------------------
	
		/*
**
*	Ubao gi sreduva rabotite :D
**
*/
		public void  resetGui ()
		{
			if (myGuiStat == guiStatus.inMenu || myGuiStat == guiStatus.googlePlusLogin) {
						theMainMenu.SetActive (true);
						inGameGUI.SetActive (false);
						rocketBtn.SetActive (false);
						pauseBtn.SetActive (false);
						GoogleAnalytics.instance.LogScreen ("In Menu");
						myGameType=gameType.Idle;
				} else {
						theMainMenu.SetActive (false);
						inGameGUI.SetActive (true);
						rocketBtn.SetActive (true);
						if (myGameType != gameType.Multi)	pauseBtn.SetActive (true);
						GoogleAnalytics.instance.LogScreen ("Playing");
				}
				Overlay.SetActive (false);

				if (enableSounds) {
						AudioListener.pause = false;
						AudioListener.volume = 1.0F;
						MuteBtn.GetComponent<SpriteRenderer> ().sprite = muteBtn [1];
				} else {
						MuteBtn.GetComponent<SpriteRenderer> ().sprite = muteBtn [0];
				}
				SaveMeBtn.SetActive (false);
				thePauseMenu.SetActive (false);
		}
		//-------------------------------------------------------------------------------
	
		/*
**
*	Dodeluvanje na dinamicni FontSize na objektite
**
*/
		void  applyFontSize ()
		{
				poeniGUI.guiText.fontSize = (int)FontSizeMultXLarge;
				poeniGUIShadow.fontSize = (int)FontSizeMultXLarge;
		
				raketiGUI.guiText.fontSize = (int)FontSizeMultLarge;
				raketiGUIShadow.fontSize = (int)FontSizeMultLarge;
		
				headerTxt.guiText.fontSize = (int)FontSizeMultBig;
				highScore.guiText.fontSize = (int)FontSizeMultBig;
				score.guiText.fontSize = (int)FontSizeMultBig;
				playerCoinsGUI.guiText.fontSize = (int)FontSizeMultBig;
				sounds.guiText.fontSize = (int)FontSizeMultBig;
		}
		//-------------------------------------------------------------------------------
	
		/*
**
*	Kesiranje na site objekti za posle da moze da se koristat
**
*/
		void  cacheAllObjects ()
		{
				theMainMenu = GameObject.FindWithTag ("MainMenu");
				inGameGUI = GameObject.FindWithTag ("InGameGUI");
				thePauseMenu = GameObject.FindWithTag ("PauseMenu");
		
				rocketBtn = GameObject.FindWithTag ("rocketBTN");
				pauseBtn = GameObject.FindWithTag ("pauseBTN");

				Overlay = GameObject.FindWithTag ("Overlay");

				MuteBtn = GameObject.FindWithTag ("MuteBtn");
				kromiradro = GameObject.FindWithTag ("Player");
				SaveMeBtn = GameObject.FindWithTag ("SaveMeBtn");
		
				for (int i=0; i<inGameGUI.transform.childCount; i++) {
						Transform nextChild = inGameGUI.transform.GetChild (i);
						switch (nextChild.gameObject.name) {
						case "poeniGUI":
								poeniGUI = nextChild.gameObject;
								break;
						case "raketiGUI":
								raketiGUI = nextChild.gameObject;
								break;
						case "pauseHeader":
								headerTxt = nextChild.gameObject;
								break;
						case "pauseCoins":
								playerCoinsGUI = nextChild.gameObject;
								break;
						case "pauseHighscore":
								highScore = nextChild.gameObject;
								break;
						case "pauseScore":
								score = nextChild.gameObject;
								break;
						case "pauseSoundTxt":
								sounds = nextChild.gameObject;
								break;			
						}
				}
		
				poeniGUIShadow = poeniGUI.transform.GetChild (0).guiText;
				raketiGUIShadow = raketiGUI.transform.GetChild (0).guiText;

				allCached = true;
		}
		//-------------------------------------------------------------------------------
	
		/*
**
*	Resetira gi site variabli tuka
**
*/
		void  initVars ()
		{
				saveMeOn = false;
				allCached = false;
				Poeni = 0;
				isPaused = false;
		

				ImaRaketi = StoreInventory.GetItemBalance (CVStore.ROCKET_ITEM_ID);
				if (PlayerPrefs.GetInt("FirstRockets")==0) {
					
					StoreInventory.GiveItem(CVStore.ROCKET_ITEM_ID, 10);
					PlayerPrefs.SetInt ("FirstRockets", 10);
					ImaRaketi=StoreInventory.GetItemBalance(CVStore.ROCKET_ITEM_ID);
				}
				isOver = false;
				Time.timeScale = 1;
				maxPoeni = PlayerPrefs.GetInt ("Player Score");
				playerCoins = ExampleLocalStoreInfo.CurrencyBalance;
				if (PlayerPrefs.GetInt ("enableSounds", 1) == 1)
						enableSounds = true;
				else
						enableSounds = false;
		
			if (myGuiStat != guiStatus.inMenu && myGuiStat != guiStatus.googlePlusLogin)
					myGuiStat = guiStatus.newGameOn;
		//GlavnaSkripta.myGuiStat = guiStatus.googlePlusLogin;
		}
		//-------------------------------------------------------------------------------
	
		/*
**
*	Presmetuva novi golemini na fontovi zavisi od rezolucijata
**
*/
		void  initFonts ()		
		{	
		FontSizeMultSmall = 0.042f;
		FontSizeMultNormal = 0.05f;
		FontSizeMultBig = 0.056f;
		FontSizeMultLarge = 0.07f;
		FontSizeMultXLarge = 0.1f;
			//	font sizes
				FontSizeMultXLarge *= Screen.height;
				FontSizeMultLarge *= Screen.height;
				FontSizeMultBig *= Screen.height;
				FontSizeMultNormal *= Screen.height;
				FontSizeMultSmall *= Screen.height;

				//FontSizeMultXLarge = FontSizeMultXLarge>48?48:FontSizeMultXLarge;
				FontSizeMultLarge = FontSizeMultXLarge > 42 ? 42 : FontSizeMultLarge;
				FontSizeMultBig = FontSizeMultBig > 36 ? 36 : FontSizeMultBig;
				FontSizeMultNormal = FontSizeMultNormal > 24 ? 24 : FontSizeMultNormal;
				FontSizeMultSmall = FontSizeMultSmall > 18 ? 18 : FontSizeMultSmall;
		}

		//-------------------------------------------------------------------------------
	
	
		/*
**
*	Updajtuva se zivo so novi pozii
**
*/
		void  doAspectAdjust ()
		{
				Vector2 aspectRatioTMP = myAspectRatio.GetAspectRatio (Screen.width, Screen.height);
				gameAspectRatio = "" + aspectRatioTMP.x + ":" + aspectRatioTMP.y;
		
				if (gameAspectRatio == "8:5") //	16:10
						PosResOffset = 1.1502590673575129533678756476684f;
				else if (gameAspectRatio == "5:3") //	
						PosResOffset = 1.0829268292682926829268292682927f;
		
				raketiGUI.transform.position.Set (raketiGUI.transform.position.x / PosResOffset, raketiGUI.transform.position.y, raketiGUI.transform.position.z);
		}
		//-------------------------------------------------------------------------------

	
		/*
**
*	Updajtuva poeni i moze da ja gase igrata
**
*/
		void  Update ()
		{
				if (myGuiStat != guiStatus.inMenu) {
					if (myGameType==gameType.Multi)
					{
						poeniGUI.guiText.text = "" + PreprekaSpawn.TimeCountDown;
						poeniGUIShadow.text = "" + PreprekaSpawn.TimeCountDown;
					}
					else {
						poeniGUI.guiText.text = "" + Poeni;
						poeniGUIShadow.text = "" + Poeni;
						}
						raketiGUI.guiText.text = "" + ImaRaketi;
						raketiGUIShadow.text = "" + ImaRaketi;
				}

				if (allCached) {
			if (myGuiStat != guiStatus.Idle && myGuiStat != guiStatus.inMenu)
								Overlay.SetActive (true);
						else
								Overlay.SetActive (false);
				}
				//	iskluci ja igrata
				if (Input.GetKeyUp ("escape")) {
						if (myGuiStat == guiStatus.inMenu)
							myGuiStat = guiStatus.ExitGuiOn;
					else if (myGameType == gameType.Multi) MultiplayerControler.LeaveRoom();
					else if (!isOver && myGuiStat != guiStatus.newGameOn && myGameType == gameType.Single)
						Pauziraj ();
			else if (myGuiStat != guiStatus.Idle && myGuiStat != guiStatus.inMenu) {
						myGuiStat = guiStatus.inMenu;
										#if UNITY_ANDROID && !UNITY_EDITOR
						Soomla.StoreController.StopIabServiceInBg();
										#endif
										}
					
			else myGuiStat = guiStatus.ExitGuiOn;
		}
		}
		//-------------------------------------------------------------------------------
	
		/*
**
*	Pauzira ja igrata i pokazuva meni so opcii
**
*/
		public void  Pauziraj ()
		{
				isPaused = !isPaused;
		
				if (isPaused) {
						thePauseMenu.SetActive (true);
						StartCoroutine (thePauseMenu.animation.Play ("menuAnim", false, null));
			
						Time.timeScale = 0;
						headerTxt.guiText.text = "pause";
						highScore.guiText.text = "highscore: " + maxPoeni;
						score.guiText.text = "score: " + Poeni;
						playerCoinsGUI.guiText.text = "coins: " + (Poeni / 5);
						if (!enableSounds)
								sounds.guiText.text = "off";
						else
								sounds.guiText.text = "on";
				} else {
						thePauseMenu.animation ["menuAnim"].speed = -1;
						thePauseMenu.animation.Play ("menuAnim");
						Time.timeScale = 1;
						headerTxt.guiText.text = "";
						highScore.guiText.text = "";
						score.guiText.text = "";
						sounds.guiText.text = "";
						playerCoinsGUI.guiText.text = "";
				}
		}
		//-------------------------------------------------------------------------------
	
		/*
**
*	Pauzira ja igrata i pokazuva meni so opcii
**
*/
		public void  toggleSound ()
		{
				enableSounds = !enableSounds;
				if (isPaused) {
						if (!enableSounds)
								sounds.guiText.text = "off";
						else
								sounds.guiText.text = "on";
				}
				if (enableSounds) {
						MuteBtn.GetComponent<SpriteRenderer> ().sprite = muteBtn [1];
						AudioListener.pause = false;
						AudioListener.volume = 1.0F;
				} else {
						MuteBtn.GetComponent<SpriteRenderer> ().sprite = muteBtn [0];
						AudioListener.pause = true;
						AudioListener.volume = 0.0F;
				}
				PlayerPrefs.SetInt ("enableSounds", enableSounds ? 1 : 0);
		}
		//-------------------------------------------------------------------------------

		/*
**
*	Da produze od tam kaj so zastanal ako iskoriste oblace
**
*/
		public void  saveMe ()
		{
				if (isOver && ExampleLocalStoreInfo.GoodsBalances [CVStore.WATER_DROP_ITEM_ID] > 0) {
			
						myGuiStat = guiStatus.Idle;
						saveMeOn = true;
						/*foreach (SpriteRenderer go in kromiradro.GetComponentsInChildren<SpriteRenderer>()) {
								go.color = Color.red;
						}*/
			 			
						Pauziraj ();
						isOver = false;

						StoreInventory.TakeItem (CVStore.WATER_DROP_ITEM_ID, 1);
						StoreInventory.TakeItem (CVStore.COIN_CURRENCY_ITEM_ID, Poeni / 5);
						if (myGameType != gameType.Multi)
						{
							PlayerPrefs.SetInt ("OveralCoinsEarned", PlayerPrefs.GetInt ("OveralCoinsEarned") - Poeni / 5);
							PlayerPrefs.SetInt ("OveralSaves", PlayerPrefs.GetInt ("OveralSaves") + 1);
						}
					
						//Invoke ("afterSave", 3);
						
						kromiradro.GetComponent<HeroPlayer> ().MultiplayerHit();
				} else
						myGuiStat = guiStatus.showMarket;
		}
		//-------------------------------------------------------------------------------

		/*public void afterSave ()
		{
				saveMeOn = false;
				foreach (SpriteRenderer go in kromiradro.GetComponentsInChildren<SpriteRenderer>()) {
						go.color = Color.white;
				}
		}*/
		//-------------------------------------------------------------------------------
	
		/*
**
*	Kraj na igrata :D
**
*/
		public void  gameOver ()
		{
				isOver = true;
				SaveMeBtn.SetActive (true);
				//playerCoins += Poeni / 5;
		if (!isPaused)
			Pauziraj ();
		
		if (myGameType != gameType.Multi) {
				PlayerPrefs.SetInt ("OveralCoinsEarned", PlayerPrefs.GetInt ("OveralCoinsEarned") + Poeni / 5);
				StoreInventory.GiveItem (CVStore.COIN_CURRENCY_ITEM_ID, Poeni / 5);
				}
		
				if (maxPoeni < Poeni) {
						maxPoeni = Poeni;
						PlayerPrefs.SetInt ("Player Score", maxPoeni);
				}
				
				if (PlayerPrefs.GetInt ("AutoLogin") == 1) {
					Social.ReportScore (maxPoeni, "CgkIzZ7Nn4gBEAIQAQ", (bool success) => {
						// handle success or failure
					});
				}
		
				//	update user achievements
				GK.Achievements.updateAchievements ();
		
				
				headerTxt.guiText.text = "gameover";

		}
		//-------------------------------------------------------------------------------
	
		public void GPGsignOut ()
		{
				if (Social.localUser.authenticated)
						((PlayGamesPlatform)Social.Active).SignOut ();
		}
		//-------------------------------------------------------------------------------
	
		public void GPGsignIn (Action callback)
		{
			myGuiStat = guiStatus.googlePlusLogin;
				GoogleAnalytics.instance.LogScreen ("User SignIn");

				if (Social.localUser.authenticated) {
						CM.LoadFromCloud ();
						GlavnaSkripta.myGuiStat = guiStatus.inMenu;
					
						return;
				}
				if (!mWaitingForAuth) {
						mWaitingForAuth = true;
						Social.localUser.Authenticate ((bool success) => {
								mWaitingForAuth = false;
				
								if (success) {
									PlayerPrefs.SetInt ("AutoLogin", 1);
									CM.LoadFromCloud ();
					GlavnaSkripta.myGuiStat = guiStatus.inMenu;
									if (callback != null) {
											callback ();										
									}									
								} else {
									PlayerPrefs.SetInt ("AutoLogin", 0);
					GlavnaSkripta.myGuiStat = guiStatus.inMenu;
								}
						});
				}
		}
		//-------------------------------------------------------------------------------
		

		void OnApplicationQuit ()
		{								
				PlayerPrefs.Save ();
		}

	
}