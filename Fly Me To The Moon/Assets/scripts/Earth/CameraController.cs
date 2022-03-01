using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    public Vector3 offset;

    public bool useOffsetValues;
    public float rotateSpeed; 

    public Transform pivot;
    public bool invertY;

    public float maxViewAngle;
    public float minViewAngle;


    // Start is called before the first frame update
    void Start() {
        if (!useOffsetValues) {
            // creating an offset by our players position minus the camera current positon 
            offset = target.position - transform.position;
        }

        // setting the pivot to target parents and as it child  
        pivot.transform.position = target.transform.position;
        //pivot.parent = target.transform;
        pivot.transform.parent = null;

        Cursor.lockState = CursorLockMode.Locked;        
    }

    // Update is called once per frame
    void LateUpdate() {

        // our pivot will always move with the player   
        pivot.transform.position = target.transform.position;

        // get the x position of the mouse and rotate to target
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        pivot.Rotate(0, horizontal, 0);

        // get the x position of the mouse and rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed; 
        if (invertY) {
            pivot.Rotate(vertical, 0, 0);
        } else {
            pivot.Rotate(-vertical, 0, 0);
        }

        //Limit the camera rotation 
        if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f) {
            pivot.rotation = Quaternion.Euler(maxViewAngle, 0, 0);
        }

        // we do 315 because 360 - 45 = 315 unity counts angle as in a circle -45 is the same as 315 degress
        if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minViewAngle) {
            pivot.rotation = Quaternion.Euler(360f + minViewAngle, 0, 0);
        }

        // rotating camera based on the current rotation and offset 
        float desiredYAngle = pivot.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;
    
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        
        // setting our camera position 
        transform.position = target.position - (rotation * offset);

        if (transform.position.y < target.position.y) {
            transform.position = new Vector3(transform.position.x, target.position.y - 0.5f, transform.position.z);
        }

        transform.LookAt(target);
    }
}
