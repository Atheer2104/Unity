using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //Connects to master server
        PhotonNetwork.ConnectUsingSettings(); 
    }

    public override void OnConnectedToMaster()
    {
        //Types out server connection
        Debug.Log("Now connected to: " + PhotonNetwork.CloudRegion + " server");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //Types out connection failure
        Debug.LogWarningFormat("Failed to connect", cause);
    }   
}
