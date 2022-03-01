using UnityEngine;

public class timemanager : MonoBehaviour {

	public float slowdownFactor = 0.05f;
	public float slowdownLength = 2f;

	void Update()
	{
		Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
		Time.timeScale = Mathf.Clamp (Time.timeScale, 0f, 1f);
		Time.fixedDeltaTime = Time.timeScale * .02f;
	}

	public void DoSlowmotion()
	{
		Time.timeScale = slowdownFactor;


	}

}
