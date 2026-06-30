using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine.Serialization;

public class AttackController : MonoBehaviourPun, IPunObservable
{
    
    //[SerializeField] private Weapons weapon;
    [Header("Objects")] [Tooltip("Use in FollowCursor.")]
    public GameObject weaponHolder;

    [SerializeField] private GameObject playerObj;
    Vector2 aimDirection;
    float aimAngle;
    private Vector2 mousePos;
    private Rigidbody2D rb;
    [SerializeField] private Camera cam;
    public LayerMask mouseAimMask;
    private Transform tragetTransform;
    [SerializeField] private Animator animator;

    [Tooltip("Attack Collider.")] 
    public BoxCollider2D attackCollider;

    [Space] 
    public ItemType itemType;
    [SerializeField] private float waitTime;
    [SerializeField] private bool isAttack;
    public WeaponStat weaponStat;

    private float _hideAttackTime = 0.2f;

    private bool Animating;

    #region Audio

    [Header("Sounds")] 
    public AudioSource audioSource;
    public AudioClip shotgunShoot;
    public AudioClip pistolShoot;
    public AudioClip rifleShoot;
    public AudioClip sniperShoot;
    public AudioClip bat;
    public AudioClip grab;

    private const byte Shotgun = 0;
    private const byte Pistol = 1;
    private const byte Rifle = 2;
    private const byte Sniper = 3;
    private const byte Bat = 4;
    private const byte Grab = 5;

    #endregion
    
    void Start()
    {
        tragetTransform = GameObject.Find("Crosshair").transform;
        cam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody2D>();
        attackCollider.enabled = false;
        isAttack = false;
        waitTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        TimeUpdate();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        itemType = weaponStat.itemType;

        if (itemType == ItemType.Bat
            || itemType == ItemType.Hammer)
        {
            animator.SetBool(AnimationKey.IsMelee, true);
            animator.SetBool(AnimationKey.IsEquipGun, false);
            animator.SetBool(AnimationKey.IsPistol, false);
            if (Input.GetMouseButtonDown(0))
                Attack();
            animator.SetBool(AnimationKey.MeleeAttack, Animating);
            
        }

        if (itemType == ItemType.Pistol
        )
        {
            animator.SetBool(AnimationKey.IsMelee, false);
            animator.SetBool(AnimationKey.IsFiring, false);
            animator.SetBool(AnimationKey.IsEquipGun, true);
            animator.SetBool(AnimationKey.IsPistol, true);
            if (Input.GetMouseButtonDown(0))
                Attack();
            animator.SetBool(AnimationKey.IsFiringPistol, Animating);
        }
        if ( itemType == ItemType.Sniper || itemType == ItemType.ShotGun)
        {
            animator.SetBool(AnimationKey.IsMelee, false);
            animator.SetBool(AnimationKey.IsFiringPistol, false);
            animator.SetBool(AnimationKey.IsEquipGun, true);
            animator.SetBool(AnimationKey.IsPistol, false);
            if (Input.GetMouseButtonDown(0))
                Attack();
            animator.SetBool(AnimationKey.IsFiring, Animating);
        }

        if (itemType == ItemType.MachineGun)
        {
            Animating = false;
            animator.SetBool(AnimationKey.IsMelee, false);
            animator.SetBool(AnimationKey.IsFiringPistol, false);
            animator.SetBool(AnimationKey.IsEquipGun, true);
            animator.SetBool(AnimationKey.IsPistol, false);
            if (Input.GetMouseButton(0))
                Attack();
            animator.SetBool(AnimationKey.IsFiring, Input.GetMouseButton(0));
            
        }
    }

    public void DoneAnimating(bool done)
    {
        Animating = !done;
    }

    //Collect Item
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!photonView.IsMine)
            return;
        if (other.gameObject.CompareTag("Item") && Input.GetKeyDown(KeyCode.F))
        {
            Item item = other.GetComponent<Item>();
            WeaponScript newWeaponScript = item.weaponScript;
            weaponStat.weaponScript = newWeaponScript;
            //Debug.Log("Collected " + newWeaponScript.weaponName);
            weaponStat.ChangeWeapon();
            Destroy(other.gameObject);
            
        }
    }

    private void TimeUpdate()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else
        {
            waitTime = 0;
        }

        //Hide Attack
        if (_hideAttackTime >= 0 && isAttack)
        {
            _hideAttackTime -= Time.deltaTime;
        }

        if (_hideAttackTime <= 0)
        {
            _hideAttackTime = 0.5f;
            attackCollider.enabled = false;
            //attackSprite.enabled = false;
            isAttack = false;
            //weaponSprite.enabled = true;
        }
    }

    private void Attack()
    {
        if (waitTime <= 0)
        {
            Animating = true;
            WeaponFire(playerObj.transform, tragetTransform, weaponStat.atkSpd);
            waitTime = weaponStat.atkSpd;
        }
    }

    private void WeaponFire(Transform PlayerTransform, Transform TragetTransform, float FireRate)
    {
        
        if (itemType == ItemType.Bat || itemType == ItemType.Hammer)//Melee attack
        {
            photonView.RPC("RPCPlaySound", RpcTarget.All, Bat);
            attackCollider.enabled = true;
        }
        else if(itemType == ItemType.Pistol || itemType == ItemType.Sniper || itemType == ItemType.MachineGun)
        {
            object[] data = {photonView.ViewID};
            switch (itemType)
            {
                case ItemType.Pistol: photonView.RPC("RPCPlaySound", RpcTarget.All, Pistol);
                    break;
                case ItemType.Sniper: photonView.RPC("RPCPlaySound", RpcTarget.All, Sniper);
                    break;
                case ItemType.MachineGun: photonView.RPC("RPCPlaySound", RpcTarget.All, Rifle);
                    break;
            }
            
            GameObject bullet = PhotonNetwork.Instantiate(this.weaponStat.bullet.name,
                weaponHolder.transform.position + (weaponHolder.transform.right * 2f),
                weaponHolder.transform.rotation,
                0,
                data);
        }
        else if (itemType == ItemType.ShotGun)
        {
            object[] data = {photonView.ViewID};
            
            GameObject bullet = PhotonNetwork.Instantiate(this.weaponStat.bullet.name,
                weaponHolder.transform.position + (weaponHolder.transform.right * 2f),
                weaponHolder.transform.rotation,
                0,
                data);
            photonView.RPC("RPCPlaySound", RpcTarget.All, Shotgun);
        }
        
        isAttack = true;
        
    }


    private void FollowCursor()
    {
        
        Vector3 holderPos = weaponHolder.transform.position;
        Vector3 dir = Vector3.Normalize(tragetTransform.position - holderPos);
        
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        weaponHolder.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward * 90);
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            tragetTransform.transform.position = hit.point;
        }
        
        Vector2 facingVector2 = Vector2.right;
        // Facing
        if (tragetTransform != null)
        {
            if (tragetTransform.position.x - playerObj.transform.position.x > 0)
            {
                facingVector2 = Vector2.right;
                playerObj.transform.rotation =
                    Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 50);
                
            }
            else
            {
                facingVector2 = Vector2.left;
                playerObj.transform.rotation =
                    Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * 50);
                
                
            }
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        FollowCursor();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(attackCollider.enabled);
        }
        else
        {
            attackCollider.enabled = (bool) stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void RPCPlaySound(byte sound)
    {
        audioSource.PlayOneShot(IndexToAudio(sound));
    }

    private AudioClip IndexToAudio(int index)
    {
        switch (index)
        {
            case 0: return shotgunShoot;
            case 1: return pistolShoot;
            case 2: return rifleShoot;
            case 3: return sniperShoot;
            case 4: return bat;
            case 5: return grab;
        }

        return null;
    }
}