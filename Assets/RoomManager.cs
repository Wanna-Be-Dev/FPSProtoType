using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject player;
    [Space] 
    public Transform spawnpoint;
    void Start()
    {
        Debug.Log("Conneting....");

        PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Has connected to server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PhotonNetwork.JoinOrCreateRoom("test", null, null);

        Debug.Log("Joined a Lobby");
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Joined a Room");

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnpoint.position, Quaternion.identity/*spawnpoint.rotation*/);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
    }
}
