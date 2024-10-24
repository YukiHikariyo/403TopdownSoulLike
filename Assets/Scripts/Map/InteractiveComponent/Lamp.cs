using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lamp : InteractiveComponent
{
    [Tooltip("光照半径变化幅度")]public AnimationCurve lightcruve;
    [Tooltip("光照外半径")][SerializeField] int radius;
    public Light2D light2D;
    private float nowTime;
    public override void Initialization()
    {
        if (state)
            light2D.enabled = true;
        else
            light2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !State)
        {
            State = true;
        }
    }
    private void Start()
    {
        nowTime = 0;
        State = false;
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        if (State)
        {
            light2D.pointLightOuterRadius = radius + lightcruve.Evaluate(nowTime / 3f);
        }
    }
}
