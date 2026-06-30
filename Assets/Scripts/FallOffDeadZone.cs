using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class FallOffDeadZone : MonoBehaviourPun
{
    public List<GameObject> spawnPoint = new List<GameObject>();
    private CharacterController _characterController;
    private void Start()
    {
        foreach (GameObject spawn in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            spawnPoint.Add(spawn);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine)
            return;
        if (other.CompareTag("DeadZone"))
        {
            photonView.RPC("PlayerDead", photonView.Owner);
        }
    }

    [PunRPC]
    public void PlayerDead()
    {
        var randomSpawn = Random.Range(0, spawnPoint.Count);
        Vector2 direction = spawnPoint[randomSpawn].transform.position;
        this.transform.position = direction;
        Debug.Log("Respawned");
    }
}
