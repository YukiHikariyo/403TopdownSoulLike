using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBang_Light : MonoBehaviour
{
    public Player player;
    public GameObject damageArea;
    public AttackArea area;
    public AudioClip bomb;
    public float existTime;
    private float nowTime;
    public float damageLength;
    private void Start()
    {
        nowTime = 0;
        area.player = player;
        AudioManager.Instance.PlaySFX(bomb, transform.position);
    }
    private void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime > damageLength)
        {
            damageArea.SetActive(false);
        }
        if (nowTime > existTime)
        {
            Destroy(gameObject);
        }
    }
}
