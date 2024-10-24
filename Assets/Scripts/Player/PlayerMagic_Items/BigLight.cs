using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class BigLight : MonoBehaviour
{
    public PlayerData playerData;
    public float existTime;
    [SerializeField]private float nowTime;
    [SerializeField] private float lightInnerRange;
    [SerializeField] private float lightOuterRange;
    public AnimationCurve radiusCruve;
    public Light2D playerLight;
    private void OnEnable()
    {
        playerData.LightRadiusMultiplication += 1f;
        nowTime = 0;
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        playerLight.pointLightInnerRadius = radiusCruve.Evaluate(nowTime / existTime) * lightInnerRange;
        playerLight.pointLightOuterRadius = radiusCruve.Evaluate(nowTime / existTime) * lightOuterRange;
        if (nowTime > existTime)
        {
            playerData.LightRadiusMultiplication -= 1f;
            gameObject.SetActive(false);
        }
    }
}
