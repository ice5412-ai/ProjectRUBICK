using Cinemachine;
using UnityEngine;
using Photon.Pun;
using TMPro;

[RequireComponent(typeof(PhotonTransformView))]
public class PunUserNetControl : MonoBehaviourPunCallbacks , IPunInstantiateMagicCallback
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public GameObject localWeapon;
    public GameObject localPlayer;
    public TextMeshProUGUI nameTMP;
    public Color nameColor;
    public static bool IsPlayerJoined = false;
    public bool is3DMode = false;
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        PlayerSpriteSorting();
        
        Debug.Log(info.photonView.Owner.ToString());
        Debug.Log(info.photonView.ViewID.ToString());
        
        // #Important
        // used in PunNetworkManager.cs
        // : we keep track of the localPlayer instance to prevent instanciation when levels are synchronized
        if (photonView.IsMine)
        {
            localWeapon.tag = "LocalWeapon";
            LocalPlayerInstance = localPlayer;
            localPlayer.tag = "LocalPlayer";
            IsPlayerJoined = true;
            
        }
        else if(!is3DMode)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            GetComponentInChildren<CharacterController>().enabled = false;
            GetComponentInChildren<CinemachineVirtualCamera>().enabled = false;
            //GetComponentInChildren<PlayerLog>().enabled = false;
            GetComponent<AttackController>().enabled = false;
            nameTMP.color = nameColor;
        }
    }
    
    private void PlayerSpriteSorting()
    {
        if (!photonView.IsMine)
        {
            SpriteRenderer[] spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.sortingOrder -= 100 * photonView.ViewID;
            }
        }
    }

}
