using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicOn : MonoBehaviour
{
    AudioSource theme;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject musicOff;

    private void Start()
    {
        theme = mainCamera.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (theme.isPlaying)
        {
            gameObject.SetActive(true);
        }
        else
        {
            musicOff.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
