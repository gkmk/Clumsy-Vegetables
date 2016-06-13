#pragma strict

var rocket:Rigidbody2D;				// Prefab of the rocket.
var speed:float = 20f;				// The speed the rocket will fire at.
var kromido:Transform;

function OnLeftClick()
{
 if (GlavnaSkripta.ImaRaketi && !GlavnaSkripta.isPaused && GlavnaSkripta.myGuiStat != guiStatus.newGameOn)
 	{
	 	if (kromido)
	 	{
		 	if (GlavnaSkripta.myGameType == gameType.Multi) {
				MultiplayerControler.SendMessage("r");
			}
			var bulletInstance:Rigidbody2D = Instantiate(rocket, kromido.position, Quaternion.Euler(new Vector3(0,0,0)));
			bulletInstance.velocity = Vector2(speed, 0);

			GlavnaSkripta.ImaRaketi--;
			
			if (GlavnaSkripta.myGameType != gameType.Multi)
			{
			PlayerPrefs.SetInt ("OveralRockets", PlayerPrefs.GetInt ("OveralRockets")+1);
			Soomla.StoreInventory.TakeItem(Soomla.CVStore.ROCKET_ITEM_ID, 1);
			}
			//PlayerPrefs.SetInt("Player Rockets", GlavnaSkripta.ImaRaketi);
		}
	}
}
