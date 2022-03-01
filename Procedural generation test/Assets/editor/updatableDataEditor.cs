using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// we added true so we can get this custom editor on object that inherit from updatableData
[CustomEditor (typeof(updatableData), true)]
public class updatableDataEditor : Editor {

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();

        // target is the object that we are inspecting
        updatableData data = (updatableData)target;

        if (GUILayout.Button("Update")) {
            data.notifyOfUpdatedValues();
            EditorUtility.SetDirty(target);
        }
    }
    
}
