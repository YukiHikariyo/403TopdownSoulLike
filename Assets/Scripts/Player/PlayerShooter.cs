using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("子弹")]
    public Player player;
    [Tooltip("燃烧瓶")] public GameObject molotovBottle;
    [Tooltip("炫目光束")] public GameObject flashBullet;
    private void Awake()
    {
        player = gameObject.GetComponent<Player>();
    }
    //发射莫洛托夫鸡尾酒，target为鼠标位置，add_y用于模拟抛物线最高点高度，num为发射的瓶子数量
    public void Molotov(Vector3 start, Vector3 target, float add_y,float deviation, int num)
    {
        for (int i = 0; i < num; i++)
        {
            if(i != 0)
                target = new Vector3(target.x + Random.Range(-deviation,deviation), target.y + +Random.Range(-deviation, deviation), target.z);
            
            Vector3 bpoint = (start + target)/2 + new Vector3(0,add_y,0);
            
            GameObject obj =Instantiate(molotovBottle);
            MolotovBottle bottle = obj.GetComponent<MolotovBottle>();
            bottle.A = start;   bottle.B = bpoint;  bottle.C = target;
            bottle.player = player;
        }
    }
    //发射闪光弹，提供发射方向，最长存在时间，发射速度
    public void FlashBang(Vector3 start,Vector3 direction,float existTime,float speed,float degree)
    { 
        GameObject obj =Instantiate(flashBullet);
        obj.transform.position = start;
        obj.transform.Rotate(0,0,degree);
        FlashBang_Bullet bullet = obj.GetComponent<FlashBang_Bullet>();
        bullet.mouse = direction;   bullet.existTime = existTime;   bullet.bulletSpeed = speed;
        bullet.player = player;
    }
}
