using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class BigLight : MonoBehaviour
{
    public float existTime;
    [SerializeField]private float nowTime;
    [SerializeField] private float lightInnerRange;
    [SerializeField] private float lightOuterRange;
    public AnimationCurve radiusCruve;
    public Light2D light;
    private void OnEnable()
    {
        nowTime = 0;
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        light.pointLightInnerRadius = radiusCruve.Evaluate(nowTime / existTime) * lightInnerRange;
        light.pointLightOuterRadius = radiusCruve.Evaluate(nowTime / existTime) * lightOuterRange;
        if (nowTime > existTime)
        {
            gameObject.SetActive(false);
        }
    }
}
