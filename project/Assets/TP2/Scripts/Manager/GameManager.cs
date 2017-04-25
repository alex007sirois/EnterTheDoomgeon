using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public float turnDelay = 0.1F;
	public static GameManager instance = null;
	public BoardManager boardScript;
	public bool playersTurn=true;
	public byte heart=3;
	public byte bullet=3;
	public byte skull=0;
	private Text skullText;

	private bool gameOver = false;

	private int level=0;
	public List<Ennemy> ennemies;
	public bool ennemiesMoving;

	// Use this for initialization
	void Awake () {
		ennemies = new List<Ennemy>();
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		boardScript = GetComponent<BoardManager>();

		level = instance.level;
		skull = instance.skull;

		InitGame ();

	}

	void Update()
	{
		if (!gameOver) {
			skull = instance.skull;

			if (skullText == null)
				skullText = GameObject.Find ("SkullText").GetComponent<Text> ();

			skullText.text = "x" + skull;

			if (playersTurn || ennemiesMoving)
				return;
		
			StartCoroutine (MoveEnnemies ());
		}
	}

	public void GameOver()
	{
		gameOver = true;

		uint score = (uint)(100 * Mathf.Sqrt(((level-1)*(skull+1))));

		GameObject scoreG = GameObject.Find("Score");
		scoreG.name = "Score: " + score;
		DontDestroyOnLoad (scoreG);

		Destroy (gameObject);

		SceneManager.LoadScene (2);
	}

	public void Restart()
	{
		ennemies.Clear ();
		SceneManager.LoadScene (1);
	}

	public void AddEnnemyToList(Ennemy e)
	{
		ennemies.Add (e);
	}

	void InitGame () {
		ennemies.Clear ();
		level++;
		instance.level = level;
		boardScript.SetupScene (level);
	}

	IEnumerator MoveEnnemies()
	{
		ennemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);

		for (int i = 0; i < ennemies.Count; i++) {
			ennemies [i].MoveEnnemy ();

		}
		yield return new WaitForSeconds (0.1F);

		playersTurn = true;
		ennemiesMoving = false;
	}
}
