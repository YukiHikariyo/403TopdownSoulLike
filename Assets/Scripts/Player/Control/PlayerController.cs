using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public PlayerInput inputs;
    //时间相关属性未特殊说明均以秒为单位
    [Header("玩家移动属性")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fastRollSpeed;
    [SerializeField] private float slowRollSpeed;
    [SerializeField] private float lastRollDuration;
    [SerializeField] private float L1AtkSpeed;
    [SerializeField] private float L2AtkSpeed;
    [SerializeField] private float L3AtkSpeed;
    [SerializeField] private float L4AtkSpeed;
    [SerializeField] public float LightAtkRotateAngle;

    [Tooltip("鸡尾酒y轴增值")][Range(0, 32f)][SerializeField] public float Molotov_Y;

    [Header("翻滚计数与计时器")]
    [Tooltip("短时间连续翻滚计数")] public int RollCount;
    [Tooltip("翻滚计时器")] public float RollTimer = 0f;
    [Tooltip("短时间连续翻滚最短间隔")] public float RollColdDown;

    [Header("蓄力重击相关属性")]
    [Tooltip("基础蓄力时间")][SerializeField] public float ChargeMaxTime;
    [Tooltip("蓄力时移动速度倍率，速度基值为移动速度")][SerializeField] public float ChargeSpeedFix;
    [Tooltip("重击移动速度")][SerializeField] public float RightAttackSpeed;
    //蓄力基础速度为1/1s，蓄力时间为从开始到满蓄的时间

    [Header("见切相关属性")]
    [Tooltip("无敌帧持续基础时间")][SerializeField] public float UnDamageableLeftTime;
    [Tooltip("完美见切判定基础时间")][SerializeField] public float PerfectCheckTime;
    [Tooltip("见切移动速度")][SerializeField] private float catchChanceSpeed;
    [Tooltip("反击移动速度")][SerializeField] private float backAttackSpeed;
 
    [Header("运动曲线")]
    public AnimationCurve fastRollCruve;
    [Tooltip("翻滚运动曲线")]public AnimationCurve slowRollCruve;
    //每段攻击的位移变化曲线
    public AnimationCurve L1AtkCruve;
    public AnimationCurve L2AtkCruve;
    public AnimationCurve L3AtkCruve;
    public AnimationCurve L4AtkCruve;

    public AnimationCurve CatchChanceCruve;
    public AnimationCurve BackAttackCruve;
    public AnimationCurve RightAttackCruve;

    private float nowSpeed;
    //二段翻滚的移动速度会由快变慢，该参数用于表示减速度，为了方便，向属性传递动作持续时间，属性内部会自行计算减速度
    public float LastRollDuration
    {
        get
        {
            return lastRollDuration;
        }
        set
        {
            lastRollDuration = (slowRollSpeed - moveSpeed) / value;
        }
    }
    private void Awake()
    {
        
    }
    #region 基本移动
    private bool HasPressedX => inputs.MoveLeft || inputs.MoveRight;
    private bool BothPressedX => inputs.MoveLeft && inputs.MoveRight;
    private bool HasPressedY => inputs.MoveUp || inputs.MoveDown;
    private bool BothPressedY => inputs.MoveUp && inputs.MoveDown;

    private int lastInputX = 0;
    
    private int lastInputY = 0;

    private void readLastInput()
    {
        if (HasPressedX)
        {
            if (BothPressedX)
            {
                if (inputs.LeftCheck)
                    lastInputX = -1;
                else if (inputs.RightCheck)
                    lastInputX = 1;
            }
            else if(inputs.MoveLeft)
                lastInputX = -1;
            else if(inputs.MoveRight)
                lastInputX = 1;
        }
        else
            lastInputX = 0;

        if (HasPressedY)
        {
            if (BothPressedY)
            {
                if (inputs.DownCheck)
                    lastInputY = -1;
                else if (inputs.UpCheck)
                    lastInputY = 1;
            }
            else if(inputs.MoveDown)
                lastInputY = -1;
            else if(inputs.MoveUp)
                lastInputY = 1;
        }
        else lastInputY = 0;
    }

    public int AxisX
    {
        get
        {
            if (HasPressedX)
            {
                return lastInputX;
            }
            return 0;
        }
    }

    public int AxisY
    {
        get
        {
            if (HasPressedY)
            {
                return lastInputY;
            }
            return 0;
        }
    }

    public Vector2 MoveAxis
    {
        get
        {
            return new Vector2(lastInputX, lastInputY).normalized;
        }
    }
    public void Idle()
    {
        playerRb.velocity = Vector2.zero;
    }
    public void Move()
    {
        playerRb.velocity = MoveAxis * moveSpeed;
    }
    public void Charge_Move()
    {
        playerRb.velocity = MoveAxis * moveSpeed * ChargeSpeedFix;
    }
    #endregion
    #region 翻滚
    public void FastRoll(Vector2 FaceDir,float time)
    {
        playerRb.velocity = FaceDir * fastRollSpeed * fastRollCruve.Evaluate(time);
    }
  
    public void SlowRoll(Vector2 FaceDir, float time)
    {
        playerRb.velocity = FaceDir * slowRollSpeed * slowRollCruve.Evaluate(time);
    }
    #endregion
    #region 轻攻击位移
    public void LightAttack_1(Vector2 FaceDir, float time)
    {
        playerRb.velocity = FaceDir * L1AtkSpeed * L1AtkCruve.Evaluate(time);
    }
    public void LightAttack_2(Vector2 FaceDir, float time)
    {
        playerRb.velocity = FaceDir * L2AtkSpeed * L2AtkCruve.Evaluate(time);
    }
    public void LightAttack_3(Vector2 FaceDir, float time)
    {
        playerRb.velocity = FaceDir * L3AtkSpeed * L3AtkCruve.Evaluate(time);
    }
    public void LightAttack_4(Vector2 FaceDir, float time)
    {
        playerRb.velocity = FaceDir * L4AtkSpeed * L4AtkCruve.Evaluate(time);
    }
    #endregion
    
    private void Update()
    {
        if (inputs.IsPlayerInputEnable)
        {
            readLastInput();
        }
        if(RollTimer > 0)
            RollTimer -= Time.deltaTime;
    }
    public void SkillDisplace(Skill_Physics skill, Vector2 FaceDir, float time)
    {
        switch (skill)
        {
            case Skill_Physics.CatchChance:
                playerRb.velocity = FaceDir * catchChanceSpeed * CatchChanceCruve.Evaluate(time);
            break;
            case Skill_Physics.BackAttack:
                playerRb.velocity = FaceDir * ChargeMaxTime * BackAttackCruve.Evaluate(time); 
            break;
            case Skill_Physics.RightAttack:
                playerRb.velocity = FaceDir * RightAttackSpeed * RightAttackCruve.Evaluate(time);
            break;  
        }
    }
}
public enum Skill_Physics
{
    CatchChance,
    BackAttack,
    RightAttack,
}
