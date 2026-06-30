using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]private Transform player;
    [SerializeField] private bool is3D = false;
    public float offset = 5;
    
    // Update is called once per frame
    void Update()
    {
        if (!is3D)
        {
            var position = player.position;
            this.transform.position = new Vector3(position.x, position.y + offset, transform.position.z);
        }
        else
        {
            var position = player.position;
            this.transform.position = new Vector3(position.x, position.y + offset, position.z);
        }
    }
}
