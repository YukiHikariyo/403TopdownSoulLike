using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("子弹")]
    [Tooltip("燃烧瓶")] public GameObject molotovBottle;
    [Tooltip("炫目光束")] public GameObject flashBullet;
    private void Awake()
    {
        
    }
    //发射莫洛托夫鸡尾酒，target为鼠标位置，add_y用于模拟抛物线最高点高度，num为发射的瓶子数量
    public void Molotov(Vector3 start, Vector3 target, float add_y,float deviation, int num)
    {
        for (int i = 0; i < num; i++)
        {
            if(i != 0)
                target = new Vector3(target.x + Random.Range(-deviation,deviation), target.y + +Random.Range(-deviation, deviation), target.z);
            
            Vector3 bpoint = (start + target)/2 + new Vector3(0,add_y,0);
            MolotovBottle obj = molotovBottle.GetComponent<MolotovBottle>();
            obj.A = start;  obj.B = bpoint; obj.C = target;
            Instantiate(molotovBottle);
        }
    }
    //发射闪光弹，提供发射方向，最长存在时间，发射速度
    public void FlashBang(Vector3 direction,float existTime,float speed)
    {
        FlashBang_Bullet obj = flashBullet.GetComponent<FlashBang_Bullet>();
        obj.mouse = direction;  obj.existTime = existTime;  obj.bulletSpeed = speed;
        Instantiate(flashBullet);
    }
}
