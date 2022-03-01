using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : MonoBehaviour {
    public Light light;
    public int value;
    public ParticleSystem pickupEffect;
    public AudioSource audioSource;
    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            // finding gamemanager adding gold
            FindObjectOfType<GameManager>().AddGold(value);
            
            // adding pickupEffect
            Instantiate(pickupEffect, transform.position, transform.rotation);
            
            audioSource.Play(0);
            
            // then removing the gold bar 
            Destroy(gameObject);

        }
    }
}
