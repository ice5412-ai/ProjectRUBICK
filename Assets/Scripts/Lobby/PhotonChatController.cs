using System;
using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using AuthenticationValues = Photon.Chat.AuthenticationValues;

public class PhotonChatController : MonoBehaviour, IChatClientListener
{
    string UserID;
    public ChatClient _chatClient;
    [SerializeField] private TMP_InputField inputMes;
    [SerializeField] private TMP_Text msArea;
    private string CurrentChatRoom;
    private bool InChatRoom = false;
    private ChatBalloon[] _chatBalloonsList;

    private void Start()
    {
        Connect();
    }

    private void Update()
    {
        _chatClient.Service();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMsg();
        }
    }

    public void Rename()
    {
        LeaveChat();
        _chatClient.Disconnect();
        UserID = PhotonNetwork.LocalPlayer.NickName;
        Debug.Log("Renamed UserID : " + UserID);
        Connect();
    }

    private void Connect()
    {
        _chatClient = new ChatClient(this);
        _chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion,
            new AuthenticationValues(UserID));
    }

    IEnumerator TimeForChangingName()
    {
        yield return new WaitForSeconds(1);
        if (PhotonNetwork.InRoom)
        {
            JoinChat();
            Debug.Log("TimeForChangingName");
        }
    }

    public void JoinChat()
    {
        if (msArea == null)
        {
            msArea = GameObject.Find("Text_(TMP)_XD").GetComponent<TMP_Text>();
        }

        if (inputMes == null)
        {
            inputMes = GameObject.Find("pl_InputField_(TMP)_XD").GetComponent<TMP_InputField>();
        }
        _chatClient.Subscribe(new string[] {PhotonNetwork.CurrentRoom.Name});
        _chatClient.SetOnlineStatus(ChatUserStatus.Online);
        CurrentChatRoom = PhotonNetwork.CurrentRoom.Name;
        msArea.text = "";
        msArea.color = Color.black;
        InChatRoom = true;
    }

    public void LeaveChat()
    {
        if (InChatRoom)
        {
            _chatClient.Unsubscribe(new string[] {CurrentChatRoom});
            _chatClient.SetOnlineStatus(ChatUserStatus.Offline);
            if (msArea != null)
            {
                msArea.text = "Loading...";
                msArea.color = Color.red;
            }
        }
        InChatRoom = false;
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    public void OnDisconnected()
    {
        Debug.Log("You have disconnected from the Photon Chat.");
    }

    public void OnConnected()
    {
        Debug.Log("You have connected to the Photon Chat.");
        StartCoroutine(TimeForChangingName());
    }

    public void SendMsg()
    {
        if (InChatRoom)
        {
            if (inputMes.text != String.Empty)
            {
                _chatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, inputMes.text);
                inputMes.text = String.Empty;
            }
            inputMes.Select();
            inputMes.ActivateInputField();
        }
    }

    public void OnChatStateChange(ChatState state)
    {
        
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            if(msArea!=null){msArea.text += senders[i] + ": " +messages[i] + "\n";}
            //Debug.Log(senders[i]);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {

    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
         Debug.Log($"Subscribed to chatroom : {PhotonNetwork.CurrentRoom.Name}");
        foreach (var channel in channels)
        {
            this._chatClient.PublishMessage(channel, "has joined.");
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log($"Unsubscribed to chatroom : {CurrentChatRoom}");
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnUserSubscribed(string channel, string user)
    {
        
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        _chatClient.PublishMessage(channel, "has left.");
    }

    private void OnDestroy()
    {
        _chatClient.Disconnect();
    }
}
