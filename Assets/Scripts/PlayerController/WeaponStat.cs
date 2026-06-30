using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

[RequireComponent(typeof(PhotonTransformView))]
public class WeaponStat : MonoBehaviourPunCallbacks
{
    public WeaponScript weaponScript;
    public SpriteRenderer weaponSprite;
    public SpriteRenderer attackSprite;
    public ItemType itemType;
    public float attackForce = 20;
    public float atkSpd = 0.5f;
    public float attackDamage;
    public float attackRange;
    public bool isRange = false;
    public GameObject bullet;
    [SerializeField] private TextMeshProUGUI weaponText;
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Trigger Enter");
        if (!photonView.IsMine)
            return;
        if (other.gameObject.CompareTag("Player") && other.gameObject != PunUserNetControl.LocalPlayerInstance)
        {
            PunUserNetControl tempOther = other.gameObject.GetComponent<PunUserNetControl>();
            CharacterController charControl = other.gameObject.GetComponent<CharacterController>();
            charControl.GetKiller(PhotonNetwork.LocalPlayer);
            
            if (tempOther != null)
                Debug.Log("Hit Other ViewID : " + tempOther.photonView.ViewID);
            
        }
    }
    
    private void Start()
    {
        if (!photonView.IsMine)
            return;
        weaponText = GameObject.FindWithTag("WeaponUI").GetComponent<TextMeshProUGUI>();
        ChangeWeapon();
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        photonView.RPC("RPCChangeWeaponSprite", newPlayer);
    }
    
    public void ChangeWeapon()
    {
        photonView.RPC("RPCChangeWeaponSprite", RpcTarget.AllBuffered);
        attackForce = weaponScript.attackForce;
        atkSpd = weaponScript.attackSpd;
        attackDamage = weaponScript.attackDamage;
        attackRange = weaponScript.attackRange;
        isRange = weaponScript.isRange;
        bullet = weaponScript.bullet;
        weaponText.text = "- " + weaponScript.weaponName;
        
    }

    [PunRPC]
    public void RPCChangeWeaponSprite()
    {
        itemType = weaponScript.itemType;
        weaponSprite.sprite = weaponScript.art;
        attackSprite.sprite = weaponScript.art;
    }
}
