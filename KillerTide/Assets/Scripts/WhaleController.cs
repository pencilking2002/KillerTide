using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleController : MonoBehaviour {

	public static WhaleController Instance;

	public Rigidbody2D rb;
	public float speed = 1f;
	public float scaleOffSet = 0f;

	// Oscilation controls
	public float tweenTime = 0.2f;
	public Vector3 offset = new Vector3(0,0.2f,0);
	[HideInInspector] 
	public int oscilateTweenID;

	public Vector3 startingPos;

	public enum whaleSet{
		Floating,
		Hunting,
		Launching,
		Dropping
	}
	public whaleSet whaleState = whaleSet.Floating;


	private Vector2 world;
	private float x;
	private float y;


	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else 
			Destroy(gameObject);

		var currPos = transform.position;
		currPos.y = startingPos.y;
		transform.position = startingPos;
	}

	void Start () 
	{
		rb = GetComponentInChildren<Rigidbody2D> ();

		// Start the game with the Whale, bopping up and down
		Oscilate();

		// Make whale sink into the water after some time
		Invoke("Sink", 1.0f);
	}


	void OnGUI()
	{
		GUI.Button(new Rect(10,10,100,40), whaleState.ToString());
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			if (IsWhaleHunting())
			{
				print("Launch Whale");
				SetWhaleLaunching();
			}

		}
	}

	//Physics calculations
	void FixedUpdate() 
	{
		world = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height));
		WhaleHunt ();
	
	}

	void WhaleHunt()
	{
		if (whaleState == whaleSet.Hunting) 
		{
			x = Input.GetAxisRaw ("WhaleHorizontal");
			rb.AddForce (new Vector2 (x, 0) * speed, ForceMode2D.Impulse);

			float scale = transform.localScale.x - scaleOffSet;
			Vector3 pos = transform.position;
//			rb.position = new Vector2 (Mathf.Clamp (pos.x, -world.x, world.x),
//				Mathf.Clamp (pos.y, -world.y - 4f, world.y + 2.5f));
		}
	}


	void whaleInAir() {
		if (whaleState == whaleSet.Launching) {
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

	void Sink()
	{
		LeanTween.cancel(oscilateTweenID);
		LeanTween.move(gameObject, transform.position + new Vector3(0,-2.3f,0), 0.5f)
			.setOnComplete(SurferController.Instance.Enter);

		whaleState = whaleSet.Hunting;
	}

	public void Oscilate () 
	{
		var targetPos = transform.position + offset;
		oscilateTweenID = LeanTween.move(gameObject, targetPos, tweenTime).setLoopPingPong(-1).id;
	}

	// Launch whale into the sky
	void Launch()
	{
		rb.rotation = 90.0f;
		transform.position += new Vector3(0, -2.5f,0);

		Vector3 targetPos = transform.position + new Vector3(0, -1.2f,0);

		LeanTween.move(gameObject, targetPos, 0.3f).setOnComplete(() => {

			targetPos = transform.position + new Vector3(0, transform.position.y + 30.0f,0);
			LeanTween.move(gameObject, targetPos, 0.5f).setOnComplete(() => {
				rb.rotation = -90;
				SetWhaleDropping();
			}).setDelay(0.1f); 

		});
	}

	public void SetWhaleDropping() { whaleState = whaleSet.Dropping; }
	public void SetWhaleFloating() { whaleState = whaleSet.Floating; }
	public void SetWhaleHunting() { whaleState = whaleSet.Hunting; }

	public void SetWhaleLaunching() 
	{ 
		whaleState = whaleSet.Launching;
		Launch();
	}


	public bool IsWhaleHunting() { return whaleState == whaleSet.Hunting; }
	public bool IsWhaleDropping() { return whaleState == whaleSet.Dropping; }
	public bool IsWhaleFloating() { return whaleState == whaleSet.Floating; }

	public bool IsWhaleLaunching() { return whaleState == whaleSet.Launching; }


}
