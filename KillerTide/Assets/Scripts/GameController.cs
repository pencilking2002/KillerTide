using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController Instance;

	[Header("Game Menu controls")]
	public GameObject hyperCube;
	public bool startFromGameMenu = true;
	public Transform GameStartPoint;

	private float windPowerCurrent = 0f;
	private float windDelay = 5f;
	private float windTimer = 0f;
	private float windPowerPrevious = 0f;
	public bool windSwitch = false;

	public Camera cam;
	public GameObject canvas;

	[SerializeField]
	private Rigidbody2D whaleRB;
	[SerializeField]
	private Rigidbody2D surferRB;

	// This will be assigned either the camera or the hyper cube group
	private GameObject cameraToMove;

	public enum GameState{
		Menu,
		Playing,
		SurferWin,
		WhaleWin
	}
	public GameState state = GameState.Menu;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else 
			Destroy(gameObject);

		if (startFromGameMenu)
			SetMenuState();
		else
			SetPlayingState();
	}

	// Use this for initialization
	void Start () 
	{
		if (IsMenuState())
		{
			if (hyperCube.activeInHierarchy)
			{
				hyperCube.transform.position = 
					new Vector3(hyperCube.transform.position.x, canvas.transform.position.y, hyperCube.transform.position.z);

				cameraToMove = hyperCube;
			}
			else
			{
				print("move cam");
				cam.transform.position = 
					new Vector3(cam.transform.position.x, canvas.transform.position.y, cam.transform.position.z);

				cameraToMove = cam.gameObject;
			
			}
		}


	}



	void OnGUI()
	{
		GUI.Button(new Rect(10,10,100,40), WhaleController.Instance.GetWhaleState().ToString());
		GUI.Button(new Rect(10,50,100,40), GetGameState().ToString());
		GUI.Button(new Rect(10,100,100,40), SurferController.Instance.GetState().ToString());
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			print("start");
			MoveCameraToStartGamePosition();
		}

		if (windTimer < windDelay) {
			windTimer = windTimer + Time.deltaTime;
		}
		if (windTimer > windDelay && windSwitch == true){
			windPowerCurrent = Random.Range(-1.5f, 1.5f);
			if (windPowerCurrent > 0f) {
				whaleRB.AddForce (new Vector2 (windPowerCurrent, 0), ForceMode2D.Impulse);
				surferRB.AddForce (new Vector2 (windPowerCurrent, 0), ForceMode2D.Impulse);
			} else if (windPowerCurrent < 0f)
			{
				whaleRB.AddForce(new Vector2 (-windPowerCurrent ,0), ForceMode2D.Impulse);
				surferRB.AddForce(new Vector2 (-windPowerCurrent ,0), ForceMode2D.Impulse);
			}
			windTimer = 0;
			print (windPowerCurrent);
		}


	}

	public void MoveCameraToStartGamePosition()
	{
		var targetPos = new Vector3(cameraToMove.transform.position.x, GameStartPoint.position.y, cameraToMove.transform.position.z);

		if (!hyperCube.activeInHierarchy)
			targetPos.y = 0;

		print("start game");
		LeanTween.move(cameraToMove,targetPos, 2.0f).setEaseOutQuint().setDelay(0.2f)
			.setOnComplete(() => {
			SetPlayingState();
		});

	}


	// GAme state

	public void SetMenuState() { state = GameState.Menu; }
	public void SetPlayingState() 
	{ 
		state = GameState.Playing;
		StartGame(); 
	}
	public void SetWhaleWin() { state = GameState.WhaleWin; }
	public void SetSurferWin() { state = GameState.SurferWin; }


	public bool IsMenuState() { return state == GameState.Menu; }
	public bool IsPlayingState() 
	{ 
		
		return state == GameState.Playing; 
	}

	public bool IsWhaleWinState() { return state == GameState.WhaleWin; }
	public bool IsSurferWinState() { return state == GameState.SurferWin; }
	public GameState GetGameState() { return state; }

	public void StartGame()
	{
		WhaleController.Instance.StartWhaleProcess();
		GameAudio.Instance.Stop();
		GameAudio.Instance.Play(GameAudio.MusicType.Game);
	}

}
