using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour {

	public static GameAudio Instance;

	public enum MusicType
	{
		Menu,
		Game	
	}

	public AudioClip menuMusic;
	public AudioClip gameMusic;

	private AudioSource  aSource;

	void Awake()
	{
		aSource = GetComponent<AudioSource>();

		if (Instance == null)
			Instance = this;
		else 
			Destroy(gameObject);
	
	}

	public void Play(MusicType type)
	{
		if (type == MusicType.Menu)
		{
			aSource.PlayOneShot(menuMusic);
		}
		else if (type == MusicType.Game)
		{
			aSource.PlayOneShot(gameMusic);
		}
	}

	public void Stop()
	{

		aSource.Stop();
	}



}
