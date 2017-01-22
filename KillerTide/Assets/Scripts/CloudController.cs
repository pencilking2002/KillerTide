using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {

	private Vector2 world;
	private float cloudSpeed;
	private SpriteRenderer sp;

	// Use this for initialization
	void Start () {
		sp = GetComponent<SpriteRenderer> ();

		cloudSpeed = CloudGroupController.Instance.cloudSpeed * transform.localScale.x;

		print (CloudGroupController.Instance.cloudSpeed);	

		world = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height));
		//print (world);
	}
	
	// Update is called once per fram

	void Update () {
		if (CloudGroupController.Instance.isLeft) {
			Vector2 nextPosition = new Vector2 (transform.position.x - cloudSpeed, transform.position.y);
			transform.position = Vector2.Lerp (transform.position, nextPosition, Time.deltaTime * cloudSpeed);
			if (transform.position.x < -(world.x + 2.2f)) {
				transform.position = new Vector2 ((world.x + 1.75f), transform.position.y);
			}
		} else {
			Vector2 nextPosition = new Vector2 (transform.position.x + cloudSpeed, transform.position.y);
			transform.position = Vector2.Lerp (transform.position, nextPosition, Time.deltaTime * cloudSpeed);
			if (transform.position.x > (world.x+ 2.2f)) {
				transform.position = new Vector2 (-(world.x + 1.75f), transform.position.y);
			}
		}
	}
}
