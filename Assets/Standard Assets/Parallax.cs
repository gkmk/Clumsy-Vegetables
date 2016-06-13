// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {	
	
	public Transform[] Backgrounds;
	
	//static float parallaxReductionFactor=2f;   // How much less each successive layer should parallax.
	
	public static float forwardSpeed=4f;
//	public static float averageFrameRate;	
//	static float lastFrameRate=0f;
//	static int frames=1;
	
	/*  void  Start (){
       // The 'previous frame' had the current frame's camera position.
//       previousCamPos = transform.position.x;
    }*/
	
	void  Update (){		
	//	lastFrameRate+=Time.deltaTime;
	//	averageFrameRate=lastFrameRate/frames;
	//	frames++;
		
		if (GlavnaSkripta.isPaused) return;
		
		//if (GlavnaSkripta.myGameType != gameType.Idle)
		//	transform.Translate((float.Parse(Parallax.averageFrameRate.ToString("f3"))-0.0015f) * Parallax.forwardSpeed, 0, 0);
		
		float bak1 = Backgrounds[0].renderer.material.mainTextureOffset.x+(Time.deltaTime * forwardSpeed/20);
		float bak2 = Backgrounds[1].renderer.material.mainTextureOffset.x+(Time.deltaTime * forwardSpeed/30);
		
		bak1 = bak1%1;bak2 = bak2%1;
		
		Backgrounds[0].renderer.material.mainTextureOffset = new Vector2(bak1, 0f);
		Backgrounds[1].renderer.material.mainTextureOffset = new Vector2(bak2, 0f);	
		
		
		if (GlavnaSkripta.myGameType != gameType.Idle)	forwardSpeed+=0.0008f;
		if (forwardSpeed>10f) forwardSpeed=10f;
	}
}