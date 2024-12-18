using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov_Fire : MonoBehaviour
{
    public Player player;
    //public Animator animator;
    public AttackArea Bomb;
    public AttackArea Fire;
    public AudioClip fire;
    public float existTime;
    public float bombExistTime;
    private float nowTime;

    private void Awake()
    {

    }
    private void Start()
    {
        nowTime = 0;
        AudioManager.Instance.PlaySFX(fire, transform.position);
    }
    private void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime > bombExistTime)
        {
            Bomb.gameObject.SetActive(false);
        }
        if (nowTime > existTime)
        {
            Destroy(gameObject);
        }
    }
}
