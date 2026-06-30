using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(PhotonRigidbody2DView))]
public class PunBullet : MonoBehaviourPun , IPunInstantiateMagicCallback
{

    public float BulletForce = 20f;
    int OwnerViewID = -1;
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // e.g. store this gameobject as this player's charater in Player.TagObject
        info.Sender.TagObject = this.gameObject;
        OwnerViewID = info.photonView.ViewID;

        //info.sender.TagObject = this.GameObject;
        Rigidbody2D bullet = GetComponent<Rigidbody2D>();
        // Add velocity to the bullet
        bullet.velocity = bullet.transform.right * BulletForce;

        if (!photonView.IsMine)
            return;
        // Destroy the bullet after 5 seconds
        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject != PunUserNetControl.LocalPlayerInstance)
        {
            PunUserNetControl tempOther = collision.gameObject.GetComponent<PunUserNetControl>();
            CharacterController charControl = collision.gameObject.GetComponent<CharacterController>();
            charControl.GetKiller(PhotonNetwork.LocalPlayer);
            
            if (tempOther != null)
                Debug.Log("Hit Other ViewID : " + tempOther.photonView.ViewID);
            
        }
        if (!photonView.IsMine)
            return;
        if(collision.gameObject.CompareTag("LocalPlayer")) return;
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (!photonView.IsMine)
            return;

        //PhotonView.Destroy(this.gameObject);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
