#pragma strict

var jumpForce:float=7f;
private var startY:float=1.63;
private var startX:float;
private var addX:float;
var gs:GlavnaSkripta;
var samijata:SpriteRenderer;
var blinking=false;
var blinkTime:int=0;

function Start () {
	
/*	if (GlavnaSkripta.myGuiStat == guiStatus.inMenu)
	{		
		animation.Play("tasak");
		animation.PlayQueued("tasak2");
	}
	else animation.Play("tasak2");*/
	
	transform.position.y=startY;
	startX=transform.position.x;
	gs = GameObject.FindWithTag("GameController").GetComponent(GlavnaSkripta);

	if (Soomla.StoreInventory.IsVirtualGoodEquipped(Soomla.CVStore.SAMIJA_ITEM_ID))
	{
		samijata.enabled=true;
	}
}

function MultiplayerHit()
{
blinking=true;
GlavnaSkripta.isPaused = true;

yield new WaitForSeconds(1);

GlavnaSkripta.isPaused = false;

yield new WaitForSeconds(2);
blinking=true;
GlavnaSkripta.saveMeOn = true;

var mySprites : SpriteRenderer[];
mySprites = gameObject.GetComponentsInChildren(SpriteRenderer);
for (var go:SpriteRenderer in mySprites ) {
		go.enabled = true;
	}
}

function OnCollisionEnter2D ( col: Collision2D ) 
{
	if (GlavnaSkripta.myGameType == gameType.Multi) { MultiplayerHit(); return; }
	
	if (GlavnaSkripta.myGameType == gameType.Idle) return;

	if (GlavnaSkripta.myGuiStat == guiStatus.inMenu) return;
	
	if (GlavnaSkripta.myGuiStat == guiStatus.newGameOn) return;

	if (col.gameObject.tag == "ground") {
		//gs.gameOver();
	}
}

function OnTriggerEnter2D(other: Collider2D) 
{
	if (GlavnaSkripta.myGameType == gameType.Multi) { MultiplayerHit(); return; }
	
	if (GlavnaSkripta.myGameType == gameType.Idle) return;
	
	if (GlavnaSkripta.myGuiStat == guiStatus.inMenu) return;
	
	if (GlavnaSkripta.myGuiStat == guiStatus.newGameOn) return;

	if (other.gameObject.tag == "Crate") {
		GlavnaSkripta.Poeni++;
		PlayerPrefs.SetInt ("OveralPoeni", PlayerPrefs.GetInt ("OveralPoeni")+1);
		Destroy(other.gameObject);
	}
	else if (other.gameObject.tag == "Obstacle" && !GlavnaSkripta.saveMeOn) {
		gs.gameOver();
	}
}

function Update () {

if (blinking) {
	if (blinkTime%100 == 0) {
		var mySprites : SpriteRenderer[];
		mySprites = gameObject.GetComponentsInChildren(SpriteRenderer);
		for (var go:SpriteRenderer in mySprites ) {
				go.enabled = !go.enabled;
			}
	}
}

if (GlavnaSkripta.myGuiStat == guiStatus.newGameOn) transform.position.y=0;

transform.position.x = Camera.main.transform.position.x+startX; 
	if ((Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && 
		  Input.GetTouch(0).phase == TouchPhase.Began)) && !GlavnaSkripta.isPaused && 
		  (GlavnaSkripta.myGameType != gameType.Idle) ) {

		if (GlavnaSkripta.myGuiStat == guiStatus.newGameOn) {
			GameObject.FindWithTag("Respawn").GetComponent(PreprekaSpawn).enabled = true;
			GameObject.FindWithTag("Player").animation.Play("tasak3");
			
			GlavnaSkripta.myGuiStat = guiStatus.Idle;
		}
		else 
		{		
			/*addX=0;
			var novKurac = transform.position.x - Camera.main.transform.position.x;
			  if (novKurac < startX)  {
			  	addX=Mathf.Abs((startX-novKurac)+1)*4;
			  }		 */		 			 
			
			gameObject.audio.Play();
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.AddForce(new Vector2(addX, jumpForce));
		}
		
		if (GlavnaSkripta.myGameType == gameType.Multi) {
			MultiplayerControler.SendMessage("j");
		}
	}
}