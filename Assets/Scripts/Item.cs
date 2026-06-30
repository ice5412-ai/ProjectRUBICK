using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Item : MonoBehaviourPun
{
    public WeaponScript weaponScript;
    public ItemType itemTypes;
    private Sprite _sprite;
    public AudioClip grab;
    [SerializeField]private SpriteRenderer image;

    private void Start()
    {

        itemTypes = weaponScript.itemType;
        _sprite = weaponScript.art;
        image.sprite = _sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine)
            return;
        if (other.gameObject.CompareTag("Item"))
        {
            Destroy(other.gameObject);
        }
    }

    public void DestroySelf()
    {
        SoundManagerSingleton.Instance.PlaySFX(grab);
        //audioSource.PlayOneShot(grab);
        photonView.RPC("RPCDestroySelf", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void RPCDestroySelf()
    {
        Destroy(this.gameObject);
    }
    private void OnDestroy()
    {
        //Debug.Log("Item Destroy");
        
        if (!photonView.IsMine)
            return;
        //PhotonView.Destroy(this.gameObject);
        
        PhotonNetwork.Destroy(this.gameObject);
    }
}
