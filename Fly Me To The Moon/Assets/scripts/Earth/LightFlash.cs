using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlash : MonoBehaviour {
    public Light light;
    public float maxIntensity = 1f;
    public float minIntensity = 0f;
    //a value of 0.5f would take 2 seconds and a value of 2f would take half a second
    public float pulseSpeed;
    private float timer;
    private float targetIntensity = 1f;
    private float currentIntensity;


    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        currentIntensity = Mathf.MoveTowards(light.intensity, targetIntensity, Time.deltaTime*pulseSpeed);
        if (currentIntensity >= maxIntensity) {
            currentIntensity = maxIntensity;
            targetIntensity = minIntensity;
        } else if (currentIntensity <= minIntensity) {
            currentIntensity = minIntensity;
            targetIntensity = maxIntensity;
        }
        light.intensity = currentIntensity;
    }
}
