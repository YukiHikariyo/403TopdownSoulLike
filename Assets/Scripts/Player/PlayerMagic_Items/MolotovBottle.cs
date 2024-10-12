using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MolotovBottle : MonoBehaviour
{
    public GameObject fire;
    public Vector3 A, B, C;
    [Range(0,10)]public float existTime;
    public float nowTime = 0;
    public void Awake()
    {
        nowTime = 0;
    }

    public void FixedUpdate()
    {
        if (nowTime < 1f)
        {
            nowTime += Time.fixedDeltaTime / existTime;
            transform.position = quardaticBezier(A, B, C, nowTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Vector3 quardaticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 aa = a + (b - a) * t;
        Vector3 bb = b + (c - b) * t;
        return aa + (bb - aa) * t;
    }
    private void OnDestroy()
    {
        Instantiate(fire);
    }
}
