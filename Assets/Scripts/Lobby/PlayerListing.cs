using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _text;

    [SerializeField] private Color masterColor;
    public Player Player { get; private set; }

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        _text.text = player.NickName;
        if (player.IsMasterClient)
        {
            _text.color = masterColor;
        }
        else
        {
            _text.color = Color.black;
        }
    }
}
