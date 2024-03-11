using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
   /* public static RoomManager instance;

    public GameObject player;

    [Space] 
    public Transform[] spawnpoints;

    [Space]

    public GameObject roomCam;

    [Header("Screens")]
    public GameObject nameUI;
    public GameObject LoadingUI;

    private string nickName;

    private void Awake()
    {
        instance = this;
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

        roomCam.SetActive(false);

        SpawnPlayer();
    }

    public void ChangeNickName(string _name)
    {
        if (_name != null)
        {
            PhotonNetwork.NickName = _name;
        }
        else
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 10000).ToString("000000");
        }
    }

    public void JoinRoomButton()
    {
        Debug.Log("Conneting....");

        nameUI.SetActive(false);
        LoadingUI.SetActive(true);

        PhotonNetwork.ConnectUsingSettings();  
    }

    public void SpawnPlayer()
    {
        Transform spawnpoint = spawnpoints[UnityEngine.Random.Range(0, spawnpoints.Length)];

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnpoint.position, Quaternion.identity*//*spawnpoint.rotation*//*);
        _player.GetComponent<PlayerManager>().IsLocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;
        Debug.Log("player:" + PhotonNetwork.LocalPlayer.NickName + " has spawned!");
    }*/
}
