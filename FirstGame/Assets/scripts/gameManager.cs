using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {

	bool gameHasEnded = false;

	public float restartDelay = 1f;

	public GameObject completeLevelUI;

	public void completedLevel()
	{
		completeLevelUI.SetActive (true);
	}

	public void endGame ()
	{
		if (gameHasEnded == false) {
			gameHasEnded = true;
			Debug.Log ("game over"); 
			// restart game
			Invoke("restart", restartDelay);
		}
	}

	void restart()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

}

