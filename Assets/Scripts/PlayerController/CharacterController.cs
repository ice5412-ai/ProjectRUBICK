using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using SimpleJSON;
using TMPro;
using Utilities;
using Random = UnityEngine.Random;

public class CharacterController : MonoBehaviourPunCallbacks, IPunObservable
{
    
    [Header("Name")]
    [SerializeField] private string nameString;
    [SerializeField] private TextMeshProUGUI nameText;
    
    [Space]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 2;
    float inputX = 0;
    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] private GameObject playerObj;
    
    [Tooltip("Is Knockingback")]
    public bool unbalance = false;
    public bool isBlock = false;
    
    [Tooltip("Block WaitTime")]
    [SerializeField] private float waitTime = 0;
    public float blockInterval = 1.0f;
    private float _blockTime = 1.5f;
    public SpriteRenderer blockSprite;
    
    [Header("Weapon")]
    [SerializeField] private WeaponStat weaponStat;
    public WeaponScript[] weaponScriptIndex;
    public WeaponStat selfWeaponStat;
    
    [SerializeField] private BulletStat bulletStat;
    
    private Vector3 _movement;
    private Transform tragetTransform;
    public Player tempKiller;
    
    private List<GameObject> spawnPoint = new List<GameObject>();
    private CharacterController _characterController;
    [Header("Animation")] 
    public Animator animator;

    [Space] public GameObject particle;
    //Log
    [SerializeField] private PlayerLog eventLog;
    [Header("Sounds")] 
    public AudioSource audioSource;
    public AudioClip bonk;
    
    #region MonoBehaviour

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView.RPC("HideBlockSprite", RpcTarget.All);
        tragetTransform = GameObject.Find("Crosshair").transform;
        foreach (GameObject spawn in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            spawnPoint.Add(spawn);
        }
        if (!photonView.IsMine)
        {
            OnPlayerPropertiesUpdate(photonView.Owner, photonView.Owner.CustomProperties);
        }
        animator.SetBool(AnimationKey.Is3D, false);
    }

    private void GetName()
    {
        string path = Application.streamingAssetsPath + "/JsonData/PlayerCharacterName.json";
        string jsonString = File.ReadAllText((path));
        JSONObject charIndex = (JSONObject) JSON.Parse(jsonString);
        
        string nameTemp = charIndex["CustomName"];
        photonView.Owner.NickName = nameTemp;
        nameString = photonView.Owner.NickName;
        SetName();
    }

    private void SetName()
    {
        nameText.text = nameString;
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (this.rb.velocity.x > speed ||
                this.rb.velocity.x < -speed)
            {
                unbalance = true;
            }
            else
            {
                unbalance = false;
            }
            
            ProcessInput();
            TimeCount();
        }
    }
    
    private void ProcessInput()
    {
        
        inputX = Input.GetAxisRaw("Horizontal");
        
        // Normalize
        _movement = new Vector3(inputX, 0, 0).normalized;

        //Block
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Block();
        }
        //Flip();
        
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        photonView.RPC("HideBlockSprite", newPlayer);
    }

    /*private void Flip()
    {
        
        Vector3 playerScale = playerObj.transform.localScale;
        if (_movement.x > 0 && !facingRight)
        {
            facingRight = true;
            playerScale.x = 1;
            playerObj.transform.localScale = playerScale;
        }
        if(_movement.x < 0 && facingRight)
        {
            facingRight = false;
            playerScale.x = -1;
            playerObj.transform.localScale = playerScale;
        }
    }*/


    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (!unbalance)
            Move();
    }

    public void GetKiller(Player player)
    {
        photonView.RPC("RPCGetKiller",photonView.Owner, player);
    }
    
    [PunRPC]
    public void RPCGetKiller(Player player)
    {
        tempKiller = player;
        //Debug.Log(tempKiller.ActorNumber);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!photonView.IsMine)
            return;
        if (other.gameObject.CompareTag("Item") && Input.GetKeyDown(KeyCode.F))
        {
            Item item = other.GetComponent<Item>();
            WeaponScript newWeaponScript = item.weaponScript;
            photonView.RPC("RPCChangeWeaponScript",RpcTarget.All, WeaponScriptToIndex(newWeaponScript));
            selfWeaponStat.weaponScript = newWeaponScript;
            Debug.Log("Collected " + newWeaponScript.weaponName);
            selfWeaponStat.ChangeWeapon();
            item.DestroySelf();
        }
    }

    public int WeaponScriptToIndex(WeaponScript newWeaponScript)
    {
        switch (newWeaponScript.itemType)
        {
            case ItemType.Bat: return 0;
            case ItemType.Hammer: return 1;
            case ItemType.Pistol: return 2;
            case ItemType.Sniper: return 3;
            case ItemType.MachineGun: return 4;
            case ItemType.ShotGun: return 5;
            
        }
        return 0;
    }

    public WeaponScript IndexToWeaponScript(int index)
    {
        switch (index)
        {
            case 0: return weaponScriptIndex[0];
            case 1: return weaponScriptIndex[1];
            case 2: return weaponScriptIndex[2];
            case 3: return weaponScriptIndex[3];
            case 4: return weaponScriptIndex[4];
            case 5: return weaponScriptIndex[5];
        }
        return weaponScriptIndex[0];
    }
    [PunRPC]
    public void RPCChangeWeaponScript(int weaponIndex)
    {
        selfWeaponStat.weaponScript = IndexToWeaponScript(weaponIndex);
        
    }
    //Get Hit!
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine)
            return;
        
        if (other.CompareTag("DeadZone"))
        {
            if (tempKiller != null && !PunNetworkManager.singleton.isGameOver)
            {
                Debug.Log(tempKiller.NickName + "Score +1");
                tempKiller.AddScore(1);
                string killLog =  photonView.Owner.NickName + " is <color=red>killed</color> by " + $"<color=orange>{tempKiller.NickName}</color>";
                photonView.RPC("RPCDeadLog", RpcTarget.MasterClient, killLog);
                tempKiller = null;
            }
            object[] data = {photonView.ViewID};
            GameObject p = PhotonNetwork.Instantiate(particle.name,
                playerObj.transform.position + (Vector3.up * 2.5f),
                playerObj.transform.rotation,
                0,
                data);
            photonView.RPC("PlayerDead", photonView.Owner);
        }
        
        if(isBlock == false)
        {
            if (other.gameObject.CompareTag("Weapon"))
            {
                if (!other.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    photonView.RPC("RPCBonkSound", RpcTarget.All);
                    weaponStat = other.GetComponent<WeaponStat>();
                    float tempForce = weaponStat.attackForce;
                    unbalance = true;
                    rb.AddForce(other.transform.right * tempForce, ForceMode2D.Impulse);
                    
                }
            }

            if (other.gameObject.CompareTag("Bullet"))
            {
                if (!other.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    bulletStat = other.GetComponent<BulletStat>();
                    Debug.Log("BulletHit");
                    float tempForce = bulletStat.attackForce;
                    unbalance = true;
                    rb.AddForce(other.transform.right * tempForce, ForceMode2D.Impulse);
                    
                }
            }
        }
    }
    [PunRPC]
    public void RPCBonkSound()
    {
        audioSource.PlayOneShot(bonk);
    }
    private void TimeCount() //Main Timer
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else
        {
            waitTime = 0;
        }
        
        if (isBlock)
        {
            _blockTime -= Time.deltaTime;
        }

        if (_blockTime <= 0)
        {
            Debug.Log("unblock");
            isBlock = false;
            photonView.RPC("HideBlockSprite", RpcTarget.All);
            //ShowBlockSprite();
            _blockTime = 1.5f;
        }
    }

    //Block
    private void Block()
    {
        if (waitTime <= 0 && !isBlock)
        {
            Debug.Log("block");
            isBlock = true;
            photonView.RPC("ShowBlockSprite", RpcTarget.All);
            //ShowBlockSprite();
            waitTime = blockInterval;
        }
    }
    [PunRPC]
    public void ShowBlockSprite()
    {
        blockSprite.enabled = true;
    }

    [PunRPC]
    public void HideBlockSprite()
    {
        blockSprite.enabled = false;
    }
    private void Move()
    {
        rb.velocity = new Vector3(_movement.x * speed, rb.velocity.y);
        
        Vector2 facingVector2 = Vector2.right;
        // Facing
        if (tragetTransform != null)
        {
            if (tragetTransform.position.x - transform.position.x > 0)
            {
                facingVector2 = Vector2.right;
                animator.SetFloat(AnimationKey.Speed, rb.velocity.x / speed);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 50);
            }
            else
            {
                facingVector2 = Vector2.left;
                animator.SetFloat(AnimationKey.Speed, -rb.velocity.x / speed);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * 50);
            }
        }

        float angle = Vector2.SignedAngle(facingVector2, tragetTransform.position - transform.position);
        if (tragetTransform.position.x - transform.position.x < 0)
        {
            angle *= -1;
        }

        if (angle > 45)
        {
            animator.SetFloat(AnimationKey.Angle, 5);
        }
        else if (angle > 15)
        {
            animator.SetFloat(AnimationKey.Angle, 4);
        }
        else if(angle > 0)
        {
            animator.SetFloat(AnimationKey.Angle, 3);
        }
        else if(angle > -15)
        {
            animator.SetFloat(AnimationKey.Angle, 2);
        }
        else
        {
            animator.SetFloat(AnimationKey.Angle, 1);
        }
    }

    #endregion

    [PunRPC]
    public void PlayerDead()
    {
        var randomSpawn = Random.Range(0, spawnPoint.Count);
        Vector2 direction = spawnPoint[randomSpawn].transform.position;
        this.transform.position = direction;
        rb.velocity = Vector2.zero;
        //Debug.Log("Respawned");
    }

    [PunRPC]
    public void RPCDeadLog(string log)
    {
        Debug.Log(log);
        eventLog.AddEvent(log);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isBlock);
            //stream.SendNext(blockSprite.active);
        }
        else
        {
            isBlock = (bool) stream.ReceiveNext();
            //blockSprite.active = (bool) stream.ReceiveNext();
        }
    }
}