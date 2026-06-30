using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;

public class CurrentCharacterShowcase : MonoBehaviour
{
    [SerializeField] private SpriteRenderer Original;
    [SerializeField] private GameObject tengCustomize;

    [SerializeField] private Sprite[] characterOptions;

    private int index = 0;
    
    private void OnEnable()
    {
        string path = Application.streamingAssetsPath + "/JsonData/PlayerCharacterIndex.json";
        string jsonString = File.ReadAllText((path));
        JSONObject charIndex = (JSONObject) JSON.Parse(jsonString);
        index = charIndex["CharIndex"];
        
        Original.gameObject.SetActive(index != 11);
        tengCustomize.SetActive(index == 11);
        
        Original.sprite = characterOptions[index];
    }
}
