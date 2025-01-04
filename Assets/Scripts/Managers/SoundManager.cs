using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundReferencesSO soundReferencesSO;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayBackgroundMusic();
        
        PlacementArea.OnAnyObjectsPaired += PlacementArea_OnAnyObjectsPaired;
        LevelManager.OnAnyObjectSpawned += LevelManager_OnAnyObjectSpawned;
        ObjectDragController.OnObjectPicked += ObjectDragController_OnObjectPicked;
        ObjectDragController.OnObjectDropped += ObjectDragController_OnObjectDropped;
    }

    private void ObjectDragController_OnObjectDropped()
    {
        var audioClip = soundReferencesSO.objectSpawnSoundEffect;
        var volume = 1f;
        var pitch = 0.9f;
        Play2DSound(audioClip, volume, pitch);
    }

    private void ObjectDragController_OnObjectPicked()
    {
        var audioClip = soundReferencesSO.objectSpawnSoundEffect;
        var volume = 1f;
        var pitch = 1.1f;
        Play2DSound(audioClip, volume, pitch);
    }

    private void LevelManager_OnAnyObjectSpawned()
    {
        var spawnSoundObject = new GameObject("SpawnSound");
        var audioSource = spawnSoundObject.AddComponent<AudioSource>();
        audioSource.clip = soundReferencesSO.objectSpawnSoundEffect;
        float pitch = Random.Range(1f, 1.2f);
        audioSource.pitch = pitch;
        audioSource.Play();
        Destroy(spawnSoundObject, audioSource.clip.length);
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
        audioSource.volume = 0.1f;
        audioSource.Play();
    }

    private void Play2DSound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        // Vector3 position = Camera.main.transform.position;
        // AudioSource.PlayClipAtPoint(clip, position, volume);

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, volume);
    }
    
    private void Play3DSound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
    }

    private void OnDestroy()
    {
        PlacementArea.OnAnyObjectsPaired -= PlacementArea_OnAnyObjectsPaired;
        LevelManager.OnAnyObjectSpawned -= LevelManager_OnAnyObjectSpawned;
        ObjectDragController.OnObjectPicked -= ObjectDragController_OnObjectPicked;
        ObjectDragController.OnObjectDropped -= ObjectDragController_OnObjectDropped;
    }
}
