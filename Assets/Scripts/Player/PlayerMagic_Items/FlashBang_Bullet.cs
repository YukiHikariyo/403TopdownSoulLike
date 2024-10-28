using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBang_Bullet : MonoBehaviour
{
    public Vector3 mouse;
    public Rigidbody2D rb;
    public Player player;
    public float bulletSpeed;
    public float existTime;
    public float nowTime = 0;

    public GameObject Light;
    public void Awake()
    {
        nowTime = 0;
    }

    public void FixedUpdate()
    {
        if(nowTime < existTime)
        {
            nowTime += Time.deltaTime;
            rb.velocity = mouse.normalized * bulletSpeed;
        }
        else
        {
            GameObject b = Instantiate(Light);
            b.transform.position = transform.position;
            FlashBang_Light l = b.GetComponent<FlashBang_Light>();
            l.player = player;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject b = Instantiate(Light);
        b.transform.position = transform.position;
        FlashBang_Light l = b.GetComponent<FlashBang_Light>();
        l.player = player;
        l.area.player = player;
        Destroy(gameObject);
    }
}
