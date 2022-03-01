using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//Enforces InputField
[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviourPunCallbacks
{
    private const string playerNamePrefKey = "PlayerName";

    void Start () 
    {
        string defaultName = string.Empty;
        InputField inputField = this.GetComponent<InputField>();

        if (inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
            defaultName = PlayerPrefs.GetString(playerNamePrefKey);
            inputField.text = defaultName;
            }
        }
        //Name of the player over network
        PhotonNetwork.NickName = defaultName;
    }
    
    public void SetPlayerName(string value)
    {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(playerNamePrefKey,value);
    }
}
