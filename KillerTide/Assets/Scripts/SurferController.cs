using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurferController : MonoBehaviour {

	public Rigidbody2D rb;

	private float x;
	private float y;
	public float speed = 1f;
	private Vector2 world;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
	}

	//Physics calculations
	void FixedUpdate() {
		world = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height));

		x = Input.GetAxisRaw ("SurferHorizontal");
		y = Input.GetAxisRaw ("SurferVertical");

		rb.AddForce (new Vector2 (x ,y) * speed, ForceMode2D.Impulse);

		float scale = transform.localScale.x - .15f;
		Vector3 pos = transform.position;

		rb.position = new Vector2 (

			Mathf.Clamp (pos.x, -(world.x - scale), (world.x - scale)),
			Mathf.Clamp (pos.y, -(world.y - scale), (world.y - scale)));

	}
}
