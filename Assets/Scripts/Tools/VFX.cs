using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    public bool isLoop;
    private float duration;
    private new ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void Initialize(float duration, Vector3 position)
    {
        this.duration = duration;
        transform.position = position;
    }

    private void Update()
    {
        if (isLoop)
        {
            if (duration > 0)
                duration -= Time.deltaTime;
            else
                Destroy(gameObject);
        }
        else
        {
            if (!particleSystem.isPlaying)
                Destroy(gameObject);
        }
    }
}
