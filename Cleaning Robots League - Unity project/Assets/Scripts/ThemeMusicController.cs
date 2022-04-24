using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ThemeMusicController : MonoBehaviour
{
    
    //[SerializeField] GameObject mainCamera;
    AudioSource themeMusic;
    [SerializeField] GameObject musicOn;
    Image iconMusicOn;
    [SerializeField] GameObject musicOff;
    Image iconMusicOff;

    public static bool isMusicPlay = true;

    private void Start()
    {
        themeMusic = gameObject.GetComponent<AudioSource>();
        iconMusicOn = musicOn.GetComponent<Image>();
        iconMusicOff = musicOff.GetComponent<Image>();
    }

    void Update()
    {
        if (isMusicPlay)
        {
            if (!themeMusic.isPlaying)
                themeMusic.Play();
            iconMusicOn.enabled = true;
            iconMusicOff.enabled = false;
        }
        else
        {
            if (themeMusic.isPlaying)
                themeMusic.Stop();
            iconMusicOn.enabled = false;
            iconMusicOff.enabled = true;
        }
    }

    public void MusicPlayOrStop()
    {
        if (isMusicPlay)
            isMusicPlay = false;
        else
            isMusicPlay = true;
    }
}
