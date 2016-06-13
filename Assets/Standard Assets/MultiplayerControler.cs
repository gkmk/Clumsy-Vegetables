using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;
using System.Linq;

public class MultiplayerControler : RealTimeMultiplayerListener {

	const int MinOpponents = 1, MaxOpponents = 3;
	const int GameVariant = 0;
	
	public static float progres = 0f;
	public static int numConnected=0;
	public static string status="";
	
	static MultiplayerControler sInstance = null;
	
	static public Participant myself;
	static public List<Participant> participants;
	
	float mRoomSetupStartTime = 0.0f;
	public static bool inRoom;
	
	private static Transform kromidPrefab;
	static Dictionary<string, Transform> kromidari;
	private Dictionary<string, Transform> serverPrepreki;
	private Dictionary<string, int> serverPoeni;
	
	static Rigidbody2D rocket;
	static Transform[] Prepreki;
	static Sprite[] Kocki;
	static Color[] boicki = new Color[3]{ Color.cyan, Color.red, Color.blue };
//	static Font mainFont;
	
	bool firstMultiSpawn;
	PreprekaSpawn mojtaPreprekaSpawn;
	
	bool startingUp=false;
	
	public static float startTime;
	
	public static void instantiateOthers()
	{
		int colorIndex = 0;
		for (int i=0; i<participants.Count; i++)
		{
			if(participants[i].ParticipantId == myself.ParticipantId) {
				GameObject.FindWithTag("Player").transform.position = new Vector3(-6.28f, 3.47f- 2.0f*i, 4f);
				GameObject.FindWithTag("Player").animation.Play("tasak3");
				continue;
			}
			
			
			Transform tmp = Object.Instantiate (kromidPrefab, new Vector3(-6.28f, 3.47f- 2.0f*i, 3f), Quaternion.identity) as Transform ;
			tmp.parent = Camera.main.transform;
			foreach (SpriteRenderer go in tmp.GetComponentsInChildren<SpriteRenderer>()) {
				go.color = boicki[colorIndex];
			}
			TextMesh tm = tmp.gameObject.GetComponent("TextMesh") as TextMesh;
			tm.text = participants[i].DisplayName;
			/*tm.font = mainFont;
			tm.lineSpacing=2.5f;
			tm.characterSize = 1f;
			tm.anchor = TextAnchor.LowerLeft;*/
			tm.color = boicki[colorIndex];
			
			
			tmp.tag = "Enemy";
			tmp.gameObject.layer = LayerMask.NameToLayer("Enemies");
			kromidari[participants[i].ParticipantId] = tmp ;
			
		//	Debug.Log("------ Instantiating " + participants[i].ParticipantId+ " pos:"+tmp.position);
			
			colorIndex++;
			tm.animation.Play("tasak3");
		}
		GlavnaSkripta.ImaRaketi = 2;
	}
	
	private MultiplayerControler() {
		mRoomSetupStartTime = Time.time;	
		kromidari = new Dictionary<string, Transform>();
		serverPrepreki = new Dictionary<string, Transform>();
		serverPoeni = new Dictionary<string, int>();
		
		ObjectCacher OCache = GameObject.FindWithTag("GameController").GetComponent<ObjectCacher>();
		rocket = OCache.Raketa;
		Prepreki = OCache.Prepreki;
		Kocki = OCache.Kocki;
		kromidPrefab = OCache.KromidPrefab;
		//mainFont = OCache.MainFont;
		
		mojtaPreprekaSpawn = GameObject.FindWithTag("Respawn").GetComponent<PreprekaSpawn>();
		
		startingUp=false;
		firstMultiSpawn=false;
	}
	
	public static void StartQuickGame()
	{
		//if (!firstMultiSpawn)
		//{
		GoogleAnalytics.instance.LogScreen ("Starting Multiplayer");
		GlavnaSkripta.myGuiStat = guiStatus.inMultiplayer;		
		GlavnaSkripta.isPaused=true;  
		GlavnaSkripta.isOver=false;
		status="Creating new game...";  
		
		sInstance = new MultiplayerControler();		
		inRoom=false;		
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MinOpponents, MaxOpponents,
		                                                    GameVariant, sInstance);
		     
		kromidari.Clear();			
		//} 
//		firstMultiSpawn=true;                         
	}
	
	public static void LeaveRoom()
	{
		if (inRoom)	PlayGamesPlatform.Instance.RealTime.LeaveRoom();
		else {
			GlavnaSkripta.myGuiStat = guiStatus.inMenu;
			Application.LoadLevel("MainLevel");
			}
	}
	
	public void OnRoomSetupProgress(float progress) {
		// update progress bar
		// (progress goes from 0.0 to 100.0)
		progres = progress;
		status = "Waiting for players...\nnote: in this mode you get 2 rockets\nin each round, and 60 seconds to race";
	}
	
	private void StartMultiGame(float startTime)
	{
		if (startingUp) return;
		
		startingUp=true;
		
		mojtaPreprekaSpawn.startAfter(startTime);

	}
	
	public void OnRoomConnected(bool success) {
		if (success) {
			progres = -1;
			firstMultiSpawn=true;
			inRoom=true;
			status = "Starting game...";
			myself = PlayGamesPlatform.Instance.RealTime.GetSelf();
		//	Debug.Log("My participant ID is " + myself.ParticipantId);
			
			participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
			
			numConnected = participants.Count;
			for (int i=0; i<numConnected; i++)
			{
				if(participants[i].ParticipantId == myself.ParticipantId) continue;
				/*
				Transform tmp = Object.Instantiate (kromidPrefab, new Vector3(0f, 4.0f-i * 2.0f, 0f), Quaternion.identity) as Transform ;
				tmp.parent = Camera.main.transform;
				foreach (SpriteRenderer go in tmp.GetComponentsInChildren<SpriteRenderer>()) {
					go.color = Color.cyan;
				}
				tmp.gameObject.animation.Play("tasak3");*/
				kromidari.Add(participants[i].ParticipantId, null );
				  
		//		Debug.Log("************* Added participant ID " + participants[i].ParticipantId);
			}
			
			
			// Successfully connected to room!
			// ...start playing game...
			
			status = "All connected, prepairing game...";
			GlavnaSkripta.myGameType = gameType.Multi;
			mojtaPreprekaSpawn.asClient = true;
			mojtaPreprekaSpawn.enabled =true;
			PreprekaSpawn.preprekiteCache.Clear();
			if (myself.ParticipantId == participants[0].ParticipantId)
				mojtaPreprekaSpawn.createObstacleBuffer(15);
							
		} else {
			// Error!
			// ...show error message to user...
			status = "Argh, smells like fried onion. Yes, thats an error :(";
		}
	}
	public static void SendMessage(string msg) {
		SendMessage( msg, false);
	}
	
	public static void SendMessage(string msg, bool reliable)
	{
		byte[] message = System.Text.ASCIIEncoding.Default.GetBytes(msg); // build your message
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliable, message);
	}
	
	public void printResults()
	{
		serverPoeni.Add(myself.ParticipantId, GlavnaSkripta.Poeni);
		List<KeyValuePair<string, int>> sorted = (from kv in serverPoeni orderby kv.Value descending select kv).ToList();
		
		string pobedniciFinal ="";
		int i=1;
		foreach (KeyValuePair<string, int> kv in sorted)
		{
			if (kv.Key == myself.ParticipantId) pobedniciFinal=pobedniciFinal+""+i+". You "+kv.Value+" points\n";
			else {
				string pName="";
				foreach (Participant p  in participants)
				{
					if (p.ParticipantId == kv.Key) {
						pName = p.DisplayName;
						break;
					}
				}
				pobedniciFinal=pobedniciFinal+""+i+". "+ pName+" "+kv.Value+" points\n";
				}
			i++;
		}
		status="Game Ended\n"+pobedniciFinal; 
	}
	
	public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data) {
		// handle message! (e.g. update player's avatar)
		if (senderId == myself.ParticipantId) return;
		
		string msg = System.Text.ASCIIEncoding.Default.GetString(data);
		
	//	Debug.Log("Multiplayer message from: "+senderId+" msg: "+msg);
		
		string[] msgs = msg.Split('^');
		
		switch (msgs[0]) 
		{
		//	kraj na igrata proveri poeni
		case "q": serverPoeni.Add(senderId, int.Parse(msgs[1]));
			//int a = serverPoeni.Count;
			//int b = participants.Count;
			//b--;
			status="Game Ended\nWaiting results...";
			if (serverPoeni.Count == kromidari.Count) {
				printResults();				
			}
		break;
			//	sync time starting game
		case "p": 
			startTime = float.Parse(msgs[1]);
			status="Starting game in "+startTime+" seconds"; 
			StartMultiGame(startTime);
			break;
			
		//	jump
		case "j": 
			kromidari[senderId].transform.position = new Vector3(kromidari[senderId].transform.position.x, float.Parse(msgs[1]), 1f);
				kromidari[senderId].rigidbody2D.velocity = Vector2.zero;
				kromidari[senderId].rigidbody2D.AddForce(new Vector2(0, 45));
				break;
				
		//	raketa
		case "r": Rigidbody2D bulletInstance = Object.Instantiate(rocket, kromidari[senderId].position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			bulletInstance.velocity = new Vector2(20f, 0f);
			break;
			
		//	nov igrac ako se uklucil ja mu e momentalnata brzina u igrata
		case "f":
			Parallax.forwardSpeed=float.Parse(msgs[1]);
			break;
			
		//	on... udaril se on :D
		case "h": 			
			//kromidari[senderId].SendMessage("HitObstacle");				
			break;
			
		//	buffer prepreki
		case "b": 			
			string [] tmpPrepreki = msgs[1].Split('|');
			foreach (string preprekata in tmpPrepreki)
			{
				if (preprekata != "")
				PreprekaSpawn.preprekiteCache.Add("s^"+preprekata);
			}
			break;
		}
	}
	
	public void OnLeftRoom() {
		inRoom=false;
		// display error message and go back to the menu screen
		
		// (do NOT call PlayGamesPlatform.Instance.RealTime.LeaveRoom() here --
		// you have already left the room!)
		GlavnaSkripta.myGuiStat = guiStatus.inMenu;
		Application.LoadLevel("MainLevel");
	}
	
	public void OnPeersConnected(string[] participantIds) {
		// react appropriately (e.g. add new avatars to the game)
		/*for (int i = 0; i<participantIds.GetLength(0); i++) {
		
			if(participantIds[i] == myself.ParticipantId) continue;
			
			if (myself.ParticipantId == participants[0].ParticipantId && startingUp) {
				string buffer = "";
				foreach (string preprekaTmp in PreprekaSpawn.preprekiteCache) {
					buffer = buffer+preprekaTmp;
				}
				SendMessage("b^"+buffer,true);
				SendMessage("f^"+Parallax.forwardSpeed, true);
				SendMessage("p^"+startTime, true);
				
				Debug.Log("************** Added new participant ID " + participantIds[i]);
				
				Transform tmp = Object.Instantiate (kromidPrefab, new Vector3(-6.35f, 3.47f- 2.0f*i, 1f), Quaternion.identity) as Transform ;
				tmp.parent = Camera.main.transform;
				kromidari.Add(participantIds[i], tmp );
			}
			
		}*/
	}
	
	public void OnPeersDisconnected(string[] participantIds) {
		// react appropriately (e.g. remove avatars from the game)
		for (int i = 0; i<participantIds.GetLength(0); i++) {
			Object.Destroy(kromidari[participantIds[i]]);
			kromidari.Remove(participantIds[i]);		
			
			if (participants[0].ParticipantId == participantIds[i])
			{
				// the main guy has left QUIT game
				GlavnaSkripta.myGuiStat = guiStatus.inMultiplayer;		  
				status="The Host has left the room.\nExiting multiplayer...";  
				progres=0f;
				
				MultiplayerControler.LeaveRoom();
			}
			for (int j=0; i<participants.Count; i++)
			{
				if (participants[j].ParticipantId == participantIds[i])
				{
					participants.RemoveAt(j);
					break;
				}
			}
			
		//	Debug.Log("************** REMOVED participant ID " + participantIds[i]);
		}
	}
}
