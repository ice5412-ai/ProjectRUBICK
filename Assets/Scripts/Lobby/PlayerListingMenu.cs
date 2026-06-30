using System;
using System.Collections;
using System.Collections.Generic;
//using BallMinigame;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerListingMenu : MonoBehaviourPunCallbacks  
{
    [SerializeField] private Transform _content;
    [SerializeField] private PlayerListing _playerListing;

    //[SerializeField] private GameObject GiantMode;
    //[SerializeField] private RectTransform ScrollView; 

    private List<PlayerListing> _listings = new List<PlayerListing>();

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void ForceReloadList()
    {
        _content.DestroyChildren();
        _listings.Clear();
        GetCurrentRoomPlayer();
    }

    public override void OnLeftRoom()
    {
        _content.DestroyChildren();
        _listings.Clear();
    }

    public override void OnJoinedRoom()
    {
        GetCurrentRoomPlayer();
    }

    private void GetCurrentRoomPlayer()
    {
        if(!PhotonNetwork.InRoom) return;
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
        
        string _gamemode = (string)PhotonNetwork.CurrentRoom.CustomProperties[GameRoomSetting.GAMEMODE];
    }

    private void AddPlayerListing(Player player)
    {
        int index = _listings.FindIndex(x => x.Player == player);

        if (index != -1)
        {
            _listings[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = Instantiate(_playerListing, _content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                _listings.Add(listing);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex((x => x.Player == otherPlayer));
        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    public void StartMatch()
    {
        string _gamemode = (string)PhotonNetwork.CurrentRoom.CustomProperties[GameRoomSetting.GAMEMODE];
        Debug.Log(_gamemode);
        //PhotonNetwork.IsMessageQueueRunning = false;

        switch (_gamemode)
        {
            case "FREEGROUND":
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
                PhotonNetwork.LoadLevel("Freeground");
                break;
            
            case "PLAYGROUND":
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
                PhotonNetwork.LoadLevel("Playground");
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                break;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        GetCurrentRoomPlayer();
    }
}
