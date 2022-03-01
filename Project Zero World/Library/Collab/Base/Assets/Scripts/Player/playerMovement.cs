using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerMovement : MonoBehaviourPunCallbacks{
    
    float verticalInput;
    float horizontalInput;
    public float movementSpeed;

    playerAnimation playerAnimation;
    float walkSpeed;
    PhotonView photonView;

    public float slopeForce;
    public float slopeForceRayLength;

    CharacterController characterController;

    public AnimationCurve jumpingFalloff;
    public float jumpMultiplier;
    bool isJumping;

    // Start is called before the first frame update
    void Start() {

        photonView = gameObject.GetComponent<PhotonView>();
        characterController = gameObject.GetComponent<CharacterController>();
        playerAnimation = gameObject.GetComponent<playerAnimation>();
        walkSpeed = playerAnimation.animator.GetFloat("WalkSpeed");
        
    }

    void playerMove() {
        
        Vector3 forwardMovement = transform.forward * verticalInput;
        Vector3 rightMovemnet = transform.right * horizontalInput;
    

        characterController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovemnet, 1f) * movementSpeed);
        // walking animation
        
        playerAnimation.AnimationChange(1);
    
        if (verticalInput < 0) {
            playerAnimation.animator.SetFloat("WalkSpeed", -walkSpeed);
        } else {
            playerAnimation.animator.SetFloat("WalkSpeed", walkSpeed);
        }

        // if we are moving and on a slope
        if ((verticalInput != 0 || horizontalInput != 0) && onSlope()) {
            characterController.Move(Vector3.down * characterController.height/2 * slopeForce * Time.deltaTime);
        }
        
    }

    bool onSlope() {
        if (isJumping) {
            return false;
        }

        RaycastHit hit; 

        if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height / 2 * slopeForceRayLength)) {
            if (hit.normal != Vector3.up) {
                return true;
            }
        }

        return false;
    }

    void jumpInput() {
        if(Input.GetKeyDown(KeyCode.Space) && !isJumping) {
            isJumping = true;
            playerAnimation.AnimationChange(3);
            StartCoroutine(jumpEvent());
        }
    } 

    void playerStop() {
        characterController.Move(new Vector3(0,0,0));
        playerAnimation.AnimationChange(0);
    }

    private IEnumerator jumpEvent() {

        // changing slopeLimit so we can jump over things 
        characterController.slopeLimit = 90f;
        float timeInAir = 0f;

        do {
            float jumpForce = jumpingFalloff.Evaluate(timeInAir);
            characterController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;

            yield return null;
            // characterController.collisionFlags != CollisionFlags.Above 
            // if we hit something above us instead of keeping adding jump force we will start falling
        } while (!characterController.isGrounded && characterController.collisionFlags != CollisionFlags.Above);
        
        // default value of slopelimit
        characterController.slopeLimit = 45f;
        isJumping = false;
        
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (photonView.IsMine) {
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");

            if (verticalInput == 0 && horizontalInput == 0) {
                playerStop();
                jumpInput();
            } else {
                // photonView.isMine viktigt att kolla med input och jump
                playerMove();
            } 
        }           
    }
}
