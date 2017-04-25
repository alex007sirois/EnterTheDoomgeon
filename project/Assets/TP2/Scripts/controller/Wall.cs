using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == null)
			return;
		if (other.tag == "GunBullet") {
			other.transform.position = new Vector3 (-10, -10, 0);
		}
	}
}
