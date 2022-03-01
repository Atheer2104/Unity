using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (mapPreview))]
public class mapPreviewEditor : Editor {
   
    public override void OnInspectorGUI() {
        // target is the object that we are inspecting
        mapPreview mapPreview = (mapPreview) target;

        // this codes excutes if any value in the inspector changes
        if (DrawDefaultInspector()) {
            if (mapPreview.autoUpdate) {
                mapPreview.drawMapInEditor();
            }
        }

        // creating button and if clicked we will generate our map
        if (GUILayout.Button("Generate")) {
            mapPreview.drawMapInEditor();
        }

    }

}
