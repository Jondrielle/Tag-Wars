using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private float beginningVolume;

    public AudioSource backgroundMusic;

    public static BackgroundMusic Instance { get; private set; }
    

    private void Awake()
    {
        //Dont destory background music
        DontDestroyOnLoad(transform.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        backgroundMusic.Play();
        beginningVolume = backgroundMusic.volume;
    }

    public void LowerVolume()
    {
        backgroundMusic.volume /= 2;
    }
    public void ResetVolume()
    {
        backgroundMusic.volume = beginningVolume;
    }
}
