using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
//using Lean.Localization;
using UnityEngine.Audio;

public class UIController : MonoBehaviourPunCallbacks
{
    public enum MenuState
    {
        Start = 0,
        MainMenu = 1,
        Option = 2,
        Credits = 3,
        ModeSelection = 4,
        RoomListing = 5,
        PlayerListing = 6
    }

    [SerializeField] private GameObject[] UIInStart;
    [SerializeField] private GameObject[] UIInMainMenu;
    [SerializeField] private GameObject[] UIInOption;
    [SerializeField] private GameObject[] UIInCredits;
    [SerializeField] private GameObject[] UIInModeSelection;
    [SerializeField] private GameObject[] UIInRoomListing;
    [SerializeField] private GameObject[] UIInPlayerListing;
    [SerializeField] private TMP_Text Version;
    [SerializeField] private Slider[] VolSliders;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private PhotonChatController _chatController;
    private GameObject[] sui;

    [SerializeField] private PlayerListingMenu _playerListingMenu;
    [SerializeField] private RoomListingMenu _roomListingMenu;
    [SerializeField] private GameObject UIBG;
    [SerializeField] private TMP_Text[] ModeText;


    private MenuState _menuState = MenuState.Start;
    public string _gameMode = "";
    public int _map = 0;

    void Start()
    {
        if (_chatController)
        {
            _chatController = FindObjectOfType<PhotonChatController>();
        }
        _audioMixer.GetFloat("MasterVol", out float MasterVol);
        VolSliders[0].value = MasterVol;
        _audioMixer.GetFloat("BGMVol", out float BGMVol);
        VolSliders[1].value = BGMVol;
        _audioMixer.GetFloat("SFXVol", out float SFXVol);
        VolSliders[2].value = SFXVol;
        Version.text = Application.version;
    }

    public void _Shutdown()
    {
        Application.Quit();
    }
    public void _ChangeUIStateButton(int menuState)
    {
        Debug.Log("Loading " + menuState);
        GameObject[][] nsui =
            {UIInStart, UIInMainMenu, UIInOption, UIInCredits, UIInModeSelection, UIInRoomListing, UIInPlayerListing};
        switch ((MenuState) menuState)
        {
            case MenuState.Start:
                State_Start();
                sui = UIInStart;
                break;
            case MenuState.MainMenu:
                State_MainMenu();
                sui = UIInMainMenu;
                break;
            case MenuState.Option:
                State_Option();
                sui = UIInOption;
                break;
            case MenuState.Credits:
                State_Credits();
                sui = UIInCredits;
                break;
            case MenuState.ModeSelection:
                State_ModeSelection();
                sui = UIInModeSelection;
                break;
            case MenuState.RoomListing:
                State_RoomListing();
                sui = UIInRoomListing;
                break;
            case MenuState.PlayerListing:
                State_PlayerListing();
                sui = UIInPlayerListing;
                break;
        }

        foreach (var ui in nsui)
        {
            if (ui != sui)
            {
                foreach (var ssui in ui)
                {
                    ssui.SetActive(false);
                }
            }
            else
            {
                foreach (var ssui in ui)
                {
                    ssui.SetActive(true);
                }
            }
        }
    }
    
    public void _SetGameMode(string gamemode)
    {
        _gameMode = gamemode;
        foreach (var modetext in ModeText)
        {
            modetext.text = _gameMode;
        }
    }

    void State_Start()
    {
        
    }

    void State_MainMenu()
    {
        
    }

    void State_Option()
    {
        
    }

    void State_Credits()
    {
        
    }

    void State_ModeSelection()
    {
        
    }

    void State_RoomListing()
    {
        _roomListingMenu.ForceReloadList();
    }

    void State_PlayerListing()
    {
        SceneManager.LoadSceneAsync("CustomizeScene", LoadSceneMode.Additive);
    }

    public void _LeaveRoom()
    {
        _roomListingMenu.ForceReloadList();
        SceneManager.UnloadSceneAsync("CustomizeScene");
        PhotonNetwork.LeaveRoom(true);
    }

    public void CreateNewRoom()
    {
        if (!PhotonNetwork.IsConnected) return;

        RoomOptions options = new RoomOptions();
        switch (_gameMode)
        {
            case "FREEGROUND":
                options.MaxPlayers = 10;
                options.IsOpen = false;
                options.IsVisible = false;
                break;
            case "PLAYGROUND":
                options.MaxPlayers = 6;
                options.IsOpen = false;
                options.IsVisible = false;
                break;
            default:
                Debug.LogError("_gameMode's string couldn't be read. options.MaxPlayer will be set at 8");
                options.MaxPlayers = 8;
                break;
        }

        options.IsOpen = true;
        options.IsVisible = true;

        options.CustomRoomProperties = new Hashtable();
        options.CustomRoomProperties.Add(GameRoomSetting.GAMEMODE, _gameMode);
        options.CustomRoomProperties.Add(GameRoomSetting.MAP, _map);

        options.CustomRoomPropertiesForLobby = new string[]
        {
            GameRoomSetting.GAMEMODE,
        };

        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        _ChangeUIStateButton(6);
        string tempGamemode = _gameMode;
        _chatController.JoinChat();
    }

    public override void OnLeftRoom()
    {
        _chatController.LeaveChat();
    }
}