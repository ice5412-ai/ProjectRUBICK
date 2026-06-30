using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;

public class CustomizeCharacterShowcase : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] spritePart;

    [Header("Sprite Options")] public Sprite[] mouthOption;
    public Sprite[] hairOptions;
    public Sprite[] faceOptions;
    public Sprite[] bodyOptions;
    public Sprite[] muscleOptions;
    public Sprite[] topBodyOptions;
    public Sprite[] arm1Options;
    public Sprite[] arm2Options;
    public Sprite[] handOptions;
    public Sprite[] bottomOptions;

    public int[] index;
    private int[] ex_index;

    private void OnEnable()
    {
        GetSkinIndex();

        ex_index = new int[spritePart.Length];
        if (index.Length != ex_index.Length)
        {
            index = new int[spritePart.Length];
        }

        ChangeSkinProperties();
    }

    private void ChangeSkinProperties()
    {
        for (int i = 0; i < spritePart.Length; i++)
        {
            spritePart[i].sprite = ConvertIndexToSprite(i, index[i]);
        }
    }

    public void GetSkinIndex()
    {
        string path = Application.streamingAssetsPath + "/JsonData/PlayerSkinIndex.json";
        string jsonString = File.ReadAllText(path);
        JSONObject playerAppearanceJson = (JSONObject) JSON.Parse(jsonString);
        // index[0] = playerAppearanceJson[SkinPartName._00PLAYER_MOUTH];
        // index[1] = playerAppearanceJson[SkinPartName._01PLAYER_HAIR];
        // index[2] = playerAppearanceJson[SkinPartName._02PLAYER_FACE];
        // index[3] = playerAppearanceJson[SkinPartName._03PLAYER_BODY];
        // index[4] = playerAppearanceJson[SkinPartName._04PLAYER_MUSCLE];
        // index[5] = playerAppearanceJson[SkinPartName._05PLAYER_TOPBODY];
        // index[6] = playerAppearanceJson[SkinPartName._06PLAYER_ARM1];
        // index[7] = playerAppearanceJson[SkinPartName._07PLAYER_ARM2];
        // index[8] = playerAppearanceJson[SkinPartName._08PLAYER_HAND];
        // index[9] = playerAppearanceJson[SkinPartName._09PLAYER_BOTTOM];
    }

    public Sprite ConvertIndexToSprite(int part, int index)
    {
        switch (part)
        {
            case 0:
                return mouthOption[index];
                
            case 1:
                return hairOptions[index];
                
            case 2:
                return faceOptions[index];
                
            case 3:
                return bodyOptions[index];
                
            case 4:
                return muscleOptions[index];
                
            case 5:
                return topBodyOptions[index];
                
            case 6:
                return arm1Options[index];
                
            case 7:
                return arm2Options[index];
                
            case 8:
                return handOptions[index];
                
            case 9:
                return bottomOptions[index];
                
        }

        return null;
    }
}
