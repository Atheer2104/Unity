using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerLobby : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject quickStartButton;
    [SerializeField]
    GameObject quickCancelButton;
    [SerializeField]
    int roomSize;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        quickStartButton.SetActive(true);
    }

    public void QuickStart()
    {
        //Enables and disables button and attempts to connect
        quickStartButton.SetActive(false);
        quickCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        print("starting");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //If fails to connect to a room, that means that there is no room
        CreateRoom();   
    }

    void CreateRoom()
    {
        //Randomised room number
        int randomRoomNumber = Random.Range(0, 1000);
        print(randomRoomNumber);

        //Options for room
        RoomOptions options = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSize
        };
        //Creates room
        Debug.Log("Creating room");
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, options);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create a room");
        //If the room name is the same (1 in 1000 chance), attempt to create a new room
        CreateRoom();
    }

    public void QuickCancel()
    {
        quickCancelButton.SetActive(false);
        quickStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
