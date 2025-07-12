using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource backgroundAudioSource; // 배경 음악을 위한 AudioSource
    public AudioSource soundEffectAudioSource; // 사운드 이펙트를 위한 AudioSource

    public List<AudioClip> backgroundMusic; // 배경 음악
    public List<AudioClip> soundEffects; // 사운드 이펙트 리스트

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 이 오브젝트를 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(int index)
    {
        if (index < 0 || index >= soundEffects.Count)
        {
            return;
        }

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && soundEffects[index] != null)
        {
            audioSource.PlayOneShot(soundEffects[index]);
        }
        else
        {
        }
    }

    public void PlayBackgroundMusic(int index)
    {
        if (index < 0 || index >= backgroundMusic.Count)
        {
            return;
        }

        if (backgroundAudioSource != null && backgroundMusic[index] != null)
        {
            backgroundAudioSource.clip = backgroundMusic[index];
            backgroundAudioSource.Play();
        }
    }
}
