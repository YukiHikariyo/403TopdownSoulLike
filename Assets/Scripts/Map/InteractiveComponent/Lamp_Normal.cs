using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lamp_Normal : MonoBehaviour
{
    [Tooltip("光照半径变化幅度")] public AnimationCurve lightcruve;
    [Tooltip("光照外半径")][SerializeField] int radius;
    public Light2D light2D;
    private float nowTime;

    private void Start()
    {
        nowTime = 0;
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        light2D.pointLightOuterRadius = radius + lightcruve.Evaluate(nowTime / 3f);
    }

}
