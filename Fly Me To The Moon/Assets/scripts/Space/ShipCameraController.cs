using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCameraController : MonoBehaviour
{
    public GameObject target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float speed;
    public float minFov;
    public float maxFov;
    public float sensitivity;

    private Quaternion origRotation;
    private Vector3 origPosition;
    private bool returnedRotation;
    private bool notRightClick;

    private void Start()
    {
        origRotation = transform.localRotation;
        origPosition = transform.localPosition;
        returnedRotation = true;
        notRightClick = true;
    }
    void FixedUpdate()
    {
        if (notRightClick == true)
        {
            Vector3 desiredPosition = target.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(target.transform);
        }
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(target.transform.position, transform.up, Input.GetAxis("Mouse X") * speed);
            transform.RotateAround(target.transform.position, transform.right, Input.GetAxis("Mouse Y") * -speed);
            notRightClick = false;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            returnedRotation = false;
            notRightClick = true;
        }

        if (returnedRotation == false)
        {
            transform.localRotation = origRotation;
            transform.localPosition = origPosition;
            returnedRotation = true;
        }

        float fov = Camera.main.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * -sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }
}
