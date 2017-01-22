using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhaleController : MonoBehaviour {

	public static WhaleController Instance;

	public Transform whaleEndDropPoint;

	public Rigidbody2D rb;
	public float speed = 1f;
	public float scaleOffSet = 0f;

	// Oscilation controls
	public float tweenTime = 0.2f;
	public Vector3 offset = new Vector3(0,0.2f,0);
	[HideInInspector] 
	public int oscilateTweenID;

	public SpriteRenderer shadow;

	public Vector3 startingPos;

	public enum WhaleState{
		Floating,
		Hiding,
		Launching,
		Dropping
	}
	public WhaleState state = WhaleState.Floating;

	private float origRotation;
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
		shadow.material.color = Color.clear;

		rb = GetComponentInChildren<Rigidbody2D> ();
		origRotation = rb.rotation;

		// Start the game with the Whale, bopping up and down
		Oscilate();
	
	}

	Vector4 val = new Vector4(1,1,1,0);

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			if (IsWhaleHiding())
			{
				print("Launch Whale");
				SetWhaleLaunching();
			}
		}

		if (IsWhaleDropping())
		{
			shadow.transform.position = new Vector3(transform.position.x, shadow.transform.position.y, shadow.transform.position.z);
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
		if (IsWhaleHiding() || IsWhaleDropping()) 
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
		if (state == WhaleState.Launching) {
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

	void Hide()
	{
		LeanTween.cancel(oscilateTweenID);
		LeanTween.move(gameObject, transform.position + new Vector3(0,-2.3f,0), 0.5f)
			.setOnComplete(SurferController.Instance.Enter)
			.setEaseOutQuart();

		SetWhaleHiding();
	}

	public void Oscilate () 
	{
		
		var targetPos = transform.position + offset;
		oscilateTweenID = LeanTween.move(gameObject, targetPos, tweenTime).setLoopPingPong(-1).id;
	}

	// Launch whale into the sky
	void Launch()
	{
		print ("launch");
		// Set whale to look up
		rb.rotation = 90.0f;
		transform.position += new Vector3(0, -2.5f,0);

		Vector3 targetPos = transform.position + new Vector3(0, -1.2f,0);

		LeanTween.move(gameObject, targetPos, 0.3f).setOnComplete(() => {

			targetPos = transform.position + new Vector3(0,30.0f,0);
			LeanTween.move(gameObject, targetPos, 0.5f).setOnComplete(() => {
				
				SetWhaleDropping();

			}).setDelay(0.1f)

			.setOnComplete(() => {

					

					SetWhaleDropping();
					shadow.material.color = Color.white;


//					LeanTween.value( shadow.gameObject, (Color col) => {}, Color.clear, Color.white, 5.0f)
//						.setOnUpdateColor((Color col) => {
//							shadow.color = col;
//						});

					rb.rotation = -90;
					targetPos = new Vector3(transform.position.x, whaleEndDropPoint.position.y + 0.5f, transform.position.z);
					LeanTween.move(gameObject,targetPos, 0.4f).setDelay(5.0f).setOnComplete(() => {
						SetWhaleHiding();
						LeanTween.cancel(oscilateTweenID);

						LeanTween.cancelAll();
						rb.rotation = origRotation;
						transform.position += new Vector3(0, 4.0f);

					});
			});

		});


	}

	public void SetWhaleDropping() 
	{ 
		state = WhaleState.Dropping;
		//shadow.gameObject.SetActive(true);

	}
	public void SetWhaleFloating() { state = WhaleState.Floating; }
	public void SetWhaleHiding() 
	{ 	
		state = WhaleState.Hiding; 
		shadow.material.color = Color.clear;
	}

	public void SetWhaleLaunching() 
	{ 
		state = WhaleState.Launching;
		Launch();

	}


	public bool IsWhaleHiding() { return state == WhaleState.Hiding; }
	public bool IsWhaleDropping() { return state == WhaleState.Dropping; }
	public bool IsWhaleFloating() { return state == WhaleState.Floating; }
	public bool IsWhaleLaunching() { return state == WhaleState.Launching; }
	public WhaleState GetWhaleState() { return state; }

	public void StartWhaleProcess()
	{
		// Make whale sink into the water after some time
		Invoke("Hide", 0.0f);
	}


}
