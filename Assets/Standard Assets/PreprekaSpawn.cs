using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreprekaSpawn : MonoBehaviour {	
	
	public static PreprekaSpawn instance;
	
	public Transform[] prepreki;
	public Transform poenTriger;
	public Sprite[] kocki;
	
	private float vremeSec=3f;
	float timerSec;
	
	private float offset=0f;
	
	private Transform Dole;
	private Transform Gore;
	
	private Vector3 boundsGore; 
	private Vector3 boundsDole;
	
	public static List<string> preprekiteCache = new List<string>();
	
	//bool waitUnpause=false;
	public bool asClient=false;
	//bool waitUnpauseMulti=false;	
	
	bool sentStartMsg=false;
	float timeDifference=0f;
	bool pustamNova=false;
	
	public static int TimeCountDown;
	
	void  Start (){
		Gore=transform.GetChild(0);
		Dole=transform.GetChild(1);
	//	sentStartMsg=false;

		if (!asClient) {
			createObstacleBuffer(15);
			spawnObstacles ();
		}
		instance = this;
	}
	
	void TimerCountDown()
	{		
		TimeCountDown--;
		
		if (TimeCountDown <= 0)
			EndMultiGame();
		else Invoke("TimerCountDown", 1f);		
	}
	
	public void EeePauziraSe()
	{
		//Debug.Log("*** CEKAM DA SE ODSTOPIRA TRIGER");
		timeDifference = timerSec-(Time.time-timeDifference);
		pustamNova=false;
		//waitUnpauseMulti=true;
	}
	
	public void OdpauziraSe()
	{
	//	Debug.Log("*** POCEKA "+(timeDifference));
		Invoke ("pocekajIPusti", timeDifference);
	}
	
	void pocekajIPusti()
	{
		pustamNova=true;
		if( GlavnaSkripta.myGameType != gameType.Multi) spawnObstacles();
		else proveriCache();
	}
	
	public void createObstacleBuffer()
	{
		createObstacleBuffer(10);
	}
	
	public void EndMultiGame()
	{		
		MultiplayerControler.SendMessage("q^"+GlavnaSkripta.Poeni, true);
		GlavnaSkripta.myGuiStat = guiStatus.inMultiplayer;		
		GlavnaSkripta.isOver=true;
		GlavnaSkripta.isPaused=true;  pustamNova=false; preprekiteCache.Clear();
		//MultiplayerControler.status="Game Ended\nWaiting results...";  		
	}
	
	public void startAfter(float startZaSek)
	{
		//if (startZaSek <= 1)
		Invoke("StartMultiGame", startZaSek-.5f);
	}
	
	public void createObstacleBuffer(int Size)
	{
		if (GlavnaSkripta.isOver) return;
		float timerSec1 = vremeSec-(Parallax.forwardSpeed/4.8f);
		if (timerSec1 < 0.5f) timerSec1=0.5f;
		
		if (preprekiteCache.Count>14) {
			//Debug.Log("Too many obsatcles in buffer skiping..."+preprekiteCache.Count);
			Invoke ("createObstacleBuffer", timerSec1*8);
			return;
		}
		//Debug.Log("Creating buffer size: "+Size);
		//Debug.Log("Prepreki count: "+preprekiteCache.Count);
		string buffer="";
		for (int i=0; i<Size; i++)
		{
		//	Debug.Log("*** Kreiram formi");
			//	randomiziraj formi
			int randomPreprekaDole= Random.Range(0, prepreki.Length-1);
			int randomPreprekaGore= 0;
			if (randomPreprekaDole < 5) {
				//	3+7
				randomPreprekaGore = Random.Range(25, prepreki.Length-1);
			} else if (randomPreprekaDole < 10) {
				//	4+6
				randomPreprekaGore = Random.Range(16, 24);
			} else if (randomPreprekaDole < 16 && randomPreprekaDole > 9)  {
				// 5+5
				randomPreprekaGore = Random.Range(10, 15);
			} else if (randomPreprekaDole < 25) {
				//	6+4
				randomPreprekaGore = Random.Range(5, 9);
			} else  {
				// 7+3
				randomPreprekaGore = Random.Range(0, 4);
			} 
		//	Debug.Log("*** formi ok *** kreiram pozicii");
			offset = 1.6f;
			if (Gore == null) { Gore=transform.GetChild(0); }
			if (Dole == null) Dole=transform.GetChild(1);
			
			Vector3 gorePos = new Vector3(Gore.position.x+offset, Gore.position.y,Gore.position.z);
			Vector3 dolePos = Dole.position;
		//	Debug.Log("*** pozicii ok *** dodavam cache");
			
			preprekiteCache.Add("s^"+gorePos + "&" + dolePos+"&"+randomPreprekaGore+"&"+randomPreprekaDole);
			buffer=buffer+gorePos + "&" + dolePos+"&"+randomPreprekaGore+"&"+randomPreprekaDole+"|";
		//	Debug.Log("*** cache ok");
		}
		if(GlavnaSkripta.myGameType == gameType.Multi && MultiplayerControler.myself.ParticipantId == MultiplayerControler.participants[0].ParticipantId) {
		//	Debug.Log("Prakam buffer: "+buffer);
			MultiplayerControler.SendMessage("b^"+buffer,true);
			if (!sentStartMsg) {
				sentStartMsg=true;
				MultiplayerControler.SendMessage("p^5",true);
				MultiplayerControler.status="Starting game in 5 seconds"; 
			//	Debug.Log("Prakam start game!");
				Invoke("StartMultiGame", 5);
			}
		}
		
	//	Debug.Log("Done! Created buffer size: "+preprekiteCache.Count);
		
		Invoke ("createObstacleBuffer", timerSec1*8);
	}
	
	private void StartMultiGame()
	{		
		MultiplayerControler.instantiateOthers();
		
		GlavnaSkripta.myGuiStat = guiStatus.Idle;		
		GlavnaSkripta.isPaused=false;
		GameObject.FindWithTag("GameController").GetComponent<GlavnaSkripta>().resetGui();
		
		Parallax.forwardSpeed=4;
		
		Camera.main.transform.position.Set(0f,0f,-10f);
		
		//if (MultiplayerControler.myself.ParticipantId == MultiplayerControler.participants[0].ParticipantId)
		waitNSpawn();
		
		GlavnaSkripta.BtnClickSnd();
		
		TimeCountDown=60;
		Invoke("TimerCountDown", 1f);
	}
	
	void  spawnObstacles ()
	{
		if (asClient) return;
		
		// za single player cekaj ako se e udaril
		if (GlavnaSkripta.isPaused || GlavnaSkripta.isOver ){
			return;
		}		
		pustamNova=true;
		proveriCache();	
		
		waitNSpawn();
	}
	
	void  waitNSpawn (){	
		if (GlavnaSkripta.isPaused && asClient){
		//	Debug.Log("*** CEKAM DA SE ODSTOPIRA WNS");
			//timeDifference = Time.time;
			//waitUnpauseMulti=true;
			return;
		}	
		pustamNova=true;
		timeDifference = Time.time;
		
		timerSec = vremeSec-(Parallax.forwardSpeed/4.8f);
		if (timerSec < 0.5f) timerSec=0.5f;
		//Debug.Log("^^^^^^^ TIMER SPAWN: "+timerSec+ " SPEED:" +Parallax.forwardSpeed+ " asClient:" +asClient+ " isPaused:" +GlavnaSkripta.isPaused);
		if (asClient) Invoke ("proveriCache", timerSec);
		else Invoke ("spawnObstacles", timerSec);
		
		//spawnObstacles();
	}
	
	public bool proveriCache()
	{
		//	za multiplayer pusti od kesiranite tasacinja kako prepreki
		if (!GlavnaSkripta.isPaused && preprekiteCache.Count>0 && pustamNova) {
		//	Debug.Log("^^^^^^^^^^ vadam kesirani i ne se zamaram: "+boundsGore + ":" + boundsDole + " time: "+Time.time);
			// veke ima kesirano koristi gi tie
			//Debug.Log("Prepreki count: "+preprekiteCache.Count);
			string tmpMsg = preprekiteCache[0];
			string [] tmps = preprekiteCache[0].Split('^');
			
			string [] msgs = tmps[1].Split('&');
			preprekiteCache.RemoveAt(0);
		//	Debug.Log("Prepreki count: "+preprekiteCache.Count);
			string[] pos = msgs[0].Split(',');
			
		//	Debug.Log(tmps[0]+":"+tmps[1]);
			
			Vector3 gorePos1 = new Vector3(float.Parse(pos[0].Substring(1)), float.Parse(pos[1]), float.Parse(pos[2].Replace(")","")));
			
			pos = msgs[1].Split(',');
			Vector3 dolePos1 = new Vector3(float.Parse(pos[0].Substring(1)), float.Parse(pos[1]), float.Parse(pos[2].Replace(")","")));
			
			//	proveri da ne udara taa gornata u nekoa pred nea
			if (boundsGore!=null && (gorePos1.x-boundsGore.x)<2)
			{
				gorePos1.x += 2f;
				dolePos1.x += 2f;
				//	Debug.Log("^^----^^ gore dodavam 2 "+boundsGore+" - "+gorePos);
			}			
			//	proveri da ne udara i taa dole
			if (boundsDole!=null && (dolePos1.x-boundsDole.x)<2)
			{
				dolePos1.x += 2f;
				gorePos1.x += 2f;
				//	Debug.Log("^----^ dole dodavam 2 "+boundsDole+" - "+dolePos);
			}
			//	zacuvaj prethodni prepreki
			boundsGore = gorePos1;
			boundsDole = dolePos1;
			
			Transform gameObject11 = Object.Instantiate(prepreki[int.Parse(msgs[2])], gorePos1, new Quaternion(0, 0, 180, 0) ) as Transform;
			Transform gameObject21 = Object.Instantiate(prepreki[int.Parse(msgs[3])], dolePos1, Quaternion.identity) as Transform;			
			
			//	daj im boicki
			for (int i=0; i<gameObject11.childCount; i++)
			{
				gameObject11.GetChild(i).GetComponent<SpriteRenderer>().sprite = kocki[Random.Range(0, kocki.GetLength(0)-1)];
				gameObject11.GetChild(i).collider2D.isTrigger = true;
			}
			for (int i=0; i<gameObject21.childCount; i++)
			{
				gameObject21.GetChild(i).GetComponent<SpriteRenderer>().sprite = kocki[Random.Range(0, kocki.GetLength(0)-1)];
				gameObject21.GetChild(i).collider2D.isTrigger = true;
			}
			PreprekiMoove pm = gameObject11.gameObject.AddComponent("PreprekiMoove") as PreprekiMoove; 
			pm.direction=1f;
			PreprekiMoove pm2 = gameObject21.gameObject.AddComponent("PreprekiMoove") as PreprekiMoove;	
			
			Transform poeno = Object.Instantiate(poenTriger, dolePos1, Quaternion.identity) as Transform;			
			PreprekiMoove pm3 = poeno.gameObject.AddComponent("PreprekiMoove") as PreprekiMoove; 
			
			
			//if (GlavnaSkripta.myGameType != gameType.Idle && preprekiteCache.Count<5) 
			pustamNova=false;
			if (asClient) waitNSpawn ();
			
			return true;
		}
		if (pustamNova) {
			pustamNova=false;
			if (asClient) waitNSpawn ();
		}
		
		return false;
	}
}

