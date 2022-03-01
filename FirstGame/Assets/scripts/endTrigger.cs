using UnityEngine;

public class endTrigger : MonoBehaviour {

	public gameManager manager;

	void OnTriggerEnter()
	{
		manager.completedLevel ();
		
	}

}
