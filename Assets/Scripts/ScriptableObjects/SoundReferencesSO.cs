using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundReferences", menuName = "ScriptableObjects/SoundReferences")]
public class SoundReferencesSO : ScriptableObject
{
    public AudioClip mainMenuMusic;
    public AudioClip successfulPair;
    public AudioClip wrongPair;
}
