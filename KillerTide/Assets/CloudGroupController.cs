using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGroupController : MonoBehaviour {

	public static CloudGroupController Instance;
	public float cloudSpeed;
	public bool isLeft;

	// Use this for initialization
	void Awake () 
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
