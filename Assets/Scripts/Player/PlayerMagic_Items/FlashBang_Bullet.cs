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
        rb.velocity = mouse.normalized * bulletSpeed;
    }

    public void FixedUpdate()
    {
        if(nowTime < existTime)
        {
            nowTime += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        Instantiate(Light);
    }
}
