﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Level : MonoBehaviour
{
	int levelNumber=1;

	public Text       TextLevel;
	public GameObject Locked;

	public GameObject backgroundNormal,backgroundInActive;

	void Start()
	{
		levelNumber=int.Parse(gameObject.name);
		backgroundNormal.SetActive(true);
		backgroundInActive.SetActive(false);

		var levelReached=GlobalValue.LevelHighest;

		TextLevel.text=levelNumber.ToString();
		if((levelNumber<=levelReached))
		{
			Locked.SetActive(false);

			var openLevel=levelReached+1>=levelNumber /*int.Parse(gameObject.name)*/;

			Locked.SetActive(!openLevel);

			bool isInActive=levelNumber==levelReached;

			backgroundNormal.SetActive(!isInActive);
			backgroundInActive.SetActive(isInActive);

			GetComponent<Button>().interactable=openLevel;
		}
		else
		{
			TextLevel.gameObject.SetActive(true);
			Locked.SetActive(true);
			GetComponent<Button>().interactable=false;
		}
	}

	public void LoadScene()
	{
		GlobalValue.levelPlaying=levelNumber;
		SoundManager.Click();
		HomeMenu.Instance.LoadLevel();
	}
}
