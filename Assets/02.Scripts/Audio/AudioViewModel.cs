using System;
using System.ComponentModel;
using UnityEngine;

public class AudioViewModel : ViewModelBase
{
    private float _bgmVolume;
    public float BGMVolume
    {
        get => _bgmVolume;
        set
        {
            if (_bgmVolume != value) // 값이 진짜 변했다면
            {
                _bgmVolume = value;
                OnPropertyChanged(nameof(BGMVolume)); // 변했다고 알림
            }
        }
    }

    private float _sfxVolume;
    public float SFXVolume
    {
        get => _sfxVolume;
        set
        {
            if (_sfxVolume != value) // 값이 진짜 변했다면
            {
                _sfxVolume = value;
                OnPropertyChanged(nameof(BGMVolume)); // 변했다고 알림
            }
        }
    }


    public AudioViewModel(AudioSettingData audioSettingData)
    {

        _bgmVolume = audioSettingData.BgmVolume;
        _sfxVolume = audioSettingData.SfxVolume;
        this.PropertyChanged += OnModelPropertyChanged;
    }

    public void Dispose()
    {
        this.PropertyChanged -= OnModelPropertyChanged;
    }

    public event Action<string> ModelPropertyChanged;

   

    public void RequestSetBgmVolume(float volume)
    {
        float clampedVolume = Mathf.Clamp01(volume);
        BGMVolume = clampedVolume;
    }

    public void RequestSetSfxVolume(float volume)
    {
        float clampedVolume = Mathf.Clamp01(volume);
        SFXVolume = clampedVolume;
    }

    private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (ModelPropertyChanged == null)
        {
            return;
        }

        ModelPropertyChanged.Invoke(e.PropertyName);
    }
}