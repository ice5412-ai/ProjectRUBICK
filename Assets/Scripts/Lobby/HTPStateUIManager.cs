using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTPStateUIManager : MonoBehaviour
{
    [SerializeField] private GameObject ObjectToSwitch;
    [SerializeField] private GameObject[] ObjectsList;
    [SerializeField] private UIController _uiController;
    
    private void OnEnable()
    {
        switch (_uiController._gameMode)
        {
            case "FREESTYLE": 
                ObjectsList[0].SetActive(true);
                ObjectsList[1].SetActive(false);
                ObjectsList[2].SetActive(false);break;
            case "MULTIPLAYER": 
                ObjectsList[0].SetActive(false);
                ObjectsList[1].SetActive(true);
                ObjectsList[2].SetActive(false);break;
            case "COOP": 
                ObjectsList[0].SetActive(false);
                ObjectsList[1].SetActive(false);
                ObjectsList[2].SetActive(true);break;
            default: Debug.LogError("_gameMode's string couldn't be read. _ObjectToSwitch will not be changed."); break;
        }
    }
}
