using UnityEngine;
using UnityEngine.SceneManagement;

public class levelcomplete : MonoBehaviour {

	public void loadNextlevel()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
	}
}
