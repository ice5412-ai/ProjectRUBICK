using Photon.Pun;
using UnityEngine;

public class PunParticle : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!photonView.IsMine)
            return;
        Destroy(this.gameObject, 1f);
    }
    private void OnDestroy()
    {
        if (!photonView.IsMine)
            return;

        PhotonNetwork.Destroy(this.gameObject);
    }
}
