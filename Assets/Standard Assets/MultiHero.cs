using UnityEngine;
using System.Collections;

public class MultiHero : MonoBehaviour {

	int moveDir=0;
	bool blinking=false;
	bool saveMeOn=false;
	int blinkTime=0;
	
	void  OnTriggerEnter2D ( Collider2D other  ){
		//return;
		if (other.gameObject.tag == "Obstacle" && !saveMeOn) {
			HitObstacle();
		}
	}
	
	void afterSave()
	{
		blinking=false;
		saveMeOn = false;
		
		SpriteRenderer[] mySprites;
		mySprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer go in mySprites ) {
			if (go.gameObject.name == "samija") continue;
			go.enabled = true;			
		}
	}

	public void HitObstacle()
	{
		blinking=true;
		saveMeOn = true;
		
		moveDir = -1;
		Invoke ("resetDir", 1);
		Invoke ("afterSave", 4);
	}
	
	public void iHit()
	{
		if (moveDir==0){	
			moveDir = 1;
			Invoke ("resetDir", 1);
			}
	}
	
	public void resetDir()
	{
		moveDir=0;
	}
	
	// Update is called once per frame
	void Update () {
		if (blinking) {
			if (blinkTime%7 == 0) {
				SpriteRenderer[] mySprites;
				mySprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
				foreach(SpriteRenderer go in mySprites ) {
					if (go.gameObject.name == "samija") continue;
					go.enabled = !go.enabled;
				}
			}
			blinkTime++;
		}
		
		if (GlavnaSkripta.isPaused && moveDir==-1) return;
		
		transform.Translate(moveDir*Time.deltaTime * Parallax.forwardSpeed, 0, 0);//((float.Parse(Parallax.averageFrameRate.ToString("f3"))-0.0015f)
	}
}
