using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviourPun
{
    private UIController _uiController;

    public void Start()
    {
        _uiController = FindObjectOfType<UIController>();
    }

    public void ChangeScene(string sceneToLoad)
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            PhotonNetwork.LeaveRoom();
            //PhotonNetwork.Disconnect();
        }

        PunUserNetControl.IsPlayerJoined = false;
        PunUserNetControl.LocalPlayerInstance = null;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void MasterLoadLevel()
    {
        if(!PhotonNetwork.IsMasterClient)
            return;
        
        string _gamemode = (string)PhotonNetwork.CurrentRoom.CustomProperties[GameRoomSetting.GAMEMODE];
        switch (_gamemode)
        {
            case "FREEGROUND" : PhotonNetwork.LoadLevel(SceneName.SCENE_FREEGROUND); break;
            case "PLAYGROUND" : PhotonNetwork.LoadLevel(SceneName.SCENE_MINIGAME); break;
        }
        
    }
}
