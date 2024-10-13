using BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov_Fire : MonoBehaviour
{
    public Player player;
    public Animator animator;
    public AttackArea Bomb;
    public AttackArea Fire;
    public float existTime;
    public float bombExistTime;
    private float nowTime;

    private void Awake()
    {
        Bomb.player = player;
        Fire.player = player;
    }
    private void Start()
    {
        nowTime = 0;
        existTime = animator.GetCurrentAnimatorClipInfo(0).Length;
    }
    private void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime > bombExistTime && Bomb.enabled)
        {
            Bomb.enabled = false;
        }
        if (nowTime > existTime)
        {
            Destroy(gameObject);
        }
    }
}
