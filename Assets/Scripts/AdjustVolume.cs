using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustVolume : MonoBehaviour
{
    public AudioSource bgSound;
    public Slider volumeSlider;
    public float volume;
    

    private void Awake()
    {
        bgSound = GameObject.Find("BGMusic").GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    { 
        volume = PlayerPrefs.GetFloat("backgroundSound");
        volumeSlider.value = volume;
    }

    // Update is called once per frame
    void Update()
    {
         bgSound.volume = volumeSlider.value;
         PlayerPrefs.SetFloat("backgroundSound", volumeSlider.value);
    }

}
