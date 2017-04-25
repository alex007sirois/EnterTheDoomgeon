using UnityEngine;
using System.Collections;

public class Sentry : MonoBehaviour, Ennemy{

	private byte position=0;

	private int x=0;
	private int y=1;
	public GameObject bullet;
	private GameObject instance;
	private bool destroyed=false;

	Animator animator;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator> ();
		GameManager.instance.AddEnnemyToList (this);
		instance =Instantiate (bullet, new Vector3 (-10, -10, 0), Quaternion.identity) as GameObject;
	}

	public void MoveEnnemy ()
	{
		if(!destroyed)
		{
		transform.Rotate(Vector3.forward*90);

		animator.SetTrigger ("fire");

		position++;

		if (position == 4)
			position = 0;

			switch (position) {
			case 0:
				x = 0;
				y = 1;
				break;
			case 1:
				x = -1;
				y = 0;
				break;
			case 2:
				x = 0;
				y = -1;
				break;
			case 3:
				x = 1;
				y = 0;
				break;
			}
		

		instance.GetComponent<GunBullet> ().Shoot(transform.position, this.x, this.y);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy() {
		GameManager.instance.ennemies.Remove (this);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == null)
			return;
		if (other.tag == "GunBullet") {
			destroyed = true;
			animator.SetTrigger ("boom");
			GetComponent<AudioSource> ().Play ();
			GameManager.instance.skull++;
			other.transform.position = new Vector3 (-10, -10, 0);

			Invoke ("Destroy", 1F);
		}
	}

	void Destroy()
	{
		Destroy (this.gameObject);
	}
}
