using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人子弹基类（默认不穿透玩家）
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    private Animator anim;
    private AttackArea attackArea;

    public float speed;
    public float acceleration;
    public float rotation;
    public float rotateSpeed;
    private float timer;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        attackArea = GetComponentInChildren<AttackArea>();
    }

    protected virtual void OnEnable()
    {
        anim.Play("Fly");
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    protected virtual void Update()
    {
        speed += acceleration * Time.deltaTime;
        rotation += rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        transform.Translate(Quaternion.Euler(0, 0, rotation) * Vector3.right * speed * Time.deltaTime, relativeTo: Space.World);

        if (timer > 0)
            timer -= Time.deltaTime;
        else
            OnDisappear();
    }

    public virtual void Initialize(Enemy enemy, float speed, float acceleration, float rotation, float rotateSpeed, float duration, int index, bool causeHealthDamage, bool causeBuffDamage, bool directBuff, float directBuffProbability, float directBuffDuration, BuffType buffType)
    {
        this.speed = speed;
        this.acceleration = acceleration;
        this.rotation = rotation;
        this.rotateSpeed = rotateSpeed;
        timer = duration;

        attackArea.enemy = enemy;
        attackArea.motionValueIndex = index;
        attackArea.attackPowerIndex = index;
        attackArea.buffMotionValueIndex = index;
        attackArea.causeHealthDamage = causeHealthDamage;
        attackArea.causeBuffDamage = causeBuffDamage;
        attackArea.directlyAssertBuff = directBuff;
        attackArea.directBuffProbability = directBuffProbability;
        attackArea.directBuffDuration = directBuffDuration;
        attackArea.buffType = buffType;
    }

    protected virtual void OnHit()
    {
        OnDisappear();
    }

    protected virtual void OnDisappear()
    {
        acceleration = 0;
        speed = 0;
        anim.Play("Hit");
    }

    public void DestroyBullet() => Destroy(gameObject);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnHit();
    }
}
