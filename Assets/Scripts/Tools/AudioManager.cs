using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioMixer audioMixer;
    public Slider mainSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public SFXPool sfxPool;

    public AudioClip testClip;

    public void PlaySFX(AudioClip clip, Vector3 position)
    {
        GameObject sfx = sfxPool.pool.Get();
        sfx.GetComponent<SFXAudioSource>().Initialize(clip, position);
    }

    [ContextMenu("播放测试音效")]
    public void TestPlaySFX() => PlaySFX(testClip, Vector3.zero);

    public void OnMainVolumeChange(GameObject image)
    {
        audioMixer.SetFloat("MainVolume", 5 + 25 * Mathf.Log10(mainSlider.value > 0 ? mainSlider.value : 0.0001f));
    }

    public void OnMusicVolumeChange(GameObject image)
    {
        audioMixer.SetFloat("MusicVolume", 5 + 25 * Mathf.Log10(musicSlider.value > 0 ? musicSlider.value : 0.0001f));
    }

    public void OnSFXVolumeChange(GameObject image)
    {
        audioMixer.SetFloat("SFXVolume", 5 + 25 * Mathf.Log10(sfxSlider.value > 0 ? sfxSlider.value : 0.0001f));
    }
}
