using System;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using SimpleJSON;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class SkinLoader : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    
    private Player player;
    public SpriteRenderer[] spritePart;

    [Header("Sprite Options")] 
    public SkinOptions so;

    [Header("Name")]
    [SerializeField] private string nameString;
    [SerializeField] private TextMeshProUGUI nameText;
    public int[] index;
    private int[] ex_index;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView.IsMine)
        {
            GetSkinIndex();
        }
    }

    private void Start()
    {
        ex_index = new int[spritePart.Length];
        if (index.Length != ex_index.Length)
        {
            index = new int[spritePart.Length];
        }
        ChangeSkinProperties();
        GetName();
    }
    
    private void GetName()
    {
        /*
        string path = Application.streamingAssetsPath + "/JsonData/PlayerCharacterName.json";
        string jsonString = File.ReadAllText((path));
        JSONObject charIndex = (JSONObject) JSON.Parse(jsonString);
        
        string nameTemp = charIndex["CustomName"];
        photonView.Owner.NickName = nameTemp;*/
        nameString = photonView.Owner.NickName;
        SetName();
    }

    private void SetName()
    {
        nameText.text = nameString;
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

    private void ChangeSkinProperties()
    {
        if (photonView.IsMine)
        {
            Hashtable skins = new Hashtable
            {
                {SkinPartName._00BODY, index[0]},
                {SkinPartName._01RIGHT_UPPERARM, index[1]},
                {SkinPartName._02RIGHT_FOREARM, index[2]},
                {SkinPartName._03RIGHT_HAND, index[3]},
                {SkinPartName._04LEFT_UPPERARM, index[4]},
                {SkinPartName._05LEFT_FOREARM, index[5]},
                {SkinPartName._06LEFT_HAND, index[6]},
                {SkinPartName._07RIGHTTHIGH, index[7]},
                {SkinPartName._08RIGHTKNEE, index[8]},
                {SkinPartName._09LEFTTHIGH, index[9]},
                {SkinPartName._10LEFTKNEE, index[10]},
                {SkinPartName._11NECKWEAR, index[11]},
                {SkinPartName._12HEAD, index[12]},
                {SkinPartName._13EAR, index[13]},
                {SkinPartName._14EYESBLOW, index[14]},
                {SkinPartName._15BACKHAT, index[15]},
                {SkinPartName._16FRONTHAT, index[16]},
                {SkinPartName._17FACE, index[17]},
                {SkinPartName._18MOUTHNOSE, index[18]},
                {SkinPartName._19LEFTEYE, index[19]},
                {SkinPartName._20RIGHTEYE, index[20]},
                {SkinPartName._21LEFTHAIR, index[21]},
                {SkinPartName._22BANGHAIR, index[22]},
                {SkinPartName._23RIGHTHAIR, index[23]},
                {SkinPartName._24AHOGEHAIR, index[24]},
                {SkinPartName._25BACKHAIR, index[25]},
                {SkinPartName._26TAIL, index[26]},
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(skins);
            for (int i = 0; i < spritePart.Length; i++)
            {
                spritePart[i].sprite = ConvertIndexToSprite(i, index[i]);
            }
        }
        else
        {
            OnPlayerPropertiesUpdate(photonView.Owner, photonView.Owner.CustomProperties);
        }
    }
    
    public Sprite ConvertIndexToSprite(int part, int index)
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

    Sprite ConvertPropsToSprite(int part, Hashtable props)
    {
        switch (part)
        {
            case 0: return so.bodyOption[(int) props[SkinPartName._00BODY]]; 
            case 1: return so.rightUpperArmOptions[(int) props[SkinPartName._01RIGHT_UPPERARM]]; 
            case 2: return so.rightForeArmOptions[(int) props[SkinPartName._02RIGHT_FOREARM]]; 
            case 3: return so.rightHandOptions[(int) props[SkinPartName._03RIGHT_HAND]]; 
            case 4: return so.leftUpperArmOptions[(int) props[SkinPartName._04LEFT_UPPERARM]]; 
            case 5: return so.leftForeArmOptions[(int) props[SkinPartName._05LEFT_FOREARM]]; 
            case 6: return so.leftHandOptions[(int) props[SkinPartName._06LEFT_HAND]]; 
            case 7: return so.rightThighOptions[(int) props[SkinPartName._07RIGHTTHIGH]]; 
            case 8: return so.rightKneeOptions[(int) props[SkinPartName._08RIGHTKNEE]]; 
            case 9: return so.leftThighOptions[(int) props[SkinPartName._09LEFTTHIGH]]; 
            case 10: return so.leftKneeOptions[(int) props[SkinPartName._10LEFTKNEE]]; 
            case 11: return so.neckWearOptions[(int) props[SkinPartName._11NECKWEAR]];
            case 12: return so.headOptions[(int) props[SkinPartName._12HEAD]]; 
            case 13: return so.earsOptions[(int) props[SkinPartName._13EAR]]; 
            case 14: return so.eyesBlowOptions[(int) props[SkinPartName._14EYESBLOW]]; 
            case 15: return so.backHatOptions[(int) props[SkinPartName._15BACKHAT]]; 
            case 16: return so.frontHatOptions[(int) props[SkinPartName._16FRONTHAT]]; 
            case 17: return so.faceOptions[(int) props[SkinPartName._17FACE]]; 
            case 18: return so.mouthNoseOptions[(int) props[SkinPartName._18MOUTHNOSE]]; 
            case 19: return so.leftEyeOptions[(int) props[SkinPartName._19LEFTEYE]]; 
            case 20: return so.rightEyeOptions[(int) props[SkinPartName._20RIGHTEYE]]; 
            case 21: return so.leftHairOptions[(int) props[SkinPartName._21LEFTHAIR]]; 
            case 22: return so.bangHairOptions[(int) props[SkinPartName._22BANGHAIR]]; 
            case 23: return so.rightHairOptions[(int) props[SkinPartName._23RIGHTHAIR]]; 
            case 24: return so.ahogeHairOptions[(int) props[SkinPartName._24AHOGEHAIR]]; 
            case 25: return so.backHairOptions[(int) props[SkinPartName._25BACKHAIR]]; 
            case 26: return so.tailOptions[(int) props[SkinPartName._26TAIL]]; 
        }
        return null;
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (changedProps.ContainsKey(SkinPartName._00BODY))
        {
            if (targetPlayer.ActorNumber == photonView.ControllerActorNr)
            {
                for (int i = 0; i < spritePart.Length; i++)
                {
                    spritePart[i].sprite = ConvertPropsToSprite(i, changedProps);
                }
            }
        }
        
        return;
    }
}
