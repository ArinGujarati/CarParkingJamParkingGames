using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    public static DDOL dOL;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    private void Awake()
    {
        if (dOL == null)
        {
            DontDestroyOnLoad(gameObject);
            dOL = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void PlaySound(string audioname)
    {
        switch (audioname)
        {
            case "ButtonClick":
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "CarHorn":
                audioSource.PlayOneShot(audioClips[1]);
                break;
            case "Win":
                audioSource.PlayOneShot(audioClips[2]);
                break;
        }
    }

}
