using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudControllerMiddle : MonoBehaviour {

	private Vector2 world;
	private float speed;

	// Use this for initialization
	void Start () {
		world = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height));
		print (world);
		speed = Random.Range (.1f, 1f);
	}
	
	// Update is called once per fram

	void Update () {
		Vector2 nextPosition = new Vector2 (transform.position.x + speed, transform.position.y);
		transform.position = Vector2.Lerp (transform.position, nextPosition, Time.deltaTime * 2.5f);
//		transform.Translate(Vector2.right * Random.Range(.1f, 1f));
//		if (transform.position.x > world.x + 2f) {
//			transform.position  = new Vector2 (-(world.x + 2f), transform.position.y);
//		}
		if (transform.position.x > world.x + 2.2f) {
			transform.position  = new Vector2 (-(world.x + 1.75f), transform.position.y);
		}
	}
}
