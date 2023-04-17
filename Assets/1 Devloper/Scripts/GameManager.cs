using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Camera camera;
    public static GameManager gameManager;
    public GameObject[] LevelPrefabs;
    public TMP_Text LevlOutPut, SettingScore, SettingLevel, SettingCarLeftToWin, WeatherOutPut;
    int Levelselected = 0;
    public GameObject SettingPannel, RainParticalObjects, CampFireObjects, CloudImage, RainyImage, LeftAndRightMovmentInstruction, UpAndDownmovmentInstruction;
    GameObject LevelInstance;
    public ParticleSystem SmokePartical;
    public Animator animator;
    Touch touch;
    public Toggle togglel;
    public bool IsGamePause;
    float randomrainfloat = 0;
    float randomraintimeonoff = 0;
    int randomcloudweather;
    GameObject TempRainInstiate;
    Vector3 LeftAndRightPos;
    private void Awake()
    {
        gameManager = this;
        togglel.isOn = true;
    }
    private void Start()
    {
        CloudImage.transform.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, -359, 0), .1f).From();
        RainyImage.transform.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, -359, 0), .1f).From();
        CampFireObjects.SetActive(true);
        TempRainInstiate = Instantiate(RainParticalObjects);
        TempRainInstiate.SetActive(false);
        randomraintimeonoff = Random.Range(3, 5); // 3 , 5
        camera = Camera.main;
        SettingPannel.SetActive(false);
        Levelselected = PlayerPrefs.GetInt("LevelSelected");
        LevlOutPut.text = "Level :" + Levelselected;
        if (Levelselected.Equals(1))
        {
            if (PlayerPrefs.GetInt("LevelCompletd") == 0)
            {
                FirstTimeLevelOpenInstruction();
            }
            LevelInstance = Instantiate(LevelPrefabs[Levelselected - 1]);
        }
        if (Levelselected.Equals(2))
            LevelInstance = Instantiate(LevelPrefabs[Levelselected - 1]);
        if (Levelselected.Equals(3))
            LevelInstance = Instantiate(LevelPrefabs[Levelselected - 1]);
        if (Levelselected.Equals(4))
            LevelInstance = Instantiate(LevelPrefabs[Levelselected - 1]);
        camera.DOFieldOfView(110, 0.6f).OnComplete(() => { camera.DOFieldOfView(105, 0.6f); });

    }
    private void Update()
    {
        //DDOL.dOL.audioSource.volume = 0;
        TempRainInstiate.SetActive(false); 
        randomrainfloat += Time.deltaTime;
        if (randomrainfloat >= randomraintimeonoff && randomrainfloat <= randomraintimeonoff + 0.5f)
        {
            TempRainInstiate.SetActive(true);
            CampFireObjects.GetComponent<ParticleSystem>().enableEmission = false;
            StartCoroutine(AnimationCloudAndRain(RainyImage));
            WeatherOutPut.text = "Is't Rainy Weather";
            randomcloudweather = Random.Range(15, 20);
        }
        if (randomrainfloat >= randomraintimeonoff + randomcloudweather && randomrainfloat <= randomraintimeonoff + randomcloudweather + 0.5f)
        {
            TempRainInstiate.SetActive(false);
            Destroy(TempRainInstiate);
            TempRainInstiate = Instantiate(RainParticalObjects);
            TempRainInstiate.SetActive(false);
            CampFireObjects.GetComponent<ParticleSystem>().enableEmission = true;
            StartCoroutine(DelayGoRain());
            WeatherOutPut.text = "Is't Sunny Weather";
            StartCoroutine(AnimationCloudAndRain(CloudImage));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RetryBtn();
        }
    }
    public void BackButton()
    {
        DDOL.dOL.PlaySound("ButtonClick");
        PlayerPrefs.SetInt("BackLevel", 1);
        SceneManager.LoadScene("StartGame");
        //SceneManager.LoadScene("GamePlay");
    }
    public void SettingButtonClick()
    {
        IsGamePause = true;
        DDOL.dOL.PlaySound("ButtonClick");
        SettingPannel.SetActive(true);
        animator.SetBool("Open", true);
        animator.SetBool("Close", false);
        SettingScore.text = "Your Score IS : " + PlayerPrefs.GetInt("Score");
        SettingLevel.text = "Your Level IS : " + PlayerPrefs.GetInt("LevelSelected");
        int tempcarleft = WinColider.winColider.totalCar - WinColider.winColider.tempcar;
        SettingCarLeftToWin.text = "Car Left To Win : " + tempcarleft;
    }
    public void SettinPannelTextcallFun()
    {
        SettingScore.text = "Your Score IS : " + PlayerPrefs.GetInt("Score");
        SettingLevel.text = "Your Level IS : " + PlayerPrefs.GetInt("LevelSelected");
        int tempcarleft = WinColider.winColider.totalCar - WinColider.winColider.tempcar;
        SettingCarLeftToWin.text = "Car Left To Win : " + tempcarleft;
    }
    public void BackButtonClickSetting()
    {
        DDOL.dOL.PlaySound("ButtonClick");
        animator.SetBool("Open", false);
        animator.SetBool("Close", true);
        Invoke("BackBtnSetting", .4f);
    }
    void BackBtnSetting()
    {
        SettingPannel.SetActive(false);
        IsGamePause = false;
    }
    public void RetryBtn()
    {
        IsGamePause = false;
        DDOL.dOL.PlaySound("ButtonClick");
        Car.car.ISCarTouchRoad = false;
        SceneManager.LoadScene("GamePlay");
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
    public void HomeClick()
    {
        DDOL.dOL.PlaySound("ButtonClick");
        SceneManager.LoadScene("StartGame");
    }
    IEnumerator AnimationCloudAndRain(GameObject Input)
    {
        yield return new WaitForSeconds(0.25f);
        ImageAnimation(Input);
    }
    IEnumerator DelayGoRain()
    {
        yield return new WaitForSeconds(10f);
        randomrainfloat = 0;
        randomraintimeonoff = Random.Range(4, 10);
    }
    void ImageAnimation(GameObject AnimationImages)
    {
        AnimationImages.transform.GetComponent<RectTransform>().DOScale(new Vector3(0, 0, 0), .1f).From().OnComplete(() =>
        {
            AnimationImages.transform.GetComponent<RectTransform>().DOScale(new Vector3(.5f, .5f, .5f), 1f);
        });
        AnimationImages.transform.GetComponent<RectTransform>().DOAnchorPos(new Vector3(726, -575, 0), .1f).From().OnComplete(() =>
        {
            AnimationImages.transform.GetComponent<RectTransform>().DOAnchorPos(new Vector3(726, 355, 0), 1).OnComplete(() =>
            {
                StartCoroutine(AnimationCompletedAfterdelayToGoBack(AnimationImages));
            });
        });

    }
    IEnumerator AnimationCompletedAfterdelayToGoBack(GameObject AnimationImages)
    {
        yield return new WaitForSeconds(1.5f);
        AnimationImages.transform.GetComponent<RectTransform>().DOScale(new Vector3(0, 0, 0), 1.2f);
        AnimationImages.transform.GetComponent<RectTransform>().DOAnchorPos(new Vector3(726, -575, 0), 1f);
    }
    void FirstTimeLevelOpenInstruction()
    {
        Debug.Log("FirstTime");
        LeftAndRightPos = LeftAndRightMovmentInstruction.transform.position;
        LeftAndRightMovmentInstruction.transform.DOMove(LeftAndRightPos + new Vector3(1, 0, 0), 1).OnComplete(() =>
             {
                 LeftAndRightMovmentInstruction.transform.DOMove(LeftAndRightPos + new Vector3(-1, 0, 0), 1);
             }).SetLoops(-1, LoopType.Yoyo);
    }
}
