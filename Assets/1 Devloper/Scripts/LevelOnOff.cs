using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOnOff : MonoBehaviour
{
    public static LevelOnOff levelOnOff;
    public GameObject[] Buttons;
    public int LevelCompleted;
    public Sprite[] LevelLockSpriteUnLock;
    public Sprite LevelWinSprite;
    private void Awake()
    {
        levelOnOff = this;

    }
    private void OnEnable()
    {
      
        for (int i = 0; i < Buttons.Length; i++)
        {            
            if (i > PlayerPrefs.GetInt("LevelCompletd"))
            {
                Buttons[i].GetComponent<Button>().interactable = false;
                Buttons[i].GetComponent<Image>().sprite = LevelLockSpriteUnLock[0];
                Buttons[i].transform.GetChild(0).GetComponent<Text>().color = new Color(1, 1, 1, 0.36f);
            }
            else
            {
                Buttons[i].GetComponent<Button>().interactable = true;
                Buttons[i].GetComponent<Image>().sprite = LevelLockSpriteUnLock[1];
                Buttons[i].transform.GetChild(0).GetComponent<Text>().color = new Color(1, 1, 1, 1);
                Buttons[i].transform.GetChild(0).transform.localPosition = new Vector3(3, 30, 0);                                
            }
        }
        for (int i = 0; i < PlayerPrefs.GetInt("LevelCompletd"); i++)
        {
            Buttons[i].GetComponent<Image>().sprite = LevelWinSprite;            
            Buttons[i].transform.GetChild(0).transform.localPosition = new Vector3(28, -101, 0);

        }
    }
}
