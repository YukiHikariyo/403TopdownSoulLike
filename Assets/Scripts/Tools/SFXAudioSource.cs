using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SFXAudioSource : MonoBehaviour
{
    public ObjectPool<GameObject> pool;
    public AudioSource audioSource;

    public void Initialize(AudioClip clip, Vector3 position)
    {
        audioSource.clip = clip;
        transform.position = position;
        audioSource.Play();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
            pool.Release(gameObject);
    }
}
