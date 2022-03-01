using UnityEngine;
using System.Collections;

// ScriptableObject a type of script that is meant to store data 
public class UpdatableData : ScriptableObject {

	public event System.Action OnValuesUpdated;
	public bool autoUpdate;

	// this code will only be run when we are in unity 
	#if UNITY_EDITOR

	// by defualt this won't be called because we are calling same method from noiseData 
    // to make sure it gets called we add protected virtual
	protected virtual void OnValidate() {
		if (autoUpdate) {
			UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;
		}
	}

	public void NotifyOfUpdatedValues() {
		UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;
		if (OnValuesUpdated != null) {
			OnValuesUpdated ();
		}
	}

	#endif

}