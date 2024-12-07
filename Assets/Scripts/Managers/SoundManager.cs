using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundReferencesSO soundReferencesSO;

    private void Start()
    {
        PlayBackgroundMusic();
        
        PlacementArea.OnAnyObjectsPaired += PlacementArea_OnAnyObjectsPaired;
    }

    private void PlacementArea_OnAnyObjectsPaired(bool isSuccessful)
    {
        var selectedAudioClip = isSuccessful ? soundReferencesSO.successfulPair : soundReferencesSO.wrongPair;
        var volume = isSuccessful ? 1f : 0.5f;
        Play2DSound(selectedAudioClip, volume);
    }

    private void PlayBackgroundMusic()
    {
        var mainMenuMusicObject = new GameObject("MainMenuMusic");
        var audioSource = mainMenuMusicObject.AddComponent<AudioSource>();
        audioSource.clip = soundReferencesSO.mainMenuMusic;
        audioSource.loop = true;
        audioSource.volume = 0.15f;
        audioSource.Play();
    }

    private void Play2DSound(AudioClip clip, float volume = 1f)
    {
        Vector3 position = Camera.main.transform.position;
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
    
    private void Play3DSound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
    }

    private void OnDestroy()
    {
        PlacementArea.OnAnyObjectsPaired -= PlacementArea_OnAnyObjectsPaired;
    }
}
