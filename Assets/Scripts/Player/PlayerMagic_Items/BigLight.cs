using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BigLight : MonoBehaviour
{
    public float existTime;
    [SerializeField]private float nowTime;
    public AnimationCurve radiusCruve;
    private void OnEnable()
    {
        nowTime = 0;
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime > existTime)
        {
            gameObject.SetActive(false);
        }
    }
}
