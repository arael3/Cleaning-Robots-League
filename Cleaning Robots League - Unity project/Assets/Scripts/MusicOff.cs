using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicOff : MonoBehaviour
{
    AudioSource theme;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject musicOn;

    private void Start()
    {
        theme = mainCamera.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!theme.isPlaying)
        {
            gameObject.SetActive(true);
        }
        else
        {
            musicOn.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
