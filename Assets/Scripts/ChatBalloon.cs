using System;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChatBalloon : MonoBehaviourPun
{
    private ChatClient _chatClient;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text BalloonText;
    [SerializeField] private Animator fadeAnim;
    public string tempMsg;

    private void Start()
    {
        if(!photonView.IsMine) return;
        inputField = GameObject.Find("inputMes").GetComponent<TMP_InputField>();
        
    }

    private void Update()
    {
        if(!photonView.IsMine) return;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (inputField.text != String.Empty)
            {
                BalloonPopup(inputField.text);
                inputField.text = String.Empty;
            }
            else
            {
                inputField.Select();
                inputField.ActivateInputField();
            }
        }
    }

    public void BalloonPopup(string message)
    {
        //Debug.Log("got message");
        tempMsg = message;
        photonView.RPC("RPCBalloonPopup",RpcTarget.AllViaServer, tempMsg);
    }

    [PunRPC]
    public void RPCBalloonPopup(string message)
    {
        //fadeAnim.SetTrigger("Active");
        fadeAnim.Play("BalloonAnim",  -1, 0f);
        BalloonText.text = message;
    }
}
