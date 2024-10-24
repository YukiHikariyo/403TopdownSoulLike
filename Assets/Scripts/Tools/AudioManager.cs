using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    public SFXPool sfxPool;

    public AudioClip testClip;

    public void PlaySFX(AudioClip clip, Vector3 position)
    {
        GameObject sfx = sfxPool.pool.Get();
        sfx.GetComponent<SFXAudioSource>().Initialize(clip, position);
    }

    [ContextMenu("播放测试音效")]
    public void TestPlaySFX() => PlaySFX(testClip, Vector3.zero);
}
