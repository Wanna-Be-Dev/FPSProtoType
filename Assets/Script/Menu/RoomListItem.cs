using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;

    public RoomInfo roomInfo;

    public void SetUp(RoomInfo _roomInfo)
    {
        roomInfo = _roomInfo;
        text.text = _roomInfo.Name;
    }

    public void OnClickToJoin()
    {
        Launcher.Instance.JoinRoom(roomInfo);

    }
}
