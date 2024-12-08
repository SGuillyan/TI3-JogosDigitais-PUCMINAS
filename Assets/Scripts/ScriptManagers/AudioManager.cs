using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    LIGHTRAIN,
    HEAVYRAIN,
    THUNDER,
    IDSINCREASE,
    FINISHQUEST,
    SCREENCLICK,
    HARVESTABLEPLANT,
    EVENTSTART,
    BUY,
    HARVEST,
    SELL,
    PLANT,
    HOE
}

[RequireComponent(typeof(AudioSource))]

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private AudioMixer mixer;

    private static AudioManager instance;
    private AudioSource audioSource;

    public const string MUSIC_KEY = "MusicVolume";
    public const string SFX_KEY = "SFXVolume";

    private static float lastSoundTime = -1f;
    private static float soundCooldown = 1f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float baseVolume = 1)
    {
        if (Time.time - AudioManager.lastSoundTime >= soundCooldown)
        {
            AudioManager.instance.mixer.GetFloat(VolumeSettings.MIXER_SFX, out float mixerVolume);

            float adjustedVolume = baseVolume * Mathf.Pow(10, mixerVolume / 20);

            AudioManager.instance.audioSource.PlayOneShot(AudioManager.instance.soundList[(int)sound], adjustedVolume);

            AudioManager.lastSoundTime = Time.time;
        }
    }
}
