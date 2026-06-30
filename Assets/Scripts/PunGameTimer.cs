using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class PunGameTimer : MonoBehaviourPunCallbacks {
    /// <summary>
    /// OnCountdownTimerHasExpired delegate.
    /// </summary>
    public delegate void CountdownTimerHasExpired();

    /// <summary>
    /// Called when the timer has expired.
    /// </summary>
    public static event CountdownTimerHasExpired OnCountdownTimerHasExpired;

    public bool isTimerRunning;

    public float startTime;

    [Header("Reference to a Text component for visualizing the countdown")]
    public TextMeshProUGUI Text;

    [Header("Countdown time in seconds")]
    public float Countdown = 60f;
    public float currentCountDown;

    public void Start() {
        if (Text == null) {
            Debug.LogError("Reference to 'Text' is not set. Please set a valid reference.", this);
            return;
        }
        //Add Delegate Function
        //StartTime();
        
        PunNetworkManager.OnFirstSetting += StartTime;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
    }

    public void Update() {

        if (!isTimerRunning)
            return;

        float timer = (float)PhotonNetwork.Time - startTime;
        currentCountDown = Countdown - timer;

        Text.text = "Time : " + CovertformatTime(currentCountDown);

        //Timeout Logic
        if (currentCountDown > 0.0f)
            return;

        isTimerRunning = false;

        Text.text = string.Empty;

        if (OnCountdownTimerHasExpired != null)  {
            OnCountdownTimerHasExpired();
        }
    }

    private void OnCountdownTimerIsExpired()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;

        Debug.Log("Game is Over? or TimeOut : " + currentCountDown);

        Hashtable props = new Hashtable
        {
            {PunGameSetting.TIMEOUT, true}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    /// <summary>
    /// Static Method to call Start Game Time
    /// </summary>
    public void StartTime() {
        Debug.Log("StartTime");
        Hashtable props = new Hashtable {
            {PunGameSetting.STARTGAMETIME, (float) PhotonNetwork.Time}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    public void GetStartTime(Hashtable propertiesThatChanged) {
        object startTimeFromProps;
        //Debug.Log("StartTime : " + PhotonNetwork.CurrentRoom.CustomProperties[PunGameSetting.START_GAMETIME].ToString());
        if (propertiesThatChanged.TryGetValue(PunGameSetting.STARTGAMETIME, out startTimeFromProps)) {
            //Debug.Log("GetStartTime Prop is : " + startTimeFromProps);
            isTimerRunning = true;
            startTime = (float)startTimeFromProps;
        }
    }

    #region Photon CallBack

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        GetStartTime(PhotonNetwork.CurrentRoom.CustomProperties);
    }

    /// <summary>
    /// Photon Room Properties Update
    /// </summary>
    /// <param name="propertiesThatChanged"></param>
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        GetStartTime(propertiesThatChanged);
    }

    #endregion

    //Uility Method
    string CovertformatTime(float seconds)
    {
        double hh = Math.Floor(seconds / 3600),
          mm = Math.Floor(seconds / 60) % 60,
          ss = Math.Floor(seconds) % 60;
        return mm.ToString("00") + ":" + ss.ToString("00");
    }
}
