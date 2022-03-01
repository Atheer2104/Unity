using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovementCubeTest : MonoBehaviourPunCallbacks
{
    void Start() {
        PhotonView photonView = gameObject.GetComponent<PhotonView>();
    }
    void Update()
    {
        if (photonView.IsMine)
        {
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(20, 0, 0);
        }
                if (Input.GetKey(KeyCode.S))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(-20, 0, 0);
        }
        }
    }
}
