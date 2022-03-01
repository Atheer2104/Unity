using UnityEngine;

public class followPlayer : MonoBehaviour {

	public Transform player;

//	public float smoothSpeed = 1f;
	public Vector3 offset;
	
	// Update is called once per frame
	void Update () {
//		Vector3 desiredPosition = player.position + offset;
//		Vector3 smoothedPosition = Vector3.Lerp (transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
//		transform.position = smoothedPosition;
		transform.position = player.position + offset;

	}
}
