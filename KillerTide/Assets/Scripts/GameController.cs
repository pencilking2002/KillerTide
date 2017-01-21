using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	private float windPowerCurrent = 0f;
	private float windDelay = 5f;
	private float windTimer = 0f;
	private float windPowerPrevious = 0f;
	public bool windSwitch = false;

	[SerializeField]
	private Rigidbody2D whaleRB;
	[SerializeField]
	private Rigidbody2D surferRB;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
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
}
