using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Melee : MonoBehaviour {

    public float damage = 150f;
    public float range = 25f;
    public float fireRate = 10f;

    PhotonView PhotonView;
    playerAnimation playerAnimation;
    public Camera playerCamera;

    private float nextTimeToFire = 0f;


    // Start is called before the first frame update
    void Start() {
        PhotonView = gameObject.transform.root.GetComponent<PhotonView>();
        playerAnimation = gameObject.transform.root.GetComponent<playerAnimation>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Mouse0) && PhotonView.IsMine && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f/fireRate;
            attack();
            
        }
        
    }

    void attack() {
        RaycastHit hitInfo;

        playerAnimation.AnimationChange(4);

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitInfo, range)) {
            Debug.Log("attacking with melee");

        }
    }
}
