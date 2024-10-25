using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioMixer audioMixer;

    public Slider currentMainSlider;
    public Slider currentMusicSlider;
    public Slider currentSFXSlider;

    public Slider[] mainSliders;
    public Slider[] musicSliders;
    public Slider[] sfxSliders;

    public SFXPool sfxPool;

    public AudioClip testClip;

    protected override void Awake()
    {
        base.Awake();

        SaveManager.Instance.LoadSettings();
    }

    public void PlaySFX(AudioClip clip, Vector3 position)
    {
        GameObject sfx = sfxPool.pool.Get();
        sfx.GetComponent<SFXAudioSource>().Initialize(clip, position);
    }

    [ContextMenu("播放测试音效")]
    public void TestPlaySFX() => PlaySFX(testClip, Vector3.zero);

    public void OnMainVolumeChange(GameObject image)
    {
        audioMixer.SetFloat("MainVolume", 5 + 25 * Mathf.Log10(currentMainSlider.value > 0 ? currentMainSlider.value : 0.0001f));
    }

    public void OnMusicVolumeChange(GameObject image)
    {
        audioMixer.SetFloat("MusicVolume", 5 + 25 * Mathf.Log10(currentMusicSlider.value > 0 ? currentMusicSlider.value : 0.0001f));
    }

    public void OnSFXVolumeChange(GameObject image)
    {
        audioMixer.SetFloat("SFXVolume", 5 + 25 * Mathf.Log10(currentSFXSlider.value > 0 ? currentSFXSlider.value : 0.0001f));
    }

    public void UpdateVolume()
    {
        audioMixer.SetFloat("MainVolume", 5 + 25 * Mathf.Log10(currentMainSlider.value > 0 ? currentMainSlider.value : 0.0001f));
        audioMixer.SetFloat("MusicVolume", 5 + 25 * Mathf.Log10(currentMusicSlider.value > 0 ? currentMusicSlider.value : 0.0001f));
        audioMixer.SetFloat("SFXVolume", 5 + 25 * Mathf.Log10(currentSFXSlider.value > 0 ? currentSFXSlider.value : 0.0001f));
    }

    public void ChangeSliders(int index)
    {
        currentMainSlider = mainSliders[index];
        currentMusicSlider = musicSliders[index];
        currentSFXSlider = sfxSliders[index];
    }
}
