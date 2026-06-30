using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using SimpleJSON;
using Random = UnityEngine.Random;

public class PunConnecter : MonoBehaviourPunCallbacks
{
    public static bool firstSetting = false;
    [Space] public PhotonChatController ChatController;

    void Start()
    {
        print("Connecting to server.");
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.ConnectUsingSettings();
        
        SetName();
    }

    public void SetName()
    {
        if (!firstSetting)
        {
            string folderPath = Application.streamingAssetsPath + "/JsonData/";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                string pathTemp = Application.streamingAssetsPath + "/JsonData/PlayerCharacterName.json";
                if (!File.Exists(pathTemp))
                {
                    JSONObject characterIndexJson = new JSONObject();
                    characterIndexJson.Add("CustomName", "");
                    File.WriteAllText(folderPath + "PlayerCharacterName.json", characterIndexJson.ToString());
                }
            }
            firstSetting = true;
        }
        
        string path = Application.streamingAssetsPath + "/JsonData/PlayerCharacterName.json";
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText((path));
            JSONObject charIndex = (JSONObject) JSON.Parse(jsonString);
        
            string nameString = charIndex["CustomName"];
        
            if (nameString != String.Empty)
            {
                PhotonNetwork.LocalPlayer.NickName = nameString + "#"+ (Random.Range(0, 9999)).ToString().PadLeft(4,'0');
            }
            else if (nameString == String.Empty)
            {
                PhotonNetwork.LocalPlayer.NickName = "#"+ (Random.Range(0, 9999)).ToString().PadLeft(4,'0');
            }
        }
        //Debug.Log(jsonString.ToString());
    }
    public override void OnConnectedToMaster()
    {
        print("Connected to server. Welcome, " + PhotonNetwork.LocalPlayer.NickName);
        if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
        if (ChatController == null)
        {
            ChatController = FindObjectOfType<PhotonChatController>();
        }
        ChatController.Rename();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from server." + cause.ToString());
    }
}
