using UnityEngine;
using System.Collections;
using UnityEditor;

// we added true so we can get this custom editor on object that inherit from updatableData
[CustomEditor (typeof(UpdatableData), true)]
public class UpdatableDataEditor : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		// target is the object that we are inspecting
		UpdatableData data = (UpdatableData)target;

		if (GUILayout.Button ("Update")) {
			data.NotifyOfUpdatedValues ();
			EditorUtility.SetDirty (target);
		}
	}
	
}
