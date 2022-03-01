using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour {

	public static soundManager Instance = null;

	public AudioClip eatingDots;
	public AudioClip eatingGhost;
	public AudioClip ghostMove;
	public AudioClip pacmanDies;
	public AudioClip powerUpEating;

	private AudioSource pacmanAudiosource;
	private AudioSource ghostAudiosource;
	private AudioSource oneShotAudiosource;

	// Use this for initialization
	void Start () {

		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) 
		{
			Destroy (gameObject);
		}

		AudioSource[] audioSources = GetComponents<AudioSource> ();

		pacmanAudiosource = audioSources [0];
		ghostAudiosource = audioSources [1];
		oneShotAudiosource = audioSources [2];

		PlayClipOnLoop (pacmanAudiosource, eatingDots);
	}
	
	public void playOneShot(AudioClip clip)
	{
		oneShotAudiosource.PlayOneShot (clip);
	}

	public void PlayClipOnLoop(AudioSource aS, AudioClip clip)
	{
		if (aS != null && clip != null) 
		{
			aS.loop = true;
			aS.volume = 0.2f;
			aS.clip = clip;
			aS.Play ();
		}
	}

	public void pausePacman()
	{
		if (pacmanAudiosource != null && pacmanAudiosource.isPlaying) 
		{
			pacmanAudiosource.Stop ();
		}
	}

	public void unpausePacman()
	{
		if (pacmanAudiosource != null && !pacmanAudiosource.isPlaying) 
		{
			pacmanAudiosource.Play ();
		}
	}

	//Ghost
	public void pauseGhost()
	{
		if (ghostAudiosource != null && ghostAudiosource.isPlaying) 
		{
			ghostAudiosource.Stop ();
		}
	}

	public void unpauseGhost()
	{
		if (ghostAudiosource != null && !ghostAudiosource.isPlaying) 
		{
			ghostAudiosource.Play ();
		}
	}


}
