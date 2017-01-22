using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurferController : MonoBehaviour {

	public static SurferController Instance;
	[HideInInspector]
	public Rigidbody2D rb;
	public float edgeCollisionAdjust = 0.15f;
	public float speed = 1f;

	public enum SurferState
	{
		Intro,
		Surfing,
		Jumping,
		Dead

	}
	SurferState state =  SurferState.Intro;

	public Transform SurferSpriteTransform;

	[Header("Oscilation controls")]
	public Vector3 offset = new Vector3(0,0.2f,0);
	public float tweenTime = 0.2f;
	[HideInInspector] 
	public int oscilateTweenID;
	public float jumpSpeed;


	private float x;
	private float y;
	private Vector2 world;
	private float initGravityScale;
	private Vector3 OscilateTargetPos;


	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else 
			Destroy(gameObject);

	}

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		initGravityScale = rb.gravityScale;
		rb.gravityScale = 0.0f;

		// Start the game with the Whale, bopping up and down
		Oscilate();

	}


	//Physics calculations
	void FixedUpdate() 
	{

		world = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height));

		x = Input.GetAxisRaw ("SurferHorizontal");
		//y = Input.GetAxisRaw ("WhaleVertical");

		rb.AddForce (new Vector2 (x ,0) * speed, ForceMode2D.Impulse);

		if (Input.GetKeyDown (KeyCode.Space) && !IsJumpingState()) 
		{
			rb.AddForce (new Vector2(0, jumpSpeed), ForceMode2D.Force);
			SetSurferJumping();
		}

	}

	public void Enter()
	{
		print ("Surfer enter");
		var targetPos = transform.position + new Vector3(6,0,0);
		LeanTween.move(gameObject, targetPos, 0.5f).setOnComplete(() => {
			rb.gravityScale = 10;
			SetSurferSurfing();

		});
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Border"))
		{
			SetSurferSurfing();
		}

		else if(col.gameObject.CompareTag("Whale") && WhaleController.Instance.IsWhaleDropping())
		{
			print("Kill surfer");
			rb.AddForce(new Vector2(Random.Range(0,200), 200), ForceMode2D.Impulse);
			rb.rotation = Random.Range(0,180);
			GetComponent<CapsuleCollider2D>().isTrigger = true;
			SetSurferDead();

			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	public void Oscilate () 
	{
		OscilateTargetPos = SurferSpriteTransform.localPosition + offset;
		oscilateTweenID = LeanTween.moveLocal(SurferSpriteTransform.gameObject, OscilateTargetPos, tweenTime).setLoopPingPong(-1).id;
	}

//	public void SetSurferIntro() { state = SurferState.Intro; }
//	public void SetSurferSurfing() { state = SurferState.Surfing; }
//	public void SetSurferJumping() { state = SurferState.Jumping; }
//	public void SetSurferDead() { state = SurferState.Dead; }


	public void SetSurferIntro() { state = SurferState.Intro; }
	public void SetSurferSurfing() { state = SurferState.Surfing; }
	public void SetSurferJumping() { state = SurferState.Jumping; }
	public void SetSurferDead() { state = SurferState.Dead; }

	public bool IsSurfingState() { return state == SurferState.Surfing; }
	public bool IsIntroState() { return state == SurferState.Intro; }
	public bool IsJumpingState() { return state == SurferState.Jumping; }
	public bool IsDeadState() { return state == SurferState.Dead; }

	public SurferState GetState()
	{
		return state;
	}

}
