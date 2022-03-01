using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
// ScriptableObject a type of script that is meant to store data 
public class noiseMapSettings : updatableData {

    public noiseSettings noiseSettings;

     public bool useFalloff;

    public float heightMultiplier;
    public AnimationCurve heightCurve;

    public float minHeight {
        get {
            return heightMultiplier * heightCurve.Evaluate(0);
        }
    }

    public float maxHeight {
        get {
            return heightMultiplier * heightCurve.Evaluate(1);
        }
    }

   
    #if UNITY_EDITOR

    protected override void OnValidate() {
        noiseSettings.validateValues();
        // calling OnValidate from updatableData
        base.OnValidate();
    }

    #endif
   
}
