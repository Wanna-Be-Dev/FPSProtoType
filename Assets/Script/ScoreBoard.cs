using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using System.ComponentModel;

public class ScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreBoardItemPrefab;
    [SerializeField] CanvasGroup canvasGroup;

    Dictionary<Player, ScoreBoardItem> scoreboardItems = new Dictionary<Player, ScoreBoardItem>();
    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreBoardItem(player);
        }
    }

    void AddScoreBoardItem(Player player)
    {
        ScoreBoardItem item = Instantiate(scoreBoardItemPrefab, container).GetComponent<ScoreBoardItem>();
        item.Initialize(player);
        scoreboardItems[player] = item;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreBoardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }
    void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }
    public void ScoreBoardShow(bool state)
    {
        canvasGroup.alpha = state ? 1 : 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ScoreBoardShow(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            ScoreBoardShow(false);
        }
    }
}
