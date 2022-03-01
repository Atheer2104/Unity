using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turningPoint : MonoBehaviour {

	public turningPoint[] nextPoints;
	public Vector2[] vectToNextPoint;

	// Use this for initialization
	void Start () {

		vectToNextPoint = new Vector2[nextPoints.Length];

		for (int i = 0; i < nextPoints.Length; i++) 
		{
			turningPoint nextpoint = nextPoints [i];

			Vector2 pointVec = nextpoint.transform.localPosition - transform.localPosition;

			vectToNextPoint [i] = pointVec.normalized; 
		}

	}
}
