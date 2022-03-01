using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerCamera : MonoBehaviourPunCallbacks {

    public string mouseXInput;
    public string mouseYInput;
    public float mouseSensitivity = 100f;
    PhotonView photonView;
    
    float xRotation = 0f;

    public Transform playerBody; 

    void Awake() {
        photonView = gameObject.GetComponent<PhotonView>();

        if (photonView.IsMine) {
            gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
        }
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void cameraRotation() {
        float mouseX = Input.GetAxis(mouseXInput) * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInput) * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);

        // changing camera rotation on the y-axsis
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
  
    // Update is called once per frame
    void Update() {
         if (photonView.IsMine) {
            cameraRotation();
        }
        //cameraRotation();
        
    }
}
