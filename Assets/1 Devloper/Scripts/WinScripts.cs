using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class WinScripts : MonoBehaviour
{

    public TMP_Text LevelText;
    public GameObject[] WinStartLowMeduimHigh;
    public TMP_Text Score_Text;
    private void Start()
    {
        Score_Text.text = "score : " + PlayerPrefs.GetInt("Score");
        LevelText.text = "Level " + PlayerPrefs.GetInt("LevelSelected") + " Is Completed!";        
        WinStartLowMeduimHigh[2] = GameObject.Find("Canvas/Win/Win Pannel/Star/Third Start");
        WinStartLowMeduimHigh[2].SetActive(true);
        WinStartLowMeduimHigh[2].transform.DOShakePosition(5,4);
        LevelText.transform.DOShakePosition(5, 4);
        Score_Text.transform.DOShakePosition(5, 4);
    }
    public void winButtonClick(string name)
    {
        switch (name)
        {
            case "Home":
              
                DDOL.dOL.PlaySound("ButtonClick");
                SceneManager.LoadScene("StartGame");
                break;
            case "Retry":
             
                DDOL.dOL.PlaySound("ButtonClick");
                Car.car.ISCarTouchRoad = false;
                SceneManager.LoadScene("GamePlay");
                break;
            case "Next":
             
                DDOL.dOL.PlaySound("ButtonClick");
                Car.car.ISCarTouchRoad = false;
                PlayerPrefs.SetInt("LevelSelected", PlayerPrefs.GetInt("LevelSelected") + 1);

                if (PlayerPrefs.GetInt("LevelSelected") > 4)
                    PlayerPrefs.SetInt("LevelSelected", 1);

                SceneManager.LoadScene("GamePlay");
                break;
        }
    }
}
