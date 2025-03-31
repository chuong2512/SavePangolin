using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject Startmenu;
    public GameObject GUI;
    public GameObject Gamesuccess;
    public GameObject Loading;

    public Image[] soundImages;
    public Sprite imageOn, imageOff;
    public Text levelTxt;

    void Awake()
    {
        Instance = this;
        Startmenu.SetActive(false);
        //GUI.SetActive(false);
        Gamesuccess.SetActive(false);
        Loading.SetActive(false);

        levelTxt.text = "Level " + GlobalValue.levelPlaying;

        foreach (var img in soundImages)
        {
            img.sprite = (AudioListener.volume == 1) ? imageOn : imageOff;
        }
    }

    // Use this for initialization
    void Start()
    {
        //InvokeRepeating("UpdateTextCo", 0, 0.1f);
    }

    public void ShowStartButton()
    {
        Startmenu.SetActive(true);
    }

    public void StartMoving()
    {
        SoundManager.Click();
        Startmenu.SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void TurnSound()
    {
        if (AudioListener.volume == 0)
            AudioListener.volume = 1;
        else
            AudioListener.volume = 0;

        foreach (var img in soundImages)
        {
            img.sprite = (AudioListener.volume == 1) ? imageOn : imageOff;
        }
        SoundManager.Click();
    }

    [Header("LOADING PROGRESS")]
    public Slider slider;
    public Text progressText;
    IEnumerator LoadAsynchronously(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (slider != null)
                slider.value = progress;
            if (progressText != null)
                progressText.text = (int)progress * 100f + "%";
            yield return null;
        }
    }

    public void TurnGUI(bool turnOn)
    {
        GUI.SetActive(turnOn);
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        SoundManager.PlaySfx(SoundManager.Instance.soundClick);
        Loading.SetActive(true);
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().name));
    }

    public void HomeScene()
    {
        SoundManager.PlaySfx(SoundManager.Instance.soundClick);
        Time.timeScale = 1;
        //Loading.SetActive(true);
        StartCoroutine(LoadAsynchronously("Home"));

    }

    public void OpenStoreLink()
    {
        GameMode.Instance.OpenStoreLink();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCo(1));
    }

    public enum WatchVideoType { Checkpoint, Restart, Next }
    public WatchVideoType watchVideoType;

    IEnumerator GameOverCo(float time)
    {
        GUI.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        BlackScreenUI.instance.Show(0.5f);
        yield return new WaitForSeconds(0.5f);
        RestartGame();
    }

    public void WatchRewardedAds()
    {
        /*AdsManager.AdResult += AdsManager_AdResult;
        AdsManager.Instance.ShowRewardedAds();*/
    }

    private void AdsManager_AdResult(bool isSuccess, int rewarded)
    {
        /*AdsManager.AdResult -= AdsManager_AdResult;
        if (isSuccess)
        {
            GlobalValue.SavedCoins += rewarded;
        }*/
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        RestartGame();
    }

    public void GameSuccess()
    {
        GUI.SetActive(false);
        Gamesuccess.SetActive(true);
    }

    public void NextLevel()
    {
        GlobalValue.levelPlaying++;
        RestartGame();
    }
}
