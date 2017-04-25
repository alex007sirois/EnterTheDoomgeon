using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1F;
	public LayerMask blockingLayer;

	private BoxCollider2D box;
	private Rigidbody2D body;
	private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start () 
	{
		box = GetComponent<BoxCollider2D>();
		body = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1 / moveTime;
	}

	protected bool Move (float xDir, float yDir, out RaycastHit2D hit)
	{
		//Store start position to move from, based on objects current transform position.
		Vector2 start = transform.position;

		// Calculate end position based on the direction parameters passed in when calling Move.
		Vector2 end = start + new Vector2 (xDir, yDir);

		//Disable the boxCollider so that linecast doesn't hit this object's own collider.
		box.enabled = false;

		//Cast a line from start point to end point checking collision on blockingLayer.
		hit = Physics2D.Linecast (start, end, blockingLayer);

		//Re-enable boxCollider after linecast
		box.enabled = true;

		//Check if anything was hit
		if (hit.transform == null) {
			//If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
			Vector3 newPosition = Vector3.MoveTowards (body.position, end, 1);
			body.MovePosition (newPosition);
			//Return true to say that Move was successful
			return true;
		}
		return false;
	}

	protected virtual void AttemptMove <T> (float xDir, float yDir)
		where T : Component
	{
		

		//Hit will store whatever our linecast hits when Move is called.
		RaycastHit2D hit;

		//Set canMove to true if Move was successful, false if failed.
		bool canMove = Move (xDir, yDir, out hit);

		//Check if nothing was hit by linecast
		if(hit.transform == null)
			//If nothing was hit, return and don't execute further code.
			return;

		//Get a component reference to the component of type T attached to the object that was hit
		T hitComponent = hit.transform.GetComponent <T> ();

		//If canMove is false and hitComponent is not equal to null, meaning MovingObject is blocked and has hit something it can interact with.
		if(!canMove && hitComponent != null)

			//Call the OnCantMove function and pass it hitComponent as a parameter.
			OnCantMove (hitComponent);
	}

	protected IEnumerator SmoothMovement(Vector3 end)
	{
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon) 
		{
			Vector3 newPosition = Vector3.MoveTowards (body.position, end, inverseMoveTime * Time.deltaTime);
			body.MovePosition (newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		}
		yield return null;
	}
	
	protected abstract void OnCantMove <T> (T component)
		where T : Component;
}
