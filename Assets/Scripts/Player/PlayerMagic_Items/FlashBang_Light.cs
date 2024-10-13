using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBang_Light : MonoBehaviour
{
    public Player player;
    public Animator animator;
    public GameObject damageArea;
    AttackArea area;
    public float existTime;
    private float nowTime;
    private float animationLength;
    private void Start()
    {
        nowTime = 0;
        animationLength = animator.GetCurrentAnimatorClipInfo(0).Length;
        area = damageArea.GetComponent<AttackArea>();
        area.player = player;
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
