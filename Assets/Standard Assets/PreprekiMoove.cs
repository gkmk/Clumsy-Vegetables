using UnityEngine;
using System.Collections;

public class PreprekiMoove : MonoBehaviour {

	public float direction=-1f;

	/*public float timeWait=0f;
	
	float moveTimeSync=0f;*/

	void Update()
	{
		if (GlavnaSkripta.isPaused) return;
		
		if (GlavnaSkripta.myGameType == gameType.Idle) return;
		
		//	Sync corrections
		/*if (timeWait > 0f) {
			float curTime = Time.time;
			moveTimeSync = curTime+( curTime - timeWait);
			Debug.Log("Resync time : "+moveTimeSync+" : " +curTime);
			timeWait = 0f;
		}
		if (moveTimeSync > Time.time) return;*/
		
		transform.Translate(direction*Time.deltaTime * Parallax.forwardSpeed, 0, 0);//((float.Parse(Parallax.averageFrameRate.ToString("f3"))-0.0015f)
	}
}
