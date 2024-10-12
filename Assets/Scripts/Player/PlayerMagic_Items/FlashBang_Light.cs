using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBang_Light : MonoBehaviour
{
    public Animator animator;
    GameObject damageArea;
    public float existTime;
    private float nowTime;
    private float animationLength;
    private void Start()
    {
        nowTime = 0;
        animationLength = animator.GetCurrentAnimatorClipInfo(0).Length;
    }
    private void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime > animationLength)
        {
            damageArea.SetActive(false);
        }
        if (nowTime > existTime)
        {
            Destroy(gameObject);
        }
    }
}
