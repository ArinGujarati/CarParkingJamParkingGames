using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class WinColider : MonoBehaviour
{
    public static WinColider winColider;
    public int totalCar;
    public int tempcar = 0;
    private void Start()
    {
        winColider = this;

        if (PlayerPrefs.GetInt("LevelSelected").Equals(1))
        {
            totalCar = 6;
        }
        if (PlayerPrefs.GetInt("LevelSelected").Equals(2))
        {
            totalCar = 8;
        }
        if (PlayerPrefs.GetInt("LevelSelected").Equals(3))
        {
            totalCar = 10;
        }
        if (PlayerPrefs.GetInt("LevelSelected").Equals(4))
        {
            totalCar = 10;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            Destroy(other.gameObject);
            tempcar++;
            GameManager.gameManager.SettinPannelTextcallFun();
            if (tempcar.Equals(totalCar))
            {
                if (PlayerPrefs.GetInt("LevelSelected") > PlayerPrefs.GetInt("LevelCompletd"))
                {
                    PlayerPrefs.SetInt("LevelCompletd", PlayerPrefs.GetInt("LevelCompletd") + 1);
                    PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 50);
                }
                //Invoke("WinFunAfterSomeSecound",1);
                WinFunAfterSomeSecound();
                Vibration.Vibrate(100);                              
                DDOL.dOL.PlaySound("Win");

            }
        }
    }
    void WinFunAfterSomeSecound()
    {
        SceneManager.LoadScene("Win");
    }
}
