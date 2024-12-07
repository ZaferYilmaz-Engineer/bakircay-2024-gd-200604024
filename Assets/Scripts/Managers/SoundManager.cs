using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundReferencesSO soundReferencesSO;

    private void Start()
    {
        var mainMenuMusicObject = new GameObject("MainMenuMusic");
        var audioSource = mainMenuMusicObject.AddComponent<AudioSource>();
        audioSource.clip = soundReferencesSO.mainMenuMusic;
        audioSource.loop = true;
        audioSource.volume = 0.15f;
        audioSource.Play();
    }
}
