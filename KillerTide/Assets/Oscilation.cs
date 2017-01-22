using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilation : MonoBehaviour {

	public float tweenTime = 0.2f;
	public Vector3 offset = new Vector3(0.2f,0.2f,0);
	// Use this for initialization
	void Start () 
	{
		var targetPos = transform.position + offset;
		LeanTween.move(gameObject, targetPos, tweenTime).setLoopPingPong(-1);
	}

}
