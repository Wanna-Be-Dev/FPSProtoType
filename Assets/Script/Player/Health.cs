using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using TMPro;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Health : MonoBehaviourPunCallbacks
{
    public int health = 100;
    public bool isLocalPlayer;

    [Header("UI")]
    public TMP_Text healthText;

    PlayerManager playerManager;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    [PunRPC]
    public void TakeDamage(int _damage, Player info)
    {
        health -= _damage;
        healthText.text = health.ToString();

        if(health <= 0)
        {
            if(PV.IsMine)
            {
                playerManager.Die();
                PlayerManager.Find(info).GetKill();
                //Debug.Log(info.GetHashCode() + " got kill");
            }     
        }
    }
}
