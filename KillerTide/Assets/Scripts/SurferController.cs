using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurferController : MonoBehaviour {

	public static SurferController Instance;
	[HideInInspector]
	public Rigidbody2D rb;
	public float edgeCollisionAdjust = 0.15f;
	public float speed = 1f;

	public Transform SurferSpriteTransform;

	[Header("Oscilation controls")]
	public Vector3 offset = new Vector3(0,0.2f,0);
	public float tweenTime = 0.2f;
	[HideInInspector] 
	public int oscilateTweenID;

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



	}

	public void Enter()
	{
		print ("Surfer enter");
		var targetPos = transform.position + new Vector3(6,0,0);
		LeanTween.move(gameObject, targetPos, 0.5f);
	}

	public void Oscilate () 
	{
		OscilateTargetPos = SurferSpriteTransform.localPosition + offset;
		oscilateTweenID = LeanTween.moveLocal(SurferSpriteTransform.gameObject, OscilateTargetPos, tweenTime).setLoopPingPong(-1).id;
	}

}
