using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlashScene : MonoBehaviour {

	public string sceneLoad = "scene name";
	public float delay = 1;

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadAsynchronously(sceneLoad));
        Screen.sleepTimeout = -1;
        Application.targetFrameRate = 60;
    }

    [Header("LOADING PROGRESS")]
    public Slider slider;
    public Text progressText;
    IEnumerator LoadAsynchronously(string name)
    {
       

        if (slider != null)
            slider.value = 0;
        if (progressText != null)
            progressText.text = (int)0 * 100f + "%";

        yield return new WaitForSeconds(delay);

        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (slider != null)
                slider.value = progress;
            if (progressText != null)
                progressText.text = (int)progress * 100f + "%";
            //			Debug.LogError (progress);
            yield return null;
        }
    }
}
