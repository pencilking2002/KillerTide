using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWater : MonoBehaviour {

	public float moveSpeed = 0.5f;
	public bool reset = false;

	public GameObject waterDetailPrefab;
	private Vector3 origPos;
	private BoxCollider2D bCollider;

	// Use this for initialization
	void Awake () 
	{
		origPos = transform.position;
		bCollider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{


		Vector3 screenWorldRightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0,-10));
		Vector3 screenWorldLeftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0,0,-10));


		if (transform.position.x > screenWorldLeftEdge.x - bCollider.bounds.size.x)
		{
			transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0,0);
		}
		else
		{

			Vector3 targetPos = origPos;
			targetPos.x = screenWorldRightEdge.x;
	
			transform.position = targetPos;
		}

				
	}
//
//	void OnBecameInvisible()
//	{
//		print ("invisible");
//		visible = false;
//
//	}

}
