using System; 
using UnityEngine;
using System.Collections;
using Soomla;

public class GUIControl : MonoBehaviour {

	public Vector2 scrollPosition = Vector2.zero;
	public GUISkin mainSkin;

	private Rect windowRect = new Rect (Screen.width/8, 25, Screen.width/4*3, Screen.height-50);
	private Rect animWindowRect;
	private int animY = Screen.height+100;
	private bool animating;
	private float t;
	private float [] buyOffset;
	private int newSizerIcons;
	private float newSizerBtnIcon;
	private float spacer;

	private int rewardIndex=-1;

	private static ExampleEventHandler handler;
	
	public void initAllStyle()
	{
		mainSkin.window.fontSize = (int)GlavnaSkripta.FontSizeMultXLarge;
		mainSkin.box.fontSize = (int)GlavnaSkripta.FontSizeMultNormal;
		mainSkin.label.fontSize = (int)GlavnaSkripta.FontSizeMultSmall;
		mainSkin.button.fontSize = (int)GlavnaSkripta.FontSizeMultBig;
		mainSkin.GetStyle("Coins").fontSize = (int)GlavnaSkripta.FontSizeMultNormal;
		
		animating=false;
		animWindowRect = windowRect;
		
		mainSkin.window.contentOffset = new Vector2(0, ((Screen.height-50)/4+20)/-1.63f);
		mainSkin.window.padding.left = mainSkin.window.padding.right = (Screen.width/4*3)/16;
		mainSkin.window.padding.bottom = (Screen.width/4*3)/30;
		mainSkin.window.padding.top = (Screen.height-50)/4+20;
		
		mainSkin.GetStyle("BuyBtns").fontSize = (int)GlavnaSkripta.FontSizeMultLarge;		
		mainSkin.GetStyle("BuyBtns").padding.top = mainSkin.GetStyle("BuyBtns").padding.bottom = (int)(GlavnaSkripta.FontSizeMultLarge*.5f);
		mainSkin.GetStyle("BuyBtns").padding.right = (int)(GlavnaSkripta.FontSizeMultLarge);
		
		newSizerIcons = mainSkin.GetStyle("BuyBtns").fontSize+mainSkin.GetStyle("BuyBtns").padding.top*2;
		newSizerIcons += (int)(newSizerIcons*.1f);
		newSizerBtnIcon = mainSkin.GetStyle("BuyBtns").fontSize+mainSkin.GetStyle("BuyBtns").padding.top;
		mainSkin.GetStyle("BuyBtns").padding.left = (int)newSizerBtnIcon+mainSkin.GetStyle("BuyBtns").padding.right;
		
		//+mainSkin.GetStyle("BuyBtns").padding.left+mainSkin.box.padding.right
		
		buyOffset = new float[2]{0, mainSkin.box.padding.top+mainSkin.GetStyle("BuyBtns").padding.top-mainSkin.GetStyle("BuyBtns").padding.top/4};//newSizerBtnIcon-newSizerBtnIcon/2.5f
		
		spacer = mainSkin.box.padding.top+mainSkin.box.padding.bottom+mainSkin.box.margin.top+mainSkin.box.margin.bottom+mainSkin.GetStyle("BuyBtns").fontSize+
			mainSkin.GetStyle("BuyBtns").padding.top*2+mainSkin.GetStyle("BuyBtns").padding.top/2;
		
		//	if (buyOffset[0] == 0){
		Vector2 wBtnSize = mainSkin.GetStyle("BuyBtns").CalcSize(new GUIContent ("buy"));
		//new GUIStyle ("Label").CalcMinMaxWidth (new GUIContent ("x100"), out temp, out wsize);
		buyOffset[0] = windowRect.width-newSizerBtnIcon-wBtnSize.x-mainSkin.window.padding.right;
	}
	

	void Start()
	{
		initAllStyle();

		handler = new ExampleEventHandler();
		
		StoreController.Initialize(new CVStore());
	}

	/*void Update()
	{
		if (animating){
			if (t<1f) {	
				t += 0.1f;
				animWindowRect.y=Mathf.Lerp(animY, windowRect.y, t);
			}
		}
	}*/
	
	/*
	**
	*	GUI funkciata, momentalno samo za QUIT prikazuva dialoze
	**
	*/
	void OnGUI() {

		GUI.skin = mainSkin;
		
		if (animating){
			if (t<1f) {	
				t += 0.1f;
				animWindowRect.y=Mathf.Lerp(animY, windowRect.y, t);
			}
		}
		
		if (GlavnaSkripta.myGuiStat == guiStatus.googlePlusLogin)
		{
			GUI.Window (2, animWindowRect, GooglePlusLogin, "Login");
		}

		if (GlavnaSkripta.myGuiStat == guiStatus.inMultiplayer)
		{
			GUI.Window (2, animWindowRect, OnlineMultiplay, "Online");
		}

		if (GlavnaSkripta.myGuiStat == guiStatus.showMarket)	{
			if (!animating)
			{
				animWindowRect = windowRect;

				animWindowRect.y=animY;
				t=0f;
			}
			animating=true;

			GUI.Window (0, animWindowRect, DoMarketPlace, "Store");
		} else animating=false;		

		if (GlavnaSkripta.myGuiStat == guiStatus.newGameOn)
		{
			int offX= Screen.width/2-205;
			int offY= Screen.height/2-200;
			
			mainSkin.label.fontSize = (int)GlavnaSkripta.FontSizeMultBig;
			mainSkin.label.alignment = TextAnchor.MiddleCenter;
			GUI.color = Color.black;
			GUI.Label( new Rect(offX+2,offY+2,410,100), "Tap anywhere on the screen to begin");
			GUI.color = Color.white;
			GUI.Label( new Rect(offX,offY,410,100), "Tap anywhere on the screen to begin");
			mainSkin.label.fontSize = (int)GlavnaSkripta.FontSizeMultSmall;
			mainSkin.label.alignment = TextAnchor.MiddleLeft;
			
			GUI.DrawTexture(new Rect(Screen.width/2, Screen.height/2, 92, 140),(Texture2D)Resources.Load("finger"));
		}
		
		if(GlavnaSkripta.myGuiStat == guiStatus.ExitGuiOn){//check if gui should be on. If false, the gui is off, if true, the gui is on
			// Make a background box Screen.width,Screen.height
			int offX = Screen.width/2-200;
			int offY = Screen.height/2-95;
			GUI.Box ( new Rect(offX,offY,400,190), "You sure you want to quit?");
			// Make the first button. If pressed, quit game 
			if (GUI.Button ( new Rect(offX+30,offY+80,150,80), "Yes")) {
				GlavnaSkripta.BtnClickSnd();
				try{GlavnaSkripta.CM.SaveToCloud ();}catch(Exception e){}
				Application.Quit();				
			}
			// Make the second button.If pressed, sets the var to false so that gui disappear
			if (GUI.Button ( new Rect(offX+210,offY+80,150,80), "No")) {
				GlavnaSkripta.myGuiStat = guiStatus.Idle;
				GlavnaSkripta.BtnClickSnd();
			}
		}

		if (GK.Achievements.hasNewAch() && ExampleLocalStoreInfo.VirtualGoods != null && GlavnaSkripta.isOver)
		{
			GlavnaSkripta.myGuiStat = guiStatus.inAchievements;
			if (rewardIndex <0)rewardIndex = UnityEngine.Random.Range(0, 2);
			GUI.Window (1, animWindowRect, showAchAwards, "Reward");
		}
		

		#if UNITY_IOS || UNITY_ANDROID
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			scrollPosition.y += Input.GetTouch(0).deltaPosition.y;
		}
		#endif

	}
	
	void GooglePlusLogin(int winID)
	{
		mainSkin.label.alignment = TextAnchor.MiddleCenter;
		mainSkin.label.fontSize = (int)GlavnaSkripta.FontSizeMultBig;
		GUILayout.Label("Connecting to Google.\nPlease wait...");
		mainSkin.label.alignment = TextAnchor.MiddleLeft;
		mainSkin.label.fontSize = (int)GlavnaSkripta.FontSizeMultSmall;
	}
	
	void OnlineMultiplay(int winID)
	{
		mainSkin.label.fontSize = (int)GlavnaSkripta.FontSizeMultBig;
		mainSkin.label.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label(MultiplayerControler.status);
		if (MultiplayerControler.progres!=-1)
			GUILayout.Label("Progress: "+MultiplayerControler.progres);

		mainSkin.button.stretchWidth = true;
		mainSkin.button.fontSize = (int)GlavnaSkripta.FontSizeMultBig;
		
		if (GUILayout.Button("Leave room")) {
			MultiplayerControler.LeaveRoom();
		}
		mainSkin.button.stretchWidth = false;
		mainSkin.label.fontSize = (int)GlavnaSkripta.FontSizeMultSmall;
		mainSkin.label.alignment = TextAnchor.MiddleLeft;
	}

	void showAchAwards(int windowID) 
	{
		
		mainSkin.label.fontSize = (int)GlavnaSkripta.FontSizeMultBig;
		mainSkin.label.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label("You have unlocked new achievement!");
		GUILayout.Label(GK.Achievements.getCurrentAch());
		//GUILayout.Label("tasace");
		//GUILayout.Space(50);
		GUILayout.BeginHorizontal("box");
		//mainSkin.label.stretchHeight = false;
		//GUILayout.Label((Texture2D)Resources.Load("SoomlaStore/images/Rocket"),GUILayout.Height(newSizerIcons),GUILayout.Width(newSizerIcons));	
		GUILayout.Label((Texture2D)Resources.Load("SoomlaStore/images/" + ExampleLocalStoreInfo.VirtualGoods[rewardIndex].Name),GUILayout.Height(newSizerIcons),GUILayout.Width(newSizerIcons));	
		GUILayout.Label("You have been awarded: "+ExampleLocalStoreInfo.VirtualGoods[rewardIndex].Name,GUILayout.Height(newSizerIcons));
		//GUILayout.Label("You have been awarded: Rocket",GUILayout.Height(newSizerIcons));
		//mainSkin.label.stretchHeight = true;
		GUILayout.EndHorizontal();
		mainSkin.button.stretchWidth = true;
		mainSkin.button.fontSize = (int)GlavnaSkripta.FontSizeMultBig;
		if (GUILayout.Button("Click Here to Claim Reward!")) {
			StoreInventory.GiveItem(ExampleLocalStoreInfo.VirtualGoods[rewardIndex].ItemId, 1);
			GK.Achievements.getNextAch();
			rewardIndex=-1;
			GlavnaSkripta.myGuiStat = guiStatus.Idle;
		}
		mainSkin.button.stretchWidth = false;
		mainSkin.label.fontSize = (int)GlavnaSkripta.FontSizeMultSmall;
		mainSkin.label.alignment = TextAnchor.MiddleLeft;
	}

	// Make the contents of the window
	void DoMarketPlace (int windowID) {
		//if (ExampleLocalStoreInfo.CurrencyBalance == -1) return;


		if (GUI.Button (new Rect (animWindowRect.width - newSizerIcons, animWindowRect.height/14, newSizerIcons, newSizerIcons), "", mainSkin.GetStyle ("CloseBtn"))) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			StoreController.StopIabServiceInBg();
			#endif

			GlavnaSkripta.BtnClickSnd();
			GlavnaSkripta.myGuiStat = guiStatus.inMenu;
			GlavnaSkripta.ImaRaketi=ExampleLocalStoreInfo.GoodsBalances[CVStore.ROCKET_ITEM_ID];
				}
		GUILayout.Label("Available coins "+ExampleLocalStoreInfo.CurrencyBalance);

		// Starts an area to draw elements
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);

		#if UNITY_EDITOR
		GUILayout.BeginHorizontal("box");		
		GUILayout.Space(newSizerIcons+15+mainSkin.box.padding.left);
		GUILayout.Label("x <size="+((int)GlavnaSkripta.FontSizeMultLarge).ToString()+">1000</size>",GUILayout.Height(newSizerIcons));
		
		if(GUILayout.Button("buy",mainSkin.GetStyle("BuyBtns"))){
			GlavnaSkripta.BtnClickSnd();
		//	Debug.Log("SOOMLA/UNITY wants to buy: Rocket");
			try {
				//StoreInventory.BuyItem(vg.ItemId);
			} catch (Exception e) {
			//	Debug.Log ("SOOMLA/UNITY " + e.Message);
			}
		}

		GUI.Label(new Rect (buyOffset[0], buyOffset[1], newSizerBtnIcon, newSizerBtnIcon), "22", mainSkin.GetStyle ("Coins"));
		GUI.DrawTexture(new Rect(mainSkin.box.padding.left, mainSkin.box.padding.top, newSizerIcons, newSizerIcons),(Texture2D)Resources.Load("SoomlaStore/images/Rocket"));		
		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal("box");		
		GUILayout.Space(newSizerIcons+15+mainSkin.box.padding.left);
		GUILayout.Label("x <size="+((int)GlavnaSkripta.FontSizeMultLarge).ToString()+">150</size>",GUILayout.MinHeight(newSizerIcons));
		
		if(GUILayout.Button("buy",mainSkin.GetStyle("BuyBtns"))){
			GlavnaSkripta.BtnClickSnd();
			//Debug.Log("SOOMLA/UNITY wants to buy: Water Drop");
			try {
				//StoreInventory.BuyItem(vg.ItemId);
			} catch (Exception e) {
				//Debug.Log ("SOOMLA/UNITY " + e.Message);
			}
		}
		GUI.Label(new Rect (buyOffset[0], buyOffset[1]+spacer, newSizerBtnIcon, newSizerBtnIcon), "12", mainSkin.GetStyle ("Coins"));
		GUI.DrawTexture(new Rect(mainSkin.box.padding.left, mainSkin.box.padding.top+spacer, newSizerIcons, newSizerIcons),(Texture2D)Resources.Load("SoomlaStore/images/Water Drop"));		
		GUILayout.EndHorizontal();
		#endif

		//	goods --------------------------------------------------------------------------
		if (ExampleLocalStoreInfo.VirtualGoods != null){
		float y = 0;
		foreach(VirtualGood vg in ExampleLocalStoreInfo.VirtualGoods){
			GUILayout.BeginHorizontal("box");

				GUILayout.Space(newSizerIcons+15+mainSkin.box.padding.left);
				GUILayout.Label("x <size="+((int)GlavnaSkripta.FontSizeMultLarge).ToString()+">"+ExampleLocalStoreInfo.GoodsBalances[vg.ItemId]+"</size>",GUILayout.MinHeight(newSizerIcons));

			if(GUILayout.Button("buy",mainSkin.GetStyle("BuyBtns"))){
				GlavnaSkripta.BtnClickSnd();
				//Debug.Log("SOOMLA/UNITY wants to buy: " + vg.Name);
				try {
					StoreInventory.BuyItem(vg.ItemId);
				} catch (Exception e) {
				//	Debug.Log ("SOOMLA/UNITY " + e.Message);
				}
			}
				GUI.Label(new Rect (buyOffset[0], buyOffset[1]+y, newSizerBtnIcon, newSizerBtnIcon), ((PurchaseWithVirtualItem)vg.PurchaseType).Amount.ToString(), mainSkin.GetStyle ("Coins"));
				GUI.DrawTexture(new Rect(mainSkin.box.padding.left, mainSkin.box.padding.top+y, newSizerIcons, newSizerIcons),(Texture2D)Resources.Load("SoomlaStore/images/" + vg.Name));
				y+= spacer;		

			GUILayout.EndHorizontal();
		}
		}

		//	paricki --------------------------------------------------------------
		if (ExampleLocalStoreInfo.VirtualCurrencyPacks != null){
			//float y = 0;
			foreach(VirtualCurrencyPack cp in ExampleLocalStoreInfo.VirtualCurrencyPacks){
				GUILayout.BeginHorizontal("box");

				GUILayout.Label("", mainSkin.GetStyle("CoinsBuy"),GUILayout.Width(100),GUILayout.Height(100));
				GUILayout.Label("<color=#ece606ff>"+cp.Name+"</color>",GUILayout.MinHeight(100));

				if(GUILayout.Button("$"+((PurchaseWithMarket)cp.PurchaseType).MarketItem.Price.ToString("0.00"), mainSkin.GetStyle ("Coins"), GUILayout.Width (100), GUILayout.Height (100))){
					GlavnaSkripta.BtnClickSnd();
					//Debug.Log("SOOMLA/UNITY wants to buy: " + cp.Name);
					try {
						StoreInventory.BuyItem(cp.ItemId);
					} catch (Exception e) {
					//	Debug.Log ("SOOMLA/UNITY " + e.Message);
					}
				}
				//y+= spacer;				
				GUILayout.EndHorizontal();
			}
		}
/*
		//	RAKETI
			GUILayout.BeginHorizontal("box");
		GUILayout.Label("", mainSkin.GetStyle("Rockets"),GUILayout.Width(newSizerIcons),GUILayout.Height(newSizerIcons));
		GUILayout.Label("x <size="+((int)GlavnaSkripta.FontSizeMultLarge).ToString()+">"+PlayerPrefs.GetInt ("Player Rockets")+"</size>",GUILayout.MinHeight(newSizerBtnIcon));

		//GUILayout.Label("<b><i><color=#ece606ff>1 Rocket</color></i></b>\n<size="+((int)GlavnaSkripta.FontSizeMultSmall).ToString()+
		 //               ">Powerfull rocket to crush any obstacle on your path!</size>",GUILayout.MinHeight(100));
		if (GUILayout.Button ("buy",mainSkin.GetStyle("BuyBtns"))) {
			try {
				VirtualGood vg = CVStore.ROCKET_GOOD;
				StoreInventory.BuyItem(vg.ItemId);
				PlayerPrefs.SetInt ("Player Rockets", PlayerPrefs.GetInt ("Player Rockets") + 1);
			} catch (Exception e) {
				Debug.Log ("SOOMLA/UNITY " + e.Message);
			}
						/*if (PlayerPrefs.GetInt ("Player Coins") >= 10) {
								PlayerPrefs.SetInt ("Player Coins", PlayerPrefs.GetInt ("Player Coins") - 10);
								PlayerPrefs.SetInt ("Player Rockets", PlayerPrefs.GetInt ("Player Rockets") + 1);
						}*/
		/*
				GlavnaSkripta.BtnClickSnd();
				}
		GUI.Label(new Rect (buyOffset[0], buyOffset[1], newSizerBtnIcon, newSizerBtnIcon), "10", mainSkin.GetStyle ("Coins"));
		//GUI.Label(new Rect (buyOffset[0], buyOffset[1], 24f, 24f), "10");
			GUILayout.EndHorizontal();

		//	save ME CLOUDCE
		GUILayout.BeginHorizontal("box");
		GUILayout.Label("", mainSkin.GetStyle("SaveMe"),GUILayout.Width(newSizerIcons),GUILayout.Height(newSizerIcons-newSizerIcons/5));
		GUILayout.Label("x <size="+((int)GlavnaSkripta.FontSizeMultLarge).ToString()+">"+PlayerPrefs.GetInt ("Water Drop")+"</size>",GUILayout.MinHeight(newSizerBtnIcon));
		//GUILayout.Label("<color=#ece606ff>1 Water Drop</color>\nYou own "+PlayerPrefs.GetInt ("Water Drop"),GUILayout.MinHeight(100));
		if (GUILayout.Button ("buy", mainSkin.GetStyle("BuyBtns"))) {
			if (PlayerPrefs.GetInt ("Player Coins") >= 25) {
				PlayerPrefs.SetInt ("Player Coins", PlayerPrefs.GetInt ("Player Coins") - 25);
				PlayerPrefs.SetInt ("Water Drop", PlayerPrefs.GetInt ("Water Drop") + 1);
			}
			GlavnaSkripta.BtnClickSnd();
		}
		GUI.Label(new Rect (buyOffset[0], buyOffset[1]+spacer, newSizerBtnIcon, newSizerBtnIcon), "25", mainSkin.GetStyle ("Coins"));
		GUILayout.EndHorizontal();

		//	COINSOVI
			GUILayout.BeginHorizontal("box");
		GUILayout.Label("", mainSkin.GetStyle("CoinsBuy"),GUILayout.Width(100),GUILayout.Height(100));
		GUILayout.Label("<color=#ece606ff>100 Coins</color>",GUILayout.MinHeight(100));
		if (GUILayout.Button("$1.99", mainSkin.GetStyle ("Coins"), GUILayout.Width (100), GUILayout.Height (100)))
		{
			VirtualCurrencyPack vg = CVStore.HUNCOINS_PACK;
			Debug.Log("SOOMLA/UNITY wants to buy: " + vg.Name);
			try {
				StoreInventory.BuyItem(vg.ItemId);
			} catch (Exception e) {
				Debug.Log ("SOOMLA/UNITY " + e.Message);
			}
		}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal("box");
		GUILayout.Label("", mainSkin.GetStyle("CoinsBuy"),GUILayout.Width(100),GUILayout.Height(100));
		GUILayout.Label("<color=#ece606ff>300 Coins</color>",GUILayout.MinHeight(100));
		if (GUILayout.Button("$4.99", mainSkin.GetStyle ("Coins"), GUILayout.Width (100), GUILayout.Height (100)))
		{
			VirtualCurrencyPack vg = CVStore.TREHUNCOINS_PACK;
			Debug.Log("SOOMLA/UNITY wants to buy: " + vg.Name);
			try {
				StoreInventory.BuyItem(vg.ItemId);
			} catch (Exception e) {
				Debug.Log ("SOOMLA/UNITY " + e.Message);
			}
		}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal("box");
		GUILayout.Label("", mainSkin.GetStyle("CoinsBuy"),GUILayout.Width(100),GUILayout.Height(100));
		GUILayout.Label("<color=#ece606ff>700 Coins</color>",GUILayout.MinHeight(100));
		if (GUILayout.Button("$9.99", mainSkin.GetStyle ("Coins"), GUILayout.Width (100), GUILayout.Height (100)))
		{
			VirtualCurrencyPack vg = CVStore.SEVHUNCOINS_PACK;
			Debug.Log("SOOMLA/UNITY wants to buy: " + vg.Name);
			try {
				StoreInventory.BuyItem(vg.ItemId);
			} catch (Exception e) {
				Debug.Log ("SOOMLA/UNITY " + e.Message);
			}
		}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal("box");
		GUILayout.Label("", mainSkin.GetStyle("CoinsBuy"),GUILayout.Width(100),GUILayout.Height(100));
		GUILayout.Label("<color=#ece606ff>1500 Coins</color>",GUILayout.MinHeight(100));
		if (GUILayout.Button("$19.99", mainSkin.GetStyle ("Coins"), GUILayout.Width (100), GUILayout.Height (100)))
		{
			VirtualCurrencyPack vg = CVStore.FIFTHUNCOINS_PACK;
			Debug.Log("SOOMLA/UNITY wants to buy: " + vg.Name);
			try {
				StoreInventory.BuyItem(vg.ItemId);
			} catch (Exception e) {
				Debug.Log ("SOOMLA/UNITY " + e.Message);
			}
		}
			GUILayout.EndHorizontal();*/

		GUILayout.EndScrollView ();
	}
}
