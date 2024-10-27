using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBang_Light : MonoBehaviour
{
    public Player player;
    public GameObject damageArea;
    AttackArea area;
    public float existTime;
    private float nowTime;
    public float damageLength;
    private void Start()
    {
        nowTime = 0;
        area = damageArea.GetComponent<AttackArea>();
        area.player = player;
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
