using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [Header("MainMenu")]
    [SerializeField] TMP_InputField usernameInput;
    
    [Header("Create Room")]
    [SerializeField] TMP_InputField CreateRoomNameInputField;

    [Header("Find Room")]
    [SerializeField] Transform roomListContent;
    [Space]
    [SerializeField] GameObject roomListItemPrefab;

    [Header("Joined Room")]
    [SerializeField] GameObject roomJoined;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform playerListContent;
    [Space]
    [SerializeField] GameObject playerListItemPrefab;
    [Space]
    [SerializeField] GameObject startGameBttn;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void UserNameValueChanged() /// enter username 
    {
        //PhotonNetwork.NickName = "Player " + Random.Range(0, 10000).ToString("000000");
        if (usernameInput != null)
        {
            PhotonNetwork.NickName = usernameInput.text;
        }
        else
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 10000).ToString("000000");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Connected To Lobby");
        MenuManager.instance.loading(false);
        PhotonNetwork.NickName = "Player " + Random.Range(0, 10000).ToString("000000");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        // ErrorPresenter.Show("Lobby failed to create: " + message);
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(CreateRoomNameInputField.text))
            return;
        PhotonNetwork.CreateRoom(CreateRoomNameInputField.text);// enter "" for random string
        MenuManager.instance.loading(true);
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        MenuManager.instance.loading(true);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.loading(true);
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        MenuManager.instance.loading(false);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.instance.OpenMenu(roomJoined.GetComponent<Menu>());
        MenuManager.instance.loading(false);


        Player[] players = PhotonNetwork.PlayerList;

        //bug: players are not deleted if left lobby
        //fix: clear room list
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        //bttn vivsible for host only
        startGameBttn.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameBttn.SetActive(PhotonNetwork.IsMasterClient);
    }
   

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //destroy previous room list incase lobby deleted
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
