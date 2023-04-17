using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ManagerStart : MonoBehaviour
{    
    public static ManagerStart managerStart;
    public GameObject canvas;
    public GameObject[] pannels;
    public TMP_Text Score_Text;
    public AudioSource audioSource;
    public Toggle togglel;
    DateTime StopTimer, Starttimer;
    private void Start()
    {         
        Starttimer = DateTime.Now;
        string temptime = PlayerPrefs.GetString("StopTimer", "");
        if (temptime != "")
        {
            DateTime QuitTimeParse = DateTime.Parse(temptime);
            if (Starttimer > QuitTimeParse)
            {
                TimeSpan Diffrence = Starttimer - QuitTimeParse;
                Debug.Log(Diffrence.TotalSeconds);
                if (Diffrence.TotalSeconds > 5)
                {
                    Debug.Log("Worked");
                }
            }
        }
        togglel.isOn = true;
        managerStart = this;
        Score_Text.text = "score : " + PlayerPrefs.GetInt("Score");
        canvas = GameObject.Find("Canvas");
        for (int i = 0; i < 3; i++)
        {
            pannels[i] = canvas.transform.GetChild(i).gameObject;
        }
        if (PlayerPrefs.GetInt("BackLevel") == 1)
        {
            PlayerPrefs.SetInt("BackLevel", 0);
            for (int i = 0; i < pannels.Length; i++)
            {
                if (i == 1)
                    pannels[i].SetActive(true);
                else
                    pannels[i].SetActive(false);
            }
        }
        else
        {
            PlayerPrefs.SetInt("BackLevel", 0);
            for (int i = 0; i < pannels.Length; i++)
            {
                if (i == 0)
                    pannels[i].SetActive(true);
                else
                    pannels[i].SetActive(false);
            }
        }
    }
    private void OnApplicationQuit()
    {
        StopTimer = DateTime.Now;
        Debug.Log(StopTimer);
        PlayerPrefs.SetString("StopTimer", StopTimer.ToString());        
    }
    private void Update()
    {
                
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }
    public void valuechnage()
    {
        if (togglel.isOn == true)
        {
            DDOL.dOL.audioSource.volume = 1;
        }
        else
        {
            DDOL.dOL.audioSource.volume = 0;
        }
    }
    public void StarButtonClick(string name)
    {
        switch (name)
        {
            case "LevelClick":
                DDOL.dOL.PlaySound("ButtonClick");
                for (int i = 0; i < pannels.Length; i++)
                {
                    if (i == 1)
                        pannels[i].SetActive(true);
                    else
                        pannels[i].SetActive(false);
                }
                break;
            case "LevelBack":

                DDOL.dOL.PlaySound("ButtonClick");
                for (int i = 0; i < pannels.Length; i++)
                {
                    if (i == 0)
                        pannels[i].SetActive(true);
                    else
                        pannels[i].SetActive(false);
                }
                break;
            case "ResetData":
                PlayerPrefs.DeleteAll();
                SceneManager.LoadScene("StartGame");
                break;
            case "SettingButton":
                //  DDOL.dOL.audioSource.volume = 1;
                DDOL.dOL.PlaySound("ButtonClick");
                for (int i = 0; i < pannels.Length; i++)
                {
                    if (i == 2)
                        pannels[i].SetActive(true);
                    else
                        pannels[i].SetActive(false);
                }
                break;
            case "SettingBack":

                DDOL.dOL.PlaySound("ButtonClick");
                for (int i = 0; i < pannels.Length; i++)
                {
                    if (i == 0)
                        pannels[i].SetActive(true);
                    else
                        pannels[i].SetActive(false);
                }
                break;
        }
    }
    public void LevelButtonClick(int name)
    {
        switch (name)
        {
            case 1:

                DDOL.dOL.PlaySound("ButtonClick");
                PlayerPrefs.SetInt("LevelSelected", 1);
                SceneManager.LoadScene("Loding");
                break;
            case 2:

                DDOL.dOL.PlaySound("ButtonClick");
                PlayerPrefs.SetInt("LevelSelected", 2);
                SceneManager.LoadScene("Loding");
                break;
            case 3:

                DDOL.dOL.PlaySound("ButtonClick");
                PlayerPrefs.SetInt("LevelSelected", 3);
                SceneManager.LoadScene("Loding");
                break;
            case 4:
                DDOL.dOL.PlaySound("ButtonClick");
                PlayerPrefs.SetInt("LevelSelected", 4);
                SceneManager.LoadScene("Loding");
                break;
        }
    }

}
