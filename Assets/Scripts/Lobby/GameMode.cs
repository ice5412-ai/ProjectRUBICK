using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(menuName =  "NungtalungAR/Photon/GameMode", fileName = "gameMode")]
public class GameMode : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private byte _maxPlayer;

    public string Name
    {
        get { return _name;}
        private set { _name = value; }
    }

    public byte MaxPlayers
    {
        get { return _maxPlayer; }
        private set { _maxPlayer = value; }
    }
}

public class GameRoomSetting
{
    public const string GAMEMODE = "gamemode";
    public const string MAP = "map";
}