using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
        playerLight.color = Color.yellow;
        playerLight.intensity = 3f;
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        playerLight.pointLightInnerRadius = radiusCruve.Evaluate(nowTime / existTime) * lightInnerRange;
        playerLight.pointLightOuterRadius = radiusCruve.Evaluate(nowTime / existTime) * lightOuterRange;
        if (nowTime > existTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        playerData.LightRadiusMultiplication -= 1f;
        playerLight.color = Color.white;
        playerLight.intensity = 2f;
    }
}
