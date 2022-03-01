using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class GameSetup : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Transform spawnPoint;

    void Start()
    {
        //playerPrefab.GetComponentInChildren<Text>().text = PhotonNetwork.NickName;
        CreatePlayer();
    }

    public void CreatePlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Resources.Load("");
            PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoint.position, Quaternion.identity);
        }
    }
}
