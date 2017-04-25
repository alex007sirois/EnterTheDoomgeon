using UnityEngine;
using System.Collections;

public class Zombie : MovingObject, Ennemy {

	private Animator animator;
	private Transform target;
	private bool skipMove;

	private int x=0;
	private int y=0;

	private int rotation=0;

	// Use this for initialization
	protected override void Start () 
	{
		animator = GetComponent<Animator> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		GameManager.instance.AddEnnemyToList (this);

		transform.Rotate(Vector3.forward*90);

		base.Start ();
	}

	protected override void AttemptMove <T> (float xDir, float yDir)
	{
		if (skipMove)
		{
			skipMove = false;
			return;
		}
		animator.SetTrigger ("move");
		base.AttemptMove <T> (xDir, yDir);

		skipMove = true;
	}

	public void MoveEnnemy ()
	{
		
		float xDir = 0;
		float yDir = 0;

		if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)

			//If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
			yDir = target.position.y > transform.position.y ? 1 : -1;

		//If the difference in positions is not approximately zero (Epsilon) do the following:
		else
			//Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
			xDir = target.position.x > transform.position.x ? 1 : -1;

		x = (int)xDir;
		y = (int)yDir;
		Turn ();

		//Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player

		AttemptMove <Player> ((float)0.64*x, (float)0.64*y);
			
	}

	protected override void OnCantMove<T> (T component)
	{
		GetComponents<AudioSource> ()[0].Play ();
		Player hitPlayer = component as Player;

		//Call the LoseFood function of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted.
		hitPlayer.HitPlayer();

		//Set the attack trigger of animator to trigger Enemy attack animation.
		animator.SetTrigger ("attack");
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == null)
			return;
		if (other.tag == "GunBullet") {
			other.transform.position = new Vector3 (-10, -10, 0);
			GameManager.instance.skull++;
			Destroy ();
		}
			
	}

	void OnDestroy() {
		GameManager.instance.ennemies.Remove (this);
	}

	void Turn()
	{
		transform.Rotate(Vector3.back*rotation);

		if (x == 1 && y==0)
			rotation = 0;

		if (x == -1 && y==0)
			rotation = 180;

		if (y == 1 && x==0)
			rotation = 90;

		if (y == -1 && x==0)
			rotation = 270;

		transform.Rotate(Vector3.forward*rotation);
	}


	void Destroy()
	{
		Destroy (this.gameObject);
	}
}
