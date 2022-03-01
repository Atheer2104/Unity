using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour {

    Animator animator;
    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical");
        transform.Translate(0, 0, translation * Time.deltaTime);
        if (translation != 0 ) {
            animator.SetBool("running", true);
        }else {
            animator.SetBool("running", false);
        }
    }
}
