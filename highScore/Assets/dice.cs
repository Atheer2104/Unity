using UnityEngine;
using UnityEngine.UI;

public class dice : MonoBehaviour {
	
	public Text score;
	public Text highScore;

	void Start()
	{
		highScore.text = PlayerPrefs.GetInt ("HighScore", 0).ToString();
	}

	public void rollDice()
	{
		int number = Random.Range (1, 7);
		score.text = number.ToString ();

		if (number > PlayerPrefs.GetInt ("HighScore", 0)) 
		{
			PlayerPrefs.SetInt ("HighScore", number);	
			highScore.text = number.ToString ();
		}
	}

	public void reset()
	{
		PlayerPrefs.DeleteKey ("HighScore");
		highScore.text = "0";
	}

}
