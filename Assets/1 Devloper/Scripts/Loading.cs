using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public TMP_Text text;
    public Slider slider;
    public const string SceneNameLoaded = "GamePlay";

    private void Start()
    {
        StartCoroutine(enumerator(SceneNameLoaded));
    }

    IEnumerator enumerator(string SceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName);

        while (!asyncOperation.isDone)
        {
            float FinalTime = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            text.text = FinalTime * 100 + "%";
            slider.value = FinalTime;
            yield return null;
        }

    }
}
