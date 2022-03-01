using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class msPacMan : MonoBehaviour {

	public float speed = 4f;
	private Rigidbody2D rb;
	public Sprite paused;

	soundManager sound;

	public AudioClip eatingGhost;
	public AudioClip pacmandies;
	public AudioClip powerupEating;

	gameBoard gameBoard;

	Ghost redGhostScript;
	Ghost pinkGhostScript;
	Ghost blueGhostScript;
	Ghost orangeGhostScript;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D> ();

		gameBoard = FindObjectOfType (typeof(gameBoard)) as gameBoard;

		GameObject redGhostGO = GameObject.Find ("redGhost");
		GameObject pinkGhostGO = GameObject.Find ("pinkGhost");
		GameObject blueGhostGO = GameObject.Find ("blueGhost");
		GameObject ornageGhostGO = GameObject.Find ("ornageGhost");

		redGhostScript = (Ghost)redGhostGO.GetComponent (typeof(Ghost));
		pinkGhostScript = (Ghost)pinkGhostGO.GetComponent (typeof(Ghost));
		blueGhostScript = (Ghost)blueGhostGO.GetComponent (typeof(Ghost));
		orangeGhostScript = (Ghost)ornageGhostGO.GetComponent (typeof(Ghost));
	}

	// Use this for initialization
	void Start () {

		rb.velocity = new Vector2 (-1, 0) * speed;
		sound = GameObject.Find ("soundManager").GetComponent<soundManager> ();

	}

	void FixedUpdate()
	{
		float horzMove = Input.GetAxisRaw ("Horizontal");
		float vertMove = Input.GetAxisRaw ("Vertical");

		Vector2 moveVect;

		var localVelocity = transform.InverseTransformDirection (rb.velocity);

		if (Input.GetKeyDown ("a")) {
			if (localVelocity.x > 0 && gameBoard.isValidSpace(transform.position.x -1, transform.position.y)) {
				moveVect = new Vector2 (horzMove, 0);
				transform.position = new Vector2 ((int)transform.position.x + .5f, (int)transform.position.y + .5f);

				rb.velocity = moveVect * speed;

				transform.localScale = new Vector2 (1, 1);

				transform.localRotation = Quaternion.Euler (0, 0, 0);
			} else {
				moveVect = new Vector2 (horzMove, 0);
				if (canIMoveInDir (moveVect)) {
					transform.position = new Vector2 ((int)transform.position.x + .5f, (int)transform.position.y + .5f);

					rb.velocity = moveVect * speed;

					transform.localScale = new Vector2 (1, 1);

					transform.localRotation = Quaternion.Euler (0, 0, 0);
				}
			}

		} else if (Input.GetKeyDown ("d") ) {
			if (localVelocity.x < 0 && gameBoard.isValidSpace(transform.position.x + 1, transform.position.y)) {
				moveVect = new Vector2 (horzMove, 0);
				transform.position = new Vector2 ((int)transform.position.x + .5f, (int)transform.position.y + .5f);

				rb.velocity = moveVect * speed;

				transform.localScale = new Vector2 (-1, 1);

				transform.localRotation = Quaternion.Euler (0, 0, 0);
			} else {
				moveVect = new Vector2 (horzMove, 0);
				if (canIMoveInDir (moveVect)) {
					transform.position = new Vector2 ((int)transform.position.x + .5f, (int)transform.position.y + .5f);

					rb.velocity = moveVect * speed;

					transform.localScale = new Vector2 (-1, 1);

					transform.localRotation = Quaternion.Euler (0, 0, 0);
				}
			}

		} else if (Input.GetKeyDown ("w")) {
			if (localVelocity.y > 0 && gameBoard.isValidSpace(transform.position.x, transform.position.y + 1)) {
				moveVect = new Vector2 (0, vertMove);
				transform.position = new Vector2 ((int)transform.position.x + .5f, (int)transform.position.y + .5f);

				rb.velocity = moveVect * speed;

				transform.localScale = new Vector2 (1, 1);

				transform.localRotation = Quaternion.Euler (0, 0, 270);
			} else {
				moveVect = new Vector2 (0, vertMove);
				if (canIMoveInDir (moveVect)) {
					transform.position = new Vector2 ((int)transform.position.x + .5f, (int)transform.position.y + .5f);

					rb.velocity = moveVect * speed;

					transform.localScale = new Vector2 (1, 1);

					transform.localRotation = Quaternion.Euler (0, 0, 270);
				}
			}

		} else if (Input.GetKeyDown ("s")) {
			if (localVelocity.y < 0 && gameBoard.isValidSpace(transform.position.x, transform.position.y - 1)) {
				moveVect = new Vector2 (0, vertMove);
				transform.position = new Vector2 ((int)transform.position.x + .5f, (int)transform.position.y + .5f);

				rb.velocity = moveVect * speed;

				transform.localScale = new Vector2 (1, 1);

				transform.localRotation = Quaternion.Euler (0, 0, 90);
			} else {
				moveVect = new Vector2 (0, vertMove);
				if (canIMoveInDir (moveVect)) {
					transform.position = new Vector2 ((int)transform.position.x + .5f, (int)transform.position.y + .5f);

					rb.velocity = moveVect * speed;

					transform.localScale = new Vector2 (1, 1);

					transform.localRotation = Quaternion.Euler (0, 0, 90);
				}
			}
		}

		UpdateEatingAnimation ();
	}

	bool canIMoveInDir(Vector2 dir)
	{
		//packman position
		Vector2 pos = transform.position;

		Transform point = GameObject.Find ("GridGB").GetComponent<gameBoard> ().gbPoints [(int)pos.x, (int)pos.y];

		if (point != null) 
		{
			GameObject pointGO = point.gameObject;

			Vector2[] vectToNextPoint = pointGO.GetComponent<turningPoint> ().vectToNextPoint;

			foreach (Vector2 vect in vectToNextPoint) 
			{
				if (vect == dir) 
				{
					return true;
				}
			}
		}

		return false;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		bool hitAWall = false;

		if (col.gameObject.tag == "point") 
		{
			Vector2[] vectToNextPoint = col.GetComponent<turningPoint> ().vectToNextPoint;

			if (Array.Exists (vectToNextPoint, element => element == rb.velocity.normalized)) {
				hitAWall = false;
			} else 
			{
				hitAWall = true;
			}

			transform.position = new Vector2 ((int)col.transform.position.x + .5f, (int)col.transform.position.y + .5f);

			if (hitAWall)
				rb.velocity = Vector2.zero;
		}

		Vector2 pmMoveVect = new Vector2 (0, 0);

		if (transform.position.x < 2 && transform.position.y == 15.5) {
			transform.position = new Vector2 (24.5f, 15.5f);
			pmMoveVect = new Vector2 (-1, 0);
			rb.velocity = pmMoveVect * speed;
		} else if (transform.position.x > 25 && transform.position.y == 15.5) 
		{
			transform.position = new Vector2 (2f, 15.5f);
			pmMoveVect = new Vector2 (1, 0);
			rb.velocity = pmMoveVect * speed;
		}

		if (col.gameObject.tag == "dot") 
		{
			dotWasEaten (col);
		}

		if (col.gameObject.tag == "pill") 
		{
			soundManager.Instance.playOneShot (soundManager.Instance.powerUpEating);

			redGhostScript.turnGhostBlue ();
			pinkGhostScript.turnGhostBlue ();
			blueGhostScript.turnGhostBlue ();
			orangeGhostScript.turnGhostBlue ();

			increaseScore (50);

			Destroy (col.gameObject);
		}
		if (col.gameObject.tag == "ghost") 
		{
			string ghostName = col.GetComponent<Collider2D> ().gameObject.name;

			AudioSource source = sound.GetComponent<AudioSource> ();

			if (ghostName == "redGhost") 
			{
				if (redGhostScript.isGhostBlue) {
					redGhostScript.resetGhostAfterEaten (gameObject);
					soundManager.Instance.playOneShot (soundManager.Instance.eatingGhost);

					increaseScore (400);
				} else 
				{
					soundManager.Instance.playOneShot (soundManager.Instance.pacmanDies);

					source.Stop ();

					Destroy (gameObject);
				}
			} else if (ghostName == "pinkGhost") 
			{
				if (pinkGhostScript.isGhostBlue) {
					pinkGhostScript.resetGhostAfterEaten (gameObject);
					soundManager.Instance.playOneShot (soundManager.Instance.eatingGhost);

					increaseScore (400);
				} else 
				{
					soundManager.Instance.playOneShot (soundManager.Instance.pacmanDies);

					source.Stop ();

					Destroy (gameObject);
				}
			}else if (ghostName == "blueGhost") 
			{
				if (blueGhostScript.isGhostBlue) {
					blueGhostScript.resetGhostAfterEaten (gameObject);
					soundManager.Instance.playOneShot (soundManager.Instance.eatingGhost);

					increaseScore (400);
				} else 
				{
					soundManager.Instance.playOneShot (soundManager.Instance.pacmanDies);

					source.Stop ();

					Destroy (gameObject);
				}
			}else if (ghostName == "ornageGhost") 
			{
				if (orangeGhostScript.isGhostBlue) {
					orangeGhostScript.resetGhostAfterEaten (gameObject);
					soundManager.Instance.playOneShot (soundManager.Instance.eatingGhost);

					increaseScore (400);
				} else 
				{
					soundManager.Instance.playOneShot (soundManager.Instance.pacmanDies);

					source.Stop ();

					Destroy (gameObject);
				}
			}
		}
	}

	void UpdateEatingAnimation()
	{
		if (rb.velocity == Vector2.zero) {
			GetComponent<Animator> ().enabled = false;
			GetComponent<SpriteRenderer> ().sprite = paused;

			sound.pausePacman ();
		} else 
		{
			GetComponent<Animator> ().enabled = true;

			sound.unpausePacman ();
		}
	}

	void dotWasEaten(Collider2D col)
	{
		increaseScore (10);

		Destroy (col.gameObject);
	}

	void increaseScore(int points)
	{
		Text scoreText = GameObject.Find ("score").GetComponent<Text> ();

		int score = int.Parse (scoreText.text);

		score += points;

		scoreText.text = score.ToString ();
	}

}
