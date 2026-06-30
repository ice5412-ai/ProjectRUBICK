using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private RoomListing _roomListing;
    [SerializeField] private UIController _uiController;

    private List<RoomListing> _listings = new List<RoomListing>();
    private List<RoomInfo> _infosTemp;

    public void ForceReloadList()
    {
        _content.DestroyChildren();
        _listings.Clear();
        RoomListShowUp(_infosTemp);
    }
    
    public override void OnJoinedRoom()
    {
        _content.DestroyChildren();
        _listings.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {
        _infosTemp = _roomList;
        RoomListShowUp(_roomList);
    }

    private void RoomListShowUp(List<RoomInfo> roomList)
    {
        Debug.Log("RoomListUpdated");
        foreach (RoomInfo info in roomList)
        {
            // Removed
            if (info.RemovedFromList)
            {
                int index = _listings.FindIndex((x => x.RoomInfo.Name == info.Name));
                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            // Added
            else
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    //Debug.Log((string)info.CustomProperties[GameRoomSetting.GAMEMODE]);
                    string _gamemode = (string)info.CustomProperties[GameRoomSetting.GAMEMODE];
                    Debug.Log(_gamemode + " and " + _uiController._gameMode);
                    if (_gamemode == _uiController._gameMode)
                    {
                        RoomListing listing = Instantiate(_roomListing, _content);
                        if (listing != null)
                        {
                            listing.SetRoomInfo(info);
                            _listings.Add(listing);
                        }
                    }
                }
            }
        }
    }
}
