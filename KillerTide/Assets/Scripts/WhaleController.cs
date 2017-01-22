using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleController : MonoBehaviour {

	public Rigidbody2D rb;

	private float x;
	private float y;
	public float speed = 1f;
	private Vector2 world;
	public float scaleOffSet = 0f;

	public enum whaleSet{
		hunting,
		inAir,
		falling
	}

	public whaleSet whaleState;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		whaleHunt ();
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			whaleState = whaleSet.inAir;
			whaleInAir ();
		}
//		if (whaleState == whaleSet.falling)
//			whaleFall ();
	}

	/// <summary>
	/// invoke method
	/// </summary>

	//Physics calculations
	void FixedUpdate() {
		world = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height));
	
//		x = Input.GetAxisRaw ("WhaleHorizontal");
//		y = Input.GetAxisRaw ("WhaleVertical");
//
//		rb.AddForce (new Vector2 (x ,y) * speed, ForceMode2D.Impulse);
//		//print (transform.position);
//
//		float scale = transform.localScale.x - scaleOffSet;
//		Vector3 pos = transform.position;
//
//			rb.position = new Vector2 (
//
//				Mathf.Clamp (pos.x, -(world.x - scale), (world.x - scale)),
//				Mathf.Clamp (pos.y, -(world.y - scale), (world.y - scale)));
	
	}

	void whaleHunt()
	{
		if (whaleState == whaleSet.hunting) {
			x = Input.GetAxisRaw ("WhaleHorizontal");
			rb.AddForce (new Vector2 (x, y) * speed, ForceMode2D.Impulse);
			float scale = transform.localScale.x - scaleOffSet;
			Vector3 pos = transform.position;
			rb.position = new Vector2 (Mathf.Clamp (pos.x, -world.x, world.x),
				Mathf.Clamp (pos.y, -world.y - 4f, world.y + 2.5f));
		}
	}


	void whaleInAir() {
		if (whaleState == whaleSet.inAir) {
			y = Input.GetAxisRaw ("WhaleVertical");
			rb.AddForce (new Vector2 (0, 50f), ForceMode2D.Impulse);
			//rb.AddForce(transform.up * speed);
//		Vector3 pos = transform.position;
//		rb.position = new Vector2 (
//			Mathf.Clamp (pos.x, -(world.x), (world.x)),
//			Mathf.Clamp (pos.y, -(world.y), (world.y)));
			Invoke ("whaleFalling", 5f);
		}
	}
//
//	void whaleFalling(){
//	}
}
