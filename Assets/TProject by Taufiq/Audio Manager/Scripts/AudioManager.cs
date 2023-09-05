using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tproject.AudioManager
{
    // Creator Instagram: @shantaufiq

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public Sound[] musicSounds, sfxSounds;
        public AudioSource musicSource, sfxSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            PlayMusic("Theme");
        }

        public void PlayMusic(string name)
        {
            Sound s = Array.Find(musicSounds, (x) => x.name == name);

            if (s == null) Debug.Log($"{name} isn't available");
            else
            {
                musicSource.clip = s.clip;
                musicSource.Play();
            }
        }

        public void MusicVolume(float volume)
        {
            musicSource.volume = volume;
        }

        public void PlaySFX(string name)
        {
            Sound s = Array.Find(sfxSounds, (x) => x.name == name);

            if (s == null) Debug.Log($"{name} isn't available");
            else
            {
                sfxSource.PlayOneShot(s.clip);
            }
        }

        public void PlaySFX(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        public void StopSFX()
        {
            sfxSource.Stop();
        }

        public void ToggleMusic()
        {
            musicSource.mute = !musicSource.mute;
        }

        public void ToggleSFX()
        {
            sfxSource.mute = !sfxSource.mute;
        }

        public void SFXVolume(float volume)
        {
            sfxSource.volume = volume;
        }

        public float GetMusicVolume()
        {
            return musicSource.volume;
        }
    }
}
