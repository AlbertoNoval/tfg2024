using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.XR;
using UnityEngine;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    int maxPlayer = 1;
    public void setPlayerAmount(int amount)
    {
        maxPlayer= amount;
    }

    public void createRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayer;
        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
    }
    public void JoinRoom()
    {

        PhotonNetwork.JoinRoom(joinInput.text);
        //PhotonNetwork.JoinRandomOrCreateRoom(null, 4);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
