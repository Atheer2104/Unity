using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    public CharacterController characterController;

    private Vector3 moveDirection;
    public float gravityScale;

    public Transform pivot;
    public float rotateSpeed;
    public Animator animator;

    public GameObject playerModel;
    public float knockbackForce;
    public float knockbackTime;
    private float knockbackCounter;


    // Start is called before the first frame update
    void Start() {
       
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (knockbackCounter <= 0) {
            // getting the forward position of player and moving it forth and back
            if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.15  && Mathf.Abs(Input.GetAxis("Vertical")) < 0.15) {
            moveDirection = new Vector3(0,0,0);
            } else {
                float yTemp = moveDirection.y;
                moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
                moveDirection = moveDirection.normalized * moveSpeed;
                moveDirection.y = yTemp;
            }
            

            if (characterController.isGrounded) {
                moveDirection.y = 0f;

                if (Input.GetButtonDown("Jump")) {
                    moveDirection.y = jumpForce;
                }
            }
        } else {
            knockbackCounter -= Time.deltaTime;
        }

        // adding gravity 
        moveDirection.y += (Physics.gravity.y * gravityScale * Time.deltaTime);
        // telling characterController to move based on the vector3 we created
        characterController.Move(moveDirection * Time.deltaTime);

        // set up animation 
        animator.SetBool("isGrounded", characterController.isGrounded);
        animator.SetFloat("speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));

        //moving the player based on where camera is looking 
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        } 

        


    }

    public void knockback(Vector3 direction) {
        knockbackCounter = knockbackTime;

        moveDirection = direction * knockbackForce;
        moveDirection.y = knockbackForce;
    }

}
