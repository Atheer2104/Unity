using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MapPreview))]
public class MapPreviewEditor : Editor {

	public override void OnInspectorGUI() {
		// target is the object that we are inspecting
		MapPreview mapPreview = (MapPreview)target;

		// this codes excutes if any value in the inspector changes
		if (DrawDefaultInspector ()) {
			if (mapPreview.autoUpdate) {
				mapPreview.DrawMapInEditor ();
			}
		}

		// creating button and if clicked we will generate our map
		if (GUILayout.Button ("Generate")) {
			mapPreview.DrawMapInEditor ();
		}
	}
}
