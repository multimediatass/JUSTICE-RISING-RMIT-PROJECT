using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeRising.UIGameSetting
{
    using Tproject.AudioManager;

    public class UIGameSettings : MonoBehaviour
    {
        public Slider musicSlider, sfxSlider;

        private void Start()
        {
            musicSlider.value = AudioManager.Instance.GetMasterVolume();
        }

        public void ToggleMuteMusic()
        {
            AudioManager.Instance.ToggleMusic();
        }

        public void ToggleMuteSFX()
        {
            AudioManager.Instance.ToggleSFX();
        }

        public void MusicVolumeSetting()
        {
            AudioManager.Instance.SetMasterVolume(musicSlider.value);
        }

        public void SFXVolumeSetting()
        {
            AudioManager.Instance.SFXVolume(sfxSlider.value);
        }
    }
}
