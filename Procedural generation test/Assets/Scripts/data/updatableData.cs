using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updatableData : ScriptableObject {

    public event System.Action onValuesUpdated;
    public bool autoUpdate;

    // this code will only be run when we are in unity 
    #if UNITY_EDITOR

    // by defualt this won't be called because we are calling same method from noiseData 
    // to make sure it gets called we add protected virtual
    protected virtual void OnValidate() {
        if (autoUpdate) {
            UnityEditor.EditorApplication.update += notifyOfUpdatedValues;
        }
    }

    public void notifyOfUpdatedValues() {
        UnityEditor.EditorApplication.update -= notifyOfUpdatedValues;
        if (onValuesUpdated != null) {
            onValuesUpdated();
        }
    }

    #endif

}
