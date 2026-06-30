using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PunNetworkManager : ConnectAndJoinRandom 
{
    public static PunNetworkManager singleton;

    [Header("Spawn Info")]
    [Tooltip("The prefab to use for representing the player")]
    public GameObject GamePlayerPrefab;
    
    public bool isGameStart = false;
    public bool isFirstSetting = false;
    public bool isGameOver = false;
    
    float m_count = 0;
    public float itemDropCount = 10;

    [Header("GameManager")]
    public Camera sceneCamera;
    public GameObject[] itemPrefab;
    public int numberOfItem = 5;
    public GameObject[] itemSpawnPoint;
    public Transform[] spawnPoint;
    
    [Header("UI")]
    public GameObject endScreenPanel;
    public GameObject leaveCanvas;
    public string eventLogString = "";
    [SerializeField] private ScoreboardOverview scoreboardOverview;
    [SerializeField] private TextMeshProUGUI eventTMP;
    [SerializeField] private Animator uiFadeAnimator;
    private bool isESC = false;
    public delegate void PlayerSpawned();
    public static event PlayerSpawned OnPlayerSpawned;

    public delegate void FirstSetting();
    public static event FirstSetting OnFirstSetting;

    private void Awake()
    {
        if (singleton)
        {
            DestroyImmediate(gameObject);
            return;
        }
        singleton = this;
        OnPlayerSpawned += SpawnPlayer;
        OnFirstSetting += FirstRoomSetting;
        endScreenPanel.SetActive(false);
        if (PhotonNetwork.IsConnected)
        {
            if (sceneCamera != null)
                sceneCamera.gameObject.SetActive(false);
            if (PunUserNetControl.LocalPlayerInstance is null)
                OnPlayerSpawned();
            if (!isGameStart)
            {
                isGameStart = true;
                OnFirstSetting();
            }
            Debug.Log("PhotonNetwork IsConnected");
        }
        
    }

    private void OnDestroy()
    {
        OnPlayerSpawned -= SpawnPlayer;
        OnFirstSetting -= FirstRoomSetting;
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
        Debug.Log("New Player. " + newPlayer.ToString());
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if(sceneCamera != null)
            sceneCamera.gameObject.SetActive(false);

        if (PunUserNetControl.LocalPlayerInstance == null)
        {
            Debug.Log("We are Instantiating LocalPlayer from " + SceneManagerHelper.ActiveSceneName);
            PunNetworkManager.singleton.SpawnPlayer();
        }
        else
        {
            Debug.Log("Ignoring scene load for " + SceneManagerHelper.ActiveSceneName);
        }

        //PhotonNetwork.CurrentRoom.CustomProperties
        //PhotonNetwork.CurrentRoom.Players[0].CustomProperties
    }

    public override void OnEnable() {
        base.OnEnable();
        // Raise Event
        PhotonNetwork.AddCallbackTarget(this);

        // LoadBalancingClient.EventReceived
        // The second way to receive custom events is to register a method
        //PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable() {
        base.OnDisable();
        // Raise Event
        PhotonNetwork.RemoveCallbackTarget(this);

        // LoadBalancingClient.EventReceived
        // The second way to receive custom events is to register a method
        //PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    
    public void SpawnPlayer()
    {
        if (PunUserNetControl.LocalPlayerInstance == null)
        {
            int spawnIndex = Random.Range(0, spawnPoint.Length);
            Debug.Log("We are Instantiating LocalPlayer from " + SceneManagerHelper.ActiveSceneName);
            //PunNetworkManager.singleton.SpawnPlayer();
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(GamePlayerPrefab.name,
                spawnPoint[spawnIndex].position,
                Quaternion.identity,
                0);

            isGameStart = true;
        }
        else
        {
            Debug.Log("Ignoring scene load for " + SceneManagerHelper.ActiveSceneName);
        }
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape) && isGameOver)
        {
            LeaveRoom();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) && !isESC && !isGameOver)
        {
            isESC = true;
            leaveCanvas.SetActive(true);
            //SoundManagerSingleton.Instance.PlaySFX(LeavingSFX);
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && isESC && !isGameOver)
        {
            LeaveRoom();
            //SoundManagerSingleton.Instance.PlaySFX(LeavingSFX);
        }
        else if (Input.anyKeyDown && isESC && !isGameOver)
        {
            if (!Input.GetKey(KeyCode.Escape))
            {
                isESC = false;
                leaveCanvas.SetActive(false);
                //SoundManagerSingleton.Instance.PlaySFX(LeavingSFX);
            }
        }

        if (isGameStart)
        {
            if (isGameOver)
            {
                isGameStart = false;
                scoreboardOverview.UpdateScoreboard();
                endScreenPanel.SetActive(true);
            }
        }
        
        if(PhotonNetwork.IsMasterClient != true)
            return;
        if (isGameStart)
        {
            if (isFirstSetting == false)
                OnFirstSetting();
            if (isFirstSetting == true)
            {
                ItemDrop();
            }
            else ItemDrop();
        }
        
    }
    
    public void LeaveRoom()
    {
        //ChatController.LeaveChat();
        PhotonNetwork.AutomaticallySyncScene = false;
        PunUserNetControl.IsPlayerJoined = false;
        PunUserNetControl.LocalPlayerInstance = null;
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            PhotonNetwork.LeaveRoom();
            //PhotonNetwork.Disconnect();
            
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        StartCoroutine(Disconnect());
        PhotonNetwork.Disconnect();
    }

    public IEnumerator Disconnect()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("Disconnected");
            PhotonNetwork.Disconnect();
        }
    }

    private void FirstRoomSetting()
    {
        isFirstSetting = true;
        Hashtable roomCustonProps = new Hashtable
        {
            //some props
            {PunGameSetting.EVENTLOG, ""}
        };
        
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomCustonProps);
        m_count = itemDropCount;
    }
    
    private void ItemDrop()
    {
        int spawnIndex = Random.Range(0, itemSpawnPoint.Length);
        int itemIndex = Random.Range(0, itemPrefab.Length);
        if (GameObject.FindGameObjectsWithTag("Item").Length <
            numberOfItem)
        {
            m_count -= Time.deltaTime;

            if (m_count <= 0)
            {
                m_count = itemDropCount;
                PhotonNetwork.InstantiateRoomObject(itemPrefab[itemIndex].name
                    , itemSpawnPoint[spawnIndex].transform.position
                    , itemSpawnPoint[spawnIndex].transform.rotation
                    , 0);
            }
        }
    }
    
    public override void OnDisconnected(DisconnectCause cause) {
        base.OnDisconnected(cause);
        SceneManager.LoadScene(SceneName.SCENE_MENU);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        if (propertiesThatChanged.ContainsKey(PunGameSetting.TIMEOUT))
        {
            TimeOut(propertiesThatChanged);
        }

        if (propertiesThatChanged.ContainsKey(PunGameSetting.EVENTLOG))
        {
            UpdateEventLog(propertiesThatChanged);
        }
    }

    private void UpdateEventLog(Hashtable propertiesThatChanged)
    {
        if (eventTMP == null)
            return;
        
        object eventLog;
        if (propertiesThatChanged.TryGetValue(PunGameSetting.EVENTLOG, out eventLog))
        {
            eventLogString = (string) eventLog;
            eventTMP.text = eventLogString;
            uiFadeAnimator.Play("UIFadingAnimation",  -1, 0f);
        }
    }
    private void TimeOut(Hashtable propertiesThatChanged)
    {
        object timeout;
        if (propertiesThatChanged.TryGetValue(PunGameSetting.TIMEOUT, out timeout))
        {
            isGameOver = (bool) timeout;
        }
    }
}
