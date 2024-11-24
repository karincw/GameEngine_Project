using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioSource _bgmPlayer;
    [SerializeField] private AudioSource _effectPlayer;
    public AudioMixer audioMixer;

    [Header("Panel")]
    [SerializeField] private RectTransform _panelTrm;

    public void PlayBGM(AudioClip clip)
    {
        _bgmPlayer.PlayOneShot(clip);
    }

    public void PlayEffect(AudioClip clip)
    {
        _effectPlayer.PlayOneShot(clip);
    }

    public void StopAll()
    {
        StopEffect();
        StopBGM();
    }
    public void StopEffect()
    {
        _bgmPlayer.Stop();
    }
    public void StopBGM()
    {
        _effectPlayer.Stop();
    }

}
