using UnityEngine;
using System.Collections;

public enum BulletFeature { Normal, Explosion, Shocking }

public class GlobalValue : MonoBehaviour
{
    public static bool isFirstOpenMainMenu = true;
    public static int levelPlaying = 1;

    public static string WorldReached = "WorldReached";
    public static string Coins = "Coins";
    public static string Character = "Character";
    public static string ChoosenCharacterID = "choosenCharacterID";
    public static string ChoosenCharacterInstanceID = "ChoosenCharacterInstanceID";
    public static GameObject CharacterPrefab;
    public static bool isPlayed = false;

    public static bool isMusic = true;
    public static bool isSound = true;
    
    public static bool RemoveAds
    {
        get { return PlayerPrefs.GetInt("RemoveAds", 0) == 1 ? true : false; }
        set { PlayerPrefs.SetInt("RemoveAds", value ? 1 : 0); }
    }

    public static bool isSetDefaultValue
    {
        get { return PlayerPrefs.GetInt("isSetDefaultValue", 0) == 1 ? true : false; }
        set { PlayerPrefs.SetInt("isSetDefaultValue", value ? 1 : 0); }
    }
    
    public static int SavedCoins
    {
        get { return PlayerPrefs.GetInt("SavedCoins",0); }
        set { PlayerPrefs.SetInt("SavedCoins", value); }
    }

    public static int BestScore
    {
        get { return PlayerPrefs.GetInt("BestScore", 0); }
        set { PlayerPrefs.SetInt("BestScore", value); }
    }

    public static int LevelHighest
    {
        get { return PlayerPrefs.GetInt("LevelHighest", 1); }
        set { PlayerPrefs.SetInt("LevelHighest", value); }
    }

    public static bool CheckUnlockCharacter(int ID)
    {
        return (PlayerPrefs.GetInt("Player" + ID, 0) == 1 ? true : false);
    }

    public static void UnlockCharacter(int ID)
    {
        PlayerPrefs.SetInt("Player" + ID, 1);
    }

    public static int PickedCharacter
    {
        get { return PlayerPrefs.GetInt("PickedCharacter", 0); }
        set { PlayerPrefs.SetInt("PickedCharacter", value); }
    }
}
