using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        if (GameObject.FindGameObjectWithTag("Level") == null)
        {
            var _go = Resources.Load("Level/Level " + GlobalValue.levelPlaying) as GameObject;
            if (_go)
                Instantiate(_go, Vector2.zero, Quaternion.identity);
            else
            {
                Debug.LogError("Can't find the Level " + GlobalValue.levelPlaying);
                Debug.LogError("Goto Home scene");
                MenuManager.Instance.HomeScene();
            }
        }
        else
            Debug.LogError("There are a level in the scene, Remove it after testing it");
    }
}