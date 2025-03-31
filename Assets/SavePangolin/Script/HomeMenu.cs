using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeMenu : MonoBehaviour
{
	public static HomeMenu   Instance;
	public        GameObject UI;
	public        GameObject LevelUI;
	public        GameObject LoadingUI;

	[Header("Sound and Music")] public Image  soundImage;
	public                             Sprite soundImageOn,soundImageOff;

	public void TurnSound()
	{
		if(AudioListener.volume==0)
			AudioListener.volume=1;
		else
			AudioListener.volume=0;

		soundImage.sprite=(AudioListener.volume==1) ? soundImageOn : soundImageOff;
		SoundManager.Click();
	}

	public void RateGame() { Application.OpenURL("market://details?id="+Application.productName); }

	public void Awake()
	{
		Instance=this;
		UI.SetActive(true);
		LevelUI.SetActive(false);
		LoadingUI.SetActive(false);

		Time.timeScale=1;
	}

	private void Start()
	{
		if(!GlobalValue.isSound)
			SoundManager.SoundVolume=0;
		if(!GlobalValue.isMusic)
			SoundManager.MusicVolume=0;



		soundImage.sprite=(AudioListener.volume==1) ? soundImageOn : soundImageOff;
	}

	public void ShowLevelUI(bool open)
	{
		SoundManager.Click();
		LevelUI.SetActive(open);
	}

	public void LoadLevel()
	{
		LoadingUI.SetActive(true);
		SceneManager.LoadSceneAsync("Playing");
	}

	public void OutGame() { Application.Quit(); }
}
