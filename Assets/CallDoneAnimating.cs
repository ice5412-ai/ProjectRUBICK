using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallDoneAnimating : MonoBehaviour
{
    public AttackController attackController;

    public void Sent()
    {
        attackController.DoneAnimating(true);
    }
}
