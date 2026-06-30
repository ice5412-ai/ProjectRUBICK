using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _text;

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        string _gamemode = (string)roomInfo.CustomProperties[GameRoomSetting.GAMEMODE];
        _text.text = roomInfo.PlayerCount +"/"+ roomInfo.MaxPlayers + ", " + roomInfo.Name +" ["+ _gamemode+"]";
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
}
