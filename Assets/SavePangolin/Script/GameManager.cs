using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Menu, Playing, Dead, Finish, Waiting };
    public GameState State { get; set; }
    public bool isWatchingAd { get; set; }

    public List<CarController> listOfCars = new List<CarController>();
    [ReadOnly] public int totalCarOnScene;
    [ReadOnly] public bool allowCarMoving = false;

    void Awake()
    {
        Instance = this;
        State = GameState.Menu;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        totalCarOnScene = FindObjectsOfType<CarController>().Length;
    }

    //float fps = 30f;
    //void OnGUI()
    //{
    //    float newFPS = 1.0f / Time.smoothDeltaTime;
    //    fps = Mathf.Lerp(fps, newFPS, 0.0005f);
    //    GUI.Label(new Rect(0, 0, 100, 100), "FPS: " + ((int)fps).ToString());
    //}

    public void RegisterCar(CarController car)
    {
        listOfCars.Add(car);
    }

    public void CarLineConnected()
    {
        foreach(var car in listOfCars)
        {
            if (car.isReadyToGo == false)
                return;
        }

        MenuManager.Instance.ShowStartButton();
    }

    public void CheckCarFinish()
    {
        foreach (var car in listOfCars)
        {
            if (car.isReachToTheDestination == false)
                return;     //meaning still have one car doesn't reach the target yet, so keep wait
        }

        //if all cars reached to the targets, check if them parked correctly or not
        foreach (var car in listOfCars)
        {
            if (car.isParkedCarCorrectly == false)
            {
                GameOver();       //there is at least one car not park correctly
                return;     //meaning still have one car doesn't reach the target yet, so keep wait
            }
        }

        //if all cars was parked correctly, do success
        GameSuccess();
    }

    public void StartGame()
    {
        State = GameState.Playing;

        var listener_ = FindObjectsOfType<MonoBehaviour>().OfType<IListener>();
        foreach (var _listener in listener_)
        {
            _listener.IPlay();
        }

        allowCarMoving = true;
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            var listener_ = FindObjectsOfType<MonoBehaviour>().OfType<IListener>();
            foreach (var _listener in listener_)
            {
                _listener.IPause();
            }
        }
        else
        {
            var listener_ = FindObjectsOfType<MonoBehaviour>().OfType<IListener>();
            foreach (var _listener in listener_)
            {
                _listener.IUnPause();
            }
        }
    }

    void GameSuccess()
    {
        SoundManager.PlaySfx(SoundManager.Instance.soundSuccess);
        StartCoroutine(GameSuccessCo());
    }

    IEnumerator GameSuccessCo()
    {
        if (GlobalValue.levelPlaying == GlobalValue.LevelHighest)
            GlobalValue.LevelHighest++;

        yield return new WaitForSeconds(1);
        MenuManager.Instance.GameSuccess();
    }

    public void GameOver(bool forceGameOver = false)
    {
        SoundManager.PlaySfx(SoundManager.Instance.soundGameover);
        StartCoroutine(GameOverCo(forceGameOver));
    }

    public IEnumerator GameOverCo(bool forceGameOver = false)
    {
        if (State == GameState.Dead)
            yield break;

        /*if (State != GameState.Dead && State != GameState.Waiting && AdsManager.Instance)
        {
            AdsManager.Instance.ShowNormalAd(GameManager.GameState.Dead);
        }*/

        var listener_ = FindObjectsOfType<MonoBehaviour>().OfType<IListener>();
        foreach (var _listener in listener_)
        {
            _listener.IGameOver();
        }

        State = GameState.Dead;

        MenuManager.Instance.GameOver();
        
        SoundManager.Instance.PauseMusic(true);

    }

    public void ResetLevel()
    {
        MenuManager.Instance.RestartGame();
    }
}
