using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

	private Animator animator;

	public Text heartText;
	public Text bulletText;

	private byte heart;
	private byte bullet;

	private bool lava=false;

	private int x=0;
	private int y=-1;

	// Use this for initialization
	protected override void Start () {
		heart = GameManager.instance.heart;
		bullet = GameManager.instance.bullet;

		animator = GetComponent<Animator> ();

		base.Start();
	}

	private void OnDisable()
	{
		GameManager.instance.heart = this.heart;
		GameManager.instance.bullet = this.bullet;
	}

	// Update is called once per frame
	void Update () 
	{

		if (heart == 0)
			GameOver ();

		if (Input.GetKeyDown (KeyCode.Space))
			Shoot ();

		heartText.text = "x" + heart;
		bulletText.text = "x" + bullet;

		float moveH=0F;
		float moveV=0F;

		animator.SetBool("move", false);

		if (GameManager.instance.playersTurn) {
			bool moved=false;

			if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
				moveV++;
				moved = true;
				animator.SetFloat ("moveX", 0);
				animator.SetFloat ("moveY", 1);
				animator.SetBool ("move", true);
			} else if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
				moveV--;
				moved=true;
				animator.SetFloat ("moveX", 0);
				animator.SetFloat ("moveY", -1);
				animator.SetBool ("move", true);
			} else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
				moveH++;
				moved=true;
				animator.SetFloat ("moveX", 1);
				animator.SetFloat ("moveY", 0);
				animator.SetBool ("move", true);
			} else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
				moveH--;
				moved=true;
				animator.SetFloat ("moveX", -1);
				animator.SetFloat ("moveY", 0);
				animator.SetBool ("move", true);
			}

			if (moved) {
				this.x = (int)moveH;
				this.y = (int)moveV;
				AttemptMove <Sentry> ((float)0.64 *x, (float)0.64 *y);
			}

		}
	}

	void Shoot()
	{
		if (GameManager.instance.playersTurn && bullet > 0) 
		{
			GameManager.instance.playersTurn = false;

			GameObject balle = GameObject.FindGameObjectsWithTag ("GunBullet") [0];

			if(balle.GetComponent<GunBullet> ().Shoot(transform.position, this.x, this.y))
				bullet--;
		}
	}

	protected override void OnCantMove <T> (T Component)
	{
		
	}

	void GameOver()
	{
		GameManager.instance.GameOver ();
	}

	protected override void AttemptMove<T> (float xDir, float yDir)
	{
		base.AttemptMove<T>(xDir, yDir);
		GameManager.instance.playersTurn = false;
	}

	public void HitPlayer()
	{
		GetComponent<AudioSource> ().Play ();
		this.heart--;
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.tag == null)
			return;
		if (other.tag == "Heart")
		{
			this.heart++;
			other.gameObject.SetActive (false);
		}
		if (other.tag == "Bullet")
		{
			this.bullet+=2;
			other.gameObject.SetActive (false);
		}
		if (other.tag == "GunBullet")
		{
			HitPlayer ();
			other.transform.position = new Vector3 (-10, -10, 0);
		}
		if (other.tag == "Lava")
		{
			if(!lava)
				HitPlayer ();

			lava = !lava;
		}
		if (other.tag == "Exit")
		{
			Invoke ("Restart", 1);
			enabled = false;
		}
	}

	void Restart()
	{
		GameManager.instance.Restart ();
	}
}
