using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonTransformView))]
public class AttackKnockback : MonoBehaviourPun
{
    private Rigidbody2D rb;

    public float force = 20;
    int OwnerViewID = -1;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // e.g. store this gameobject as this player's charater in Player.TagObject
        info.Sender.TagObject = this.gameObject;
        OwnerViewID = info.photonView.ViewID;
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;
        if (other.gameObject.CompareTag("Player"))
        {
            PunUserNetControl tempOther = other.gameObject.GetComponent<PunUserNetControl>();
            if (tempOther != null)
                Debug.Log("Hit Other ViewID : " + tempOther.photonView.ViewID);
            other.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
        }
        
    }*/
    private void OnDestroy()
    {
        if (!photonView.IsMine)
            return;

        //PhotonView.Destroy(this.gameObject);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
