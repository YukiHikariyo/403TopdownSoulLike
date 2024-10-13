using BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov_Fire : MonoBehaviour
{
    public Animator animator;
    public AttackArea Bomb;
    public AttackArea Fire;
    public float existTime;
    private float nowTime;

    private void Awake()
    {
        Bomb.player = GameObject.Find("Player").GetComponent<Player>(); 
        Fire.player = GameObject.Find("Player").GetComponent<Player>();
    }
    private void Start()
    {
        nowTime = 0;
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
