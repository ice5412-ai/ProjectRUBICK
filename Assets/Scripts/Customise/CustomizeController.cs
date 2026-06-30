using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using SimpleJSON;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class CustomizeController : MonoBehaviour
{
    [Header("Sprite Parts")]//Part to change
    public SpriteRenderer[] spritePart;
    public TextMeshProUGUI[] partNumber;
    
    #region SpriteOptions
    [Header("Sprite Options")] 
    public SkinOptions so;
    #endregion
    
    [Header("Name")]
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI nameInputText;
    public string customName = "";

    [Header("Audio")]
    public AudioClip ClickSFX;
    public AudioClip TypingSFX;
    
    [Space]
    public int[] index;
    private int[] ex_index;

    private PhotonChatController ChatController;

    public Button startButton;
    
    private void Start()
    {
        ex_index = new int[spritePart.Length];
        if (index.Length != ex_index.Length)
        {
            index = new int[spritePart.Length];
        }

        ChatController = FindObjectOfType<PhotonChatController>();
        GetSkinIndex();
        ApplyPart();

        startButton.interactable = PhotonNetwork.IsMasterClient;
    }

    public void ApplyName()
    {
        string randTag = "#" + (Random.Range(0, 9999)).ToString().PadLeft(4, '0');
        customName = nameInputText.text;
        nameTxt.text = nameInputText.text + randTag;
        PhotonNetwork.LocalPlayer.NickName = customName + randTag;
        ChatController.Rename();
        SaveName();
        SoundManagerSingleton.Instance.PlaySFX(ClickSFX);
    }
    
    public void GetSkinIndex()
    {
        string path = Application.streamingAssetsPath + "/JsonData/PlayerSkinIndex.json";
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            JSONObject playerAppearanceJson = (JSONObject) JSON.Parse(jsonString);
            index[0] = playerAppearanceJson[SkinPartName._00BODY];
            index[1] = playerAppearanceJson[SkinPartName._01RIGHT_UPPERARM];
            index[2] = playerAppearanceJson[SkinPartName._02RIGHT_FOREARM];
            index[3] = playerAppearanceJson[SkinPartName._03RIGHT_HAND];
            index[4] = playerAppearanceJson[SkinPartName._04LEFT_UPPERARM];
            index[5] = playerAppearanceJson[SkinPartName._05LEFT_FOREARM];
            index[6] = playerAppearanceJson[SkinPartName._06LEFT_HAND];
            index[7] = playerAppearanceJson[SkinPartName._07RIGHTTHIGH];
            index[8] = playerAppearanceJson[SkinPartName._08RIGHTKNEE];
            index[9] = playerAppearanceJson[SkinPartName._09LEFTTHIGH];
            index[10] = playerAppearanceJson[SkinPartName._10LEFTKNEE];
            index[11] = playerAppearanceJson[SkinPartName._11NECKWEAR];
            index[12] = playerAppearanceJson[SkinPartName._12HEAD];
            index[13] = playerAppearanceJson[SkinPartName._13EAR];
            index[14] = playerAppearanceJson[SkinPartName._14EYESBLOW];
            index[15] = playerAppearanceJson[SkinPartName._15BACKHAT];
            index[16] = playerAppearanceJson[SkinPartName._16FRONTHAT];
            index[17] = playerAppearanceJson[SkinPartName._17FACE];
            index[18] = playerAppearanceJson[SkinPartName._18MOUTHNOSE];
            index[19] = playerAppearanceJson[SkinPartName._19LEFTEYE];
            index[20] = playerAppearanceJson[SkinPartName._20RIGHTEYE];
            index[21] = playerAppearanceJson[SkinPartName._21LEFTHAIR];
            index[22] = playerAppearanceJson[SkinPartName._22BANGHAIR];
            index[23] = playerAppearanceJson[SkinPartName._23RIGHTHAIR];
            index[24] = playerAppearanceJson[SkinPartName._24AHOGEHAIR];
            index[25] = playerAppearanceJson[SkinPartName._25BACKHAIR];
            index[26] = playerAppearanceJson[SkinPartName._26TAIL];
        }
        
    }
    
    public void ApplyPart()
    {
        for (int i = 0; i < spritePart.Length; i++)
        {
            spritePart[i].sprite = ConvertIndexToSprite(i, index[i]);
        }
        Save();
    }

    #region IndexConversion

    private Sprite ConvertIndexToSprite(int part, int index)
    {
        switch (part)
        {
            case 0: return so.bodyOption[index]; 
            case 1: return so.rightUpperArmOptions[index]; 
            case 2: return so.rightForeArmOptions[index]; 
            case 3: return so.rightHandOptions[index]; 
            case 4: return so.leftUpperArmOptions[index]; 
            case 5: return so.leftForeArmOptions[index]; 
            case 6: return so.leftHandOptions[index]; 
            case 7: return so.rightThighOptions[index]; 
            case 8: return so.rightKneeOptions[index]; 
            case 9: return so.leftThighOptions[index]; 
            case 10: return so.leftKneeOptions[index]; 
            case 11: return so.neckWearOptions[index]; 
            case 12: return so.headOptions[index]; 
            case 13: return so.earsOptions[index]; 
            case 14: return so.eyesBlowOptions[index]; 
            case 15: return so.backHatOptions[index]; 
            case 16: return so.frontHatOptions[index]; 
            case 17: return so.faceOptions[index]; 
            case 18: return so.mouthNoseOptions[index]; 
            case 19: return so.leftEyeOptions[index]; 
            case 20: return so.rightEyeOptions[index]; 
            case 21: return so.leftHairOptions[index]; 
            case 22: return so.bangHairOptions[index]; 
            case 23: return so.rightHairOptions[index]; 
            case 24: return so.ahogeHairOptions[index]; 
            case 25: return so.backHairOptions[index]; 
            case 26: return so.tailOptions[index]; 
        }
        return null;
    }
    
    private int ConvertIndexToOption(int part)
    {
        switch (part)
        {
            case 0: return so.bodyOption.Length; 
            case 1: return so.rightUpperArmOptions.Length; 
            case 2: return so.rightForeArmOptions.Length; 
            case 3: return so.rightHandOptions.Length; 
            case 4: return so.leftUpperArmOptions.Length; 
            case 5: return so.leftForeArmOptions.Length; 
            case 6: return so.leftHandOptions.Length; 
            case 7: return so.rightThighOptions.Length; 
            case 8: return so.rightKneeOptions.Length; 
            case 9: return so.leftThighOptions.Length; 
            case 10: return so.leftKneeOptions.Length; 
            case 11: return so.neckWearOptions.Length; 
            case 12: return so.headOptions.Length; 
            case 13: return so.earsOptions.Length; 
            case 14: return so.eyesBlowOptions.Length; 
            case 15: return so.backHatOptions.Length; 
            case 16: return so.frontHatOptions.Length; 
            case 17: return so.faceOptions.Length; 
            case 18: return so.mouthNoseOptions.Length; 
            case 19: return so.leftEyeOptions.Length; 
            case 20: return so.rightEyeOptions.Length; 
            case 21: return so.leftHairOptions.Length; 
            case 22: return so.bangHairOptions.Length; 
            case 23: return so.rightHairOptions.Length; 
            case 24: return so.ahogeHairOptions.Length; 
            case 25: return so.backHairOptions.Length; 
            case 26: return so.tailOptions.Length; 
        }

        return 0;
    }
    
    #endregion
    
    #region Options
    public void NextPart(int part)
    {
        switch (part)
        {
            case SkinPartName.IDX_BODY :
                index[SkinPartName.IDX_BODY]++;
                index[SkinPartName.IDX_RIGHT_UPPERARM]++;
                index[SkinPartName.IDX_RIGHT_FOREARM]++;
                index[SkinPartName.IDX_RIGHT_HAND]++;
                index[SkinPartName.IDX_LEFT_UPPERARM]++;
                index[SkinPartName.IDX_LEFT_FOREARM]++;
                index[SkinPartName.IDX_LEFT_HAND]++;
                index[SkinPartName.IDX_RIGHTTHIGH]++;
                index[SkinPartName.IDX_RIGHTKNEE]++;
                index[SkinPartName.IDX_LEFTTHIGH]++;
                index[SkinPartName.IDX_LEFTKNEE]++;
                partNumber[part].text = index[part].ToString();
                break;
            
            case SkinPartName.IDX_LEFTEYE:
                index[SkinPartName.IDX_LEFTEYE]++;
                index[SkinPartName.IDX_RIGHTEYE]++;
                partNumber[part].text = index[part].ToString();
                break;
            
            case SkinPartName.IDX_LEFTHAIR:
                index[SkinPartName.IDX_LEFTHAIR]++;
                index[SkinPartName.IDX_BANGHAIR]++;
                index[SkinPartName.IDX_RIGHTHAIR]++;
                index[SkinPartName.IDX_AHOGEHAIR]++;
                index[SkinPartName.IDX_BACKHAIR]++; 
                partNumber[part].text = index[part].ToString();
                break;
            case SkinPartName.IDX_FRONTHAT:
                index[SkinPartName.IDX_BACKHAT]++;
                index[SkinPartName.IDX_FRONTHAT]++;
                partNumber[part].text = index[part].ToString();
                break;
            default : index[part]++;
                partNumber[part].text = index[part].ToString();
                break;
        }

        if (index[part] >= ConvertIndexToOption(part))
        {
            switch (part)
            {
                case SkinPartName.IDX_BODY :
                    index[SkinPartName.IDX_BODY] = 0;
                    index[SkinPartName.IDX_RIGHT_UPPERARM] = 0;
                    index[SkinPartName.IDX_RIGHT_FOREARM] = 0;
                    index[SkinPartName.IDX_RIGHT_HAND] = 0;
                    index[SkinPartName.IDX_LEFT_UPPERARM] = 0;
                    index[SkinPartName.IDX_LEFT_FOREARM] = 0;
                    index[SkinPartName.IDX_LEFT_HAND] = 0;
                    index[SkinPartName.IDX_RIGHTTHIGH] = 0;
                    index[SkinPartName.IDX_RIGHTKNEE] = 0;
                    index[SkinPartName.IDX_LEFTTHIGH] = 0;
                    index[SkinPartName.IDX_LEFTKNEE] = 0;
                    partNumber[part].text = "0";
                    break;
            
                case SkinPartName.IDX_LEFTEYE:
                    index[SkinPartName.IDX_LEFTEYE] = 0;
                    index[SkinPartName.IDX_RIGHTEYE] = 0;
                    partNumber[part].text = "0";
                    break;
            
                case SkinPartName.IDX_LEFTHAIR:
                    index[SkinPartName.IDX_LEFTHAIR] = 0;
                    index[SkinPartName.IDX_BANGHAIR] = 0;
                    index[SkinPartName.IDX_RIGHTHAIR] = 0;
                    index[SkinPartName.IDX_AHOGEHAIR] = 0;
                    index[SkinPartName.IDX_BACKHAIR] = 0; 
                    partNumber[part].text = "0";
                    break;
                case SkinPartName.IDX_FRONTHAT:
                    index[SkinPartName.IDX_BACKHAT] = 0;
                    index[SkinPartName.IDX_FRONTHAT] = 0;
                    partNumber[part].text = "0";
                    break;
                default : index[part] = 0; 
                    partNumber[part].text = "0";
                    break;
            }
        }
        ApplyPart();
        SoundManagerSingleton.Instance.PlaySFX(ClickSFX);
    }
    
    public void PreviousPart(int part)
    {
        switch (part)
        {
            case SkinPartName.IDX_BODY :
                index[SkinPartName.IDX_BODY]--;
                index[SkinPartName.IDX_RIGHT_UPPERARM]--;
                index[SkinPartName.IDX_RIGHT_FOREARM]--;
                index[SkinPartName.IDX_RIGHT_HAND]--;
                index[SkinPartName.IDX_LEFT_UPPERARM]--;
                index[SkinPartName.IDX_LEFT_FOREARM]--;
                index[SkinPartName.IDX_LEFT_HAND]--;
                index[SkinPartName.IDX_RIGHTTHIGH]--;
                index[SkinPartName.IDX_RIGHTKNEE]--;
                index[SkinPartName.IDX_LEFTTHIGH]--;
                index[SkinPartName.IDX_LEFTKNEE]--;
                partNumber[part].text = index[part].ToString();
                break;
            
            case SkinPartName.IDX_LEFTEYE:
                index[SkinPartName.IDX_LEFTEYE]--;
                index[SkinPartName.IDX_RIGHTEYE]--; 
                partNumber[part].text = index[part].ToString();
                break;
            
            case SkinPartName.IDX_LEFTHAIR:
                index[SkinPartName.IDX_LEFTHAIR]--;
                index[SkinPartName.IDX_BANGHAIR]--;
                index[SkinPartName.IDX_RIGHTHAIR]--;
                index[SkinPartName.IDX_AHOGEHAIR]--;
                index[SkinPartName.IDX_BACKHAIR]--; 
                partNumber[part].text = index[part].ToString();
                break;
            
            case SkinPartName.IDX_FRONTHAT:
                index[SkinPartName.IDX_BACKHAT]--;
                index[SkinPartName.IDX_FRONTHAT]--;
                partNumber[part].text = index[part].ToString();
                break;
            
            default : index[part]--; 
                partNumber[part].text = index[part].ToString(); break;
        }
        
        if (index[part] < 0)
        {
            switch (part)
            {
                case SkinPartName.IDX_BODY :
                    index[SkinPartName.IDX_BODY] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_RIGHT_UPPERARM] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_RIGHT_FOREARM] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_RIGHT_HAND] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_LEFT_UPPERARM] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_LEFT_FOREARM] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_LEFT_HAND] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_RIGHTTHIGH] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_RIGHTKNEE] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_LEFTTHIGH] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_LEFTKNEE] = ConvertIndexToOption(part) - 1;
                    partNumber[part].text = (ConvertIndexToOption(part) - 1).ToString();
                    break;
            
                case SkinPartName.IDX_LEFTEYE:
                    index[SkinPartName.IDX_LEFTEYE] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_RIGHTEYE] = ConvertIndexToOption(part) - 1; 
                    partNumber[part].text = (ConvertIndexToOption(part) - 1).ToString();
                    break;
            
                case SkinPartName.IDX_LEFTHAIR:
                    index[SkinPartName.IDX_LEFTHAIR] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_BANGHAIR] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_RIGHTHAIR] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_AHOGEHAIR] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_BACKHAIR] = ConvertIndexToOption(part) - 1; 
                    partNumber[part].text = (ConvertIndexToOption(part) - 1).ToString();
                    break;
                case SkinPartName.IDX_FRONTHAT:
                    index[SkinPartName.IDX_BACKHAT] = ConvertIndexToOption(part) - 1;
                    index[SkinPartName.IDX_FRONTHAT] = ConvertIndexToOption(part) - 1;
                    partNumber[part].text = (ConvertIndexToOption(part) - 1).ToString();
                    break;
                default : index[part] = ConvertIndexToOption(part) - 1; 
                    partNumber[part].text = (ConvertIndexToOption(part) - 1).ToString(); 
                    break;
            }
        }
        ApplyPart();
        SoundManagerSingleton.Instance.PlaySFX(ClickSFX);
    }
    
    #endregion
    
    #region Json

    private void Save()
    {
        JSONObject playerAppearanceJson = new JSONObject();
        playerAppearanceJson.Add(SkinPartName._00BODY,index[0]);
        playerAppearanceJson.Add(SkinPartName._01RIGHT_UPPERARM,index[1]);
        playerAppearanceJson.Add(SkinPartName._02RIGHT_FOREARM,index[2]);
        playerAppearanceJson.Add(SkinPartName._03RIGHT_HAND,index[3]);
        playerAppearanceJson.Add(SkinPartName._04LEFT_UPPERARM,index[4]);
        playerAppearanceJson.Add(SkinPartName._05LEFT_FOREARM,index[5]);
        playerAppearanceJson.Add(SkinPartName._06LEFT_HAND,index[6]);
        playerAppearanceJson.Add(SkinPartName._07RIGHTTHIGH,index[7]);
        playerAppearanceJson.Add(SkinPartName._08RIGHTKNEE,index[8]);
        playerAppearanceJson.Add(SkinPartName._09LEFTTHIGH,index[9]);
        playerAppearanceJson.Add(SkinPartName._10LEFTKNEE,index[10]);
        playerAppearanceJson.Add(SkinPartName._11NECKWEAR,index[11]);
        playerAppearanceJson.Add(SkinPartName._12HEAD,index[12]);
        playerAppearanceJson.Add(SkinPartName._13EAR,index[13]);
        playerAppearanceJson.Add(SkinPartName._14EYESBLOW,index[14]);
        playerAppearanceJson.Add(SkinPartName._15BACKHAT,index[15]);
        playerAppearanceJson.Add(SkinPartName._16FRONTHAT,index[16]);
        playerAppearanceJson.Add(SkinPartName._17FACE,index[17]);
        playerAppearanceJson.Add(SkinPartName._18MOUTHNOSE,index[18]);
        playerAppearanceJson.Add(SkinPartName._19LEFTEYE,index[19]);
        playerAppearanceJson.Add(SkinPartName._20RIGHTEYE,index[20]);
        playerAppearanceJson.Add(SkinPartName._21LEFTHAIR,index[21]);
        playerAppearanceJson.Add(SkinPartName._22BANGHAIR,index[22]);
        playerAppearanceJson.Add(SkinPartName._23RIGHTHAIR,index[23]);
        playerAppearanceJson.Add(SkinPartName._24AHOGEHAIR,index[24]);
        playerAppearanceJson.Add(SkinPartName._25BACKHAIR,index[25]);
        playerAppearanceJson.Add(SkinPartName._26TAIL,index[26]);
        Debug.Log(playerAppearanceJson.ToString());
        //Save Json
        string path = Application.streamingAssetsPath + "/JsonData/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(path + "PlayerSkinIndex.json", playerAppearanceJson.ToString());
    }

    private void SaveName()
    {
        JSONObject characterNameJson = new JSONObject();
        characterNameJson.Add("CustomName", customName);
        
        //Save Json
        string path = Application.streamingAssetsPath + "/JsonData/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(path + "PlayerCharacterName.json", characterNameJson.ToString());
    }
    #endregion
    

    public void Typing()
    {
        SoundManagerSingleton.Instance.PlaySFX(TypingSFX);
    }
}
