using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov_Fire : MonoBehaviour
{
    public Animator animator;
    public AttackArea attackArea;
    public float existTime;
    private float nowTime;
    private void Start()
    {
        existTime = animator.GetCurrentAnimatorClipInfo(0).Length;
    }
    private void Update()
    {
        nowTime += Time.deltaTime;
        if (nowTime > existTime)
        {
            Destroy(gameObject);
        }
    }
}
