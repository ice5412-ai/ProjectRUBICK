using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerLog : MonoBehaviourPun
{
    [Header("Event Log")]
    private List<string> EventLog = new List<string>();
    public string guiText = "";
    public int maxLines = 3;
    public void AddEvent(string eventString)
    {
        EventLog.Add(eventString);

        guiText = PunNetworkManager.singleton.eventLogString;

        if (EventLog.Count >= maxLines)
        {
            EventLog.RemoveAt(0);
            guiText = "";
        }
            
        
        foreach (string logEvent in EventLog)
        {
            guiText += logEvent;
            guiText += "\n";
        }
        
        Hashtable roomCustonProps = new Hashtable
        {
            //some props
            {PunGameSetting.EVENTLOG, guiText}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomCustonProps);
        
        //photonView.RPC("RPCAddEvent", RpcTarget.All, eventString);
    }

    [PunRPC]
    public void RPCAddEvent(string eventString)
    {
        
    }
    
}
