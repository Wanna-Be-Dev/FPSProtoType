using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject _player;

    Movement playerController;

    int kills;
    int deaths;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public int ReturnPV()
    {
        return PV.ViewID;
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }
    void CreateController()
    {
        Transform spawnpoint = SpawnManager.instance.GetSpawnPoint();
        _player  = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        if(!_player.GetPhotonView().IsMine)
        {
            _player.GetComponent<Movement>().enabled = false;
            Destroy(_player.GetComponentInChildren<Camera>().gameObject);
        }
        
    }
    public void GetKill()
    {

        PV.RPC(nameof(RPC_GetKill), PV.Owner);
    }
    public void Die()
    {
        PV.RPC(nameof(RPC_GetDeath), PV.Owner);
        PhotonNetwork.Destroy(_player);
        CreateController();
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;
        Debug.Log(PV.ViewID);
        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        
    }

    [PunRPC]
    void RPC_GetDeath()
    {
        deaths++;
        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public static PlayerManager Find(Player player)
    {
        //return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);//not effiectent of find player
        //PlayerManager test = FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
    }

}
