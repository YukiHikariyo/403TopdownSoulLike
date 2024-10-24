using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lamp : InteractiveComponent
{
    AnimationCurve lightcruve;
    public Light2D light2D;
    public override void Initialization()
    {
        if (state)
            light2D.enabled = true;
        else
            light2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !state)
        {
            state = true;
        }
    }

    private void Update()
    {
        if (state)
        {
        }
    }
}
