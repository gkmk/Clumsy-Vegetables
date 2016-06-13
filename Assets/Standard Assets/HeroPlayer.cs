// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class HeroPlayer : MonoBehaviour {
	
	public float jumpForce=45f;
	private float startY=3.47f;
	private float startX=-6.28f;
	//private float addX;
	GlavnaSkripta gs;
	public SpriteRenderer samijata;
	bool blinking=false;
	int blinkTime=0;
	
	bool hasSamija;
	
	void  Start (){
		
		if (GlavnaSkripta.myGuiStat == guiStatus.inMenu)
		{		
			animation.Play("tasak");
			animation.PlayQueued("tasak2");
		}
		else animation.Play("tasak2");
		
		//transform.position = new Vector3(startX, startY, 2f);
		//startX=transform.position.x;
		gs = GameObject.FindWithTag("GameController").GetComponent<GlavnaSkripta>();
		
		hasSamija = Soomla.StoreInventory.IsVirtualGoodEquipped(Soomla.CVStore.SAMIJA_ITEM_ID);
		
		
		if (hasSamija)
		{
			samijata.enabled=true;
		}
	}
	
	void unPause()
	{
		if (!GlavnaSkripta.isOver)
		{
		GlavnaSkripta.isPaused = false;
		PreprekaSpawn.instance.OdpauziraSe();
		}
	}
	
	void afterSave()
	{
		blinking=false;
		GlavnaSkripta.saveMeOn = false;
		
		SpriteRenderer[] mySprites;
		mySprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer go in mySprites ) {
			if (go == samijata && !hasSamija) continue;
			go.enabled = true;
			
		}
	}
	
	public void  MultiplayerHit (){
		//MultiplayerControler.SendMessage("h", true);
		if (GlavnaSkripta.myGameType == gameType.Multi)
			PreprekaSpawn.instance.EeePauziraSe();
		
		foreach (GameObject gos in GameObject.FindGameObjectsWithTag("Enemy")) {
			gos.SendMessage("iHit");			
		}
		
		
		blinking=true;
		if (GlavnaSkripta.myGameType == gameType.Multi)	GlavnaSkripta.isPaused = true;
		GlavnaSkripta.saveMeOn = true;
		
		if (GlavnaSkripta.myGameType == gameType.Multi)
		Invoke ("unPause", 1);
		
		Invoke ("afterSave", 4);		
	}
	
	void  OnCollisionEnter2D (  Collision2D col   ){
	//return;
		if (GlavnaSkripta.saveMeOn) return;
		
		if (GlavnaSkripta.myGameType == gameType.Multi) { return; }
		
		if (GlavnaSkripta.myGameType == gameType.Idle) return;
		
		if (GlavnaSkripta.myGuiStat == guiStatus.inMenu) return;
		
		if (GlavnaSkripta.myGuiStat == guiStatus.newGameOn) return;
		
		if (col.gameObject.tag == "ground") {
		//	PreprekaSpawn.instance.EeePauziraSe();
			gs.gameOver();
		}
	}
	
	void  OnTriggerEnter2D ( Collider2D other  ){
	//return;
		if (GlavnaSkripta.saveMeOn) return;
	
		if (GlavnaSkripta.myGameType == gameType.Multi) { 
			if (other.gameObject.tag == "Crate") {
				GlavnaSkripta.Poeni++;
				Destroy(other.gameObject);
			}
			else if (other.gameObject.tag == "Obstacle" && !GlavnaSkripta.saveMeOn) {
				MultiplayerHit();
			}
			return; 
		}
		
		if (GlavnaSkripta.myGameType == gameType.Idle) return;
		
		if (GlavnaSkripta.myGuiStat == guiStatus.inMenu) return;
		
		if (GlavnaSkripta.myGuiStat == guiStatus.newGameOn) return;
		
		if (other.gameObject.tag == "Crate") {
			GlavnaSkripta.Poeni++;
			PlayerPrefs.SetInt ("OveralPoeni", PlayerPrefs.GetInt ("OveralPoeni")+1);
			Destroy(other.gameObject);
		}
		else if (other.gameObject.tag == "Obstacle" && !GlavnaSkripta.saveMeOn) {
			//PreprekaSpawn.instance.EeePauziraSe();
			gs.gameOver();
		}
	}
	
	void  Update (){
		if (blinking) {
			if (blinkTime%5 == 0) {
				SpriteRenderer[] mySprites;
				mySprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
				foreach(SpriteRenderer go in mySprites ) {
					if (go == samijata && !hasSamija) continue;
					go.enabled = !go.enabled;
				}
			}
			blinkTime++;
		}
		
		if (GlavnaSkripta.myGuiStat == guiStatus.newGameOn) transform.position = new Vector3(startX, startY, 2f);
		
		transform.position = new Vector3( startX, transform.position.y, 2f); 
		
		if ((Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && 
		                                     Input.GetTouch(0).phase == TouchPhase.Began)) && !GlavnaSkripta.isPaused && 
		    (GlavnaSkripta.myGameType != gameType.Idle) ) {
			
			if (GlavnaSkripta.myGuiStat == guiStatus.newGameOn) {
				GameObject.FindWithTag("Respawn").GetComponent<PreprekaSpawn>().enabled = true;
				GameObject.FindWithTag("Player").animation.Play("tasak3");
				
				GlavnaSkripta.myGuiStat = guiStatus.Idle;
			}
			else 
			{		
				/*addX=0;
			FIXME_VAR_TYPE novKurac= transform.position.x - Camera.main.transform.position.x;
			  if (novKurac < startX)  {
			  	addX=Mathf.Abs((startX-novKurac)+1)*4;
			  }		 */		 			 
				
				gameObject.audio.Play();
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			}
			
			if (GlavnaSkripta.myGameType == gameType.Multi) {
				MultiplayerControler.SendMessage("j^"+transform.position.y);
			}
		}
	}
}