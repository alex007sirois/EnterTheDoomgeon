using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("ScoreText").GetComponent<Text>().text=GameObject.FindGameObjectWithTag("Score").name;
		Destroy (GameObject.FindGameObjectWithTag ("Score"));
	}
}
