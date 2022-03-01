using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ainm : MonoBehaviour {

    static Animator anim;

    // Start is called before the first frame update
    void Start() {
        
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown("space")) {
            anim.SetBool("playing", true);
            bool play = anim.GetBool("playing");
            Debug.Log("the value is: " + play);
        } else {
            anim.SetBool("playing", false);
        }
            
        
    }
}
