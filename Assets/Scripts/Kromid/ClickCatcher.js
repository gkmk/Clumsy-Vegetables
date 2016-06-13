/* Check out our new 2D point and click adventure at */
/* http://www.NotQuiteBlackandWhite.com */

/*
"FrontmostSpriteClicker" for Unity

Attaching this script to the game camera will allow any 2D sprite that has a collider attached to receive clicks.
It will ignore anything beneath the frontmost sprite i.e a sprite in a lower sorting layer or in a 
lower sorting order in the same layer.

This script requires that any sprite you want to be clickable has a script attached
that has a function called OnLeftClick, and one called OnRightClick.

NOTE 1: Works with 2D colliders only but more code could be added to take into account 3D objects and Z depth.
NOTE 2: No effort has been made to optimise this script, it can almost definitely be improved upon.
*/

#pragma strict

private var leftClickedObject : GameObject;
private var rightClickedObject : GameObject;
private var frontmostRaycastHit : RaycastHit2D;

// It's necessary to access the SpriteRenderer of a game object to be able to access its sorting layer ID 
// and sorting order ("Order in Layer" in the inspector)
private var spriteRenderer : SpriteRenderer;

function Update () 
{
	// If the left mouse button is clicked anywhere...
	if (Input.GetMouseButtonDown(0))
	{
		// frontmostRaycastHit stores information about the RaycastHit2D that is returned by GetFrontmostRaycastHit()
		frontmostRaycastHit = GetFrontmostRaycastHit();

		// If frontmostRaycastHit is true, i,e the user hasn't clicked on nothing, i.e GetFrontmostRaycastHit() didn't return nothing...
		if (frontmostRaycastHit)
		{
			// Assigns the game object that the collider that has been clicked on to leftClickedObject.
			leftClickedObject = frontmostRaycastHit.collider.gameObject;
			
			// Sends the frontmostRaycast to a function called OnLeftClick in a script attached to whatever the leftClickedObject is.
			leftClickedObject.SendMessage("OnLeftClick", frontmostRaycastHit, SendMessageOptions.DontRequireReceiver);
		}
	}
	// If the right mouse button is clicked anywhere...
	else if (Input.GetMouseButtonDown(1))
	{
		// frontmostRaycastHit stores information about the RaycastHit2D that is returned by GetFrontmostRaycastHit()
		frontmostRaycastHit = GetFrontmostRaycastHit();

		// If frontmostRaycastHit is true, i,e the user hasn't clicked on nothing, i.e GetFrontmostRaycastHit() didn't return nothing...
		if (frontmostRaycastHit)
		{
			// Assigns the game object that the collider that has been clicked on to rightClickedObject.
			rightClickedObject = frontmostRaycastHit.collider.gameObject;
			
			// Sends the frontmostRaycast to a function called OnLeftClick in a script attached to whatever the rightClickedObject is.
			rightClickedObject.SendMessage("OnRightClick", frontmostRaycastHit, SendMessageOptions.DontRequireReceiver);
		}
	}
}

function GetFrontmostRaycastHit(): RaycastHit2D
{
	// Store the point where the user has clicked as a Vector3.
	var clickPosition : Vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	// Retrieve all raycast hits from the click position (to the same click position - more a dot than a ray) and store them in an array called "hits".
	var hits : RaycastHit2D[] = Physics2D.LinecastAll (clickPosition, clickPosition);

	// If the raycast hits something...
	if (hits.length != 0)
	{
		// A variable that will store the frontmost sorting layer that contains an object that has been clicked on as an int.
		var topSortingLayer : int = 0;
		// A variable that will store the index of the top sorting layer as an int.
		var indexOfTopSortingLayer : int;
		// An array that stores the IDs of all the sorting layers that contain a sprite in the path of the linecast.
		var sortingLayerIDArray : int[] = new int[hits.length];
		// An array that stores the sorting orders of each sprite that has been hit by the linecast
		var sortingOrderArray : int[] = new int[hits.length];
		// An array that stores the sorting order number of the frontmost sprite that has been clicked.
		var topSortingOrder : int = 0;
		// A variable that will store the index in the sortingOrderArray where topSortingOrder is. This index used with the hits array will give us our frontmost clicked sprite.
		var indexOfTopSortingOrder : int;

		// Loop through the array of raycast hits...
		for (var i = 0; i < hits.length; i++)
		{
			// Get the SpriteRenderer from each game object under the click.
			spriteRenderer = hits[i].collider.gameObject.GetComponent(SpriteRenderer);
		
			if (spriteRenderer)
			{
			// Access the sortingLayerID through the SpriteRenderer and store it in the sortingLayerIDArray.
			sortingLayerIDArray[i] = spriteRenderer.sortingLayerID;
			
			// Access the sortingOrder through the SpriteRenderer and store it in the sortingOrderArray.
			sortingOrderArray[i] = spriteRenderer.sortingOrder;
			}
		}
		
		// Loop through the array of sprite sorting layer IDs...
		for (var j = 0; j < sortingLayerIDArray.length; j++)
		{
			// If the sortingLayerID is higher that the topSortingLayer...
			if (sortingLayerIDArray[j] >= topSortingLayer)
			{
				topSortingLayer = sortingLayerIDArray[j];
				indexOfTopSortingLayer = j;
			}
		}

		// Loop through the array of sprite sorting orders...
		for (var k = 0; k < sortingOrderArray.length; k++)
		{
			// If the sorting order of the sprite is higher than topSortingOrder AND the sprite is on the top sorting layer...
			if (sortingOrderArray[k] >= topSortingOrder && sortingLayerIDArray[k] == topSortingLayer)
			{
				topSortingOrder = sortingOrderArray[k];
				indexOfTopSortingOrder = k;
			}
			else
			{
				// Do nothing and continue loop.
			}
		}

		// How many sprites with colliders attached are underneath the click?
		/*Debug.Log("How many sprites have been clicked on: " + hits.length);
		
		// Which is the sorting layer of the frontmost clicked sprite?
		Debug.Log("Frontmost sorting layer ID: "+ topSortingLayer);
		
		// Which is the order in that sorting layer of the frontmost clicked sprite?
		Debug.Log("Frontmost order in layer: "+ topSortingOrder);*/

		// The indexOfTopSortingOrder will also be the index of the frontmost raycast hit in the array "hits". 
		return hits[indexOfTopSortingOrder];
	}
	else // If the hits array has a length of 0 then nothing has been clicked...
	{
		//Debug.Log("Nothing clicked.");
		return;
	}
}