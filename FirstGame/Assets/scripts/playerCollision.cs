using UnityEngine;

public class playerCollision : MonoBehaviour {

	public playerMovment movement;

	void OnCollisionEnter( Collision collisionInfo)
	{
		if (collisionInfo.collider.tag == "Obstacle") 
		{
			movement.enabled = false;
			FindObjectOfType<gameManager> ().endGame ();

		}
	}



}
