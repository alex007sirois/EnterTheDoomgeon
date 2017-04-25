using UnityEngine;
using System.Collections;

public class GunBullet : MonoBehaviour {

	private float x=0;
	private float y=0;
	private float speed=40;
	public LayerMask blockingLayer;
	private CircleCollider2D box;
	private Rigidbody2D body;

	private float depart;

	// Use this for initialization
	private void Awake()
	{
		box = GetComponent<CircleCollider2D>();
		body = GetComponent<Rigidbody2D>();
	}

	public bool Shoot (Vector3 spawn, int x, int y)
	{

		this.x = x;
		this.y = y;

		RaycastHit2D hit;

		//Store start position to move from, based on objects current transform position.
				// Calculate end position based on the direction parameters passed in when calling Move.
		Vector3 end = spawn + new Vector3 (0.64F*x, 0.64F*y);

		//Disable the boxCollider so that linecast doesn't hit this object's own collider.
		box.enabled = false;

		//Cast a line from start point to end point checking collision on blockingLayer.
		hit = Physics2D.Linecast (end, end, blockingLayer);

		//Re-enable boxCollider after linecast
		box.enabled = true;

		if (hit.transform == null) {
			GetComponent<AudioSource> ().Play ();
			gameObject.transform.position = spawn + new Vector3 (0.64F * x, 0.64F * y, 0F);

			depart = Time.fixedTime;
			body.velocity= new Vector2 (speed*x, speed*y);
			return true;
		}
		return false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (x == 0 && y == 0)
			return;
		else
			if ((Time.fixedTime - depart) > 0.25F) {
				x = 0;
				y = 0;
				body.velocity= new Vector2 (speed*x, speed*y);
			}
		
	}
}
