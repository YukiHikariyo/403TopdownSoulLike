using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public PlayerInput inputs;
    public Light2D playerLight;
    public PlayerData playerData;
    //时间相关属性未特殊说明均以秒为单位
    [Header("玩家移动属性")]
    [SerializeField] private float moveSpeed;
    [Tooltip("疾跑基础速度")][SerializeField] private float runSpeed;
    [SerializeField] private float fastRollSpeed;
    [SerializeField] private float slowRollSpeed;
    [SerializeField] private float lastRollDuration;
    [SerializeField] private float L1AtkSpeed;
    [SerializeField] private float L2AtkSpeed;
    [SerializeField] private float L3AtkSpeed;
    [SerializeField] private float L4AtkSpeed;
    [SerializeField] public float LightAtkRotateAngle;
    [HideInInspector][Tooltip("移动方向是否面朝鼠标")]public bool isSameDirection;

    [Header("翻滚计数与计时器")]
    [Tooltip("短时间连续翻滚计数")] public int RollCount;
    [Tooltip("翻滚计时器")] public float RollTimer = 0f;
    [Tooltip("短时间连续翻滚最短间隔")] public float RollColdDown;

    [Header("蓄力重击相关属性")]
    [Tooltip("基础蓄力时间（每段）")] public float ChargeMaxTime;
    [Tooltip("蓄力时移动速度倍率，速度基值为移动速度")] public float ChargeSpeedFix;
    [Tooltip("重击移动速度")] public float RightAttackSpeed;
    //蓄力基础速度为1/1s，蓄力时间为从开始到满蓄的时间

    [Header("见切相关属性")]
    [Tooltip("无敌帧持续基础时间")]  public float UnDamageableLeftTime;
    [Tooltip("完美见切判定基础时间")] public float PerfectCheckTime;
    [Tooltip("见切移动速度")][SerializeField] private float catchChanceSpeed;
    [Tooltip("反击移动速度")][SerializeField] private float backAttackSpeed;

    #region 魔法相关参数
    [Header("魔法相关参数")]
    [Tooltip("生成燃烧弹数量")]public int molotovBottleCount;
    [Tooltip("鸡尾酒y轴增值")][Range(0, 32f)]public float molotov_Y;
    [Tooltip("落点偏差最大范围")]public float deviation;
    [Space(16)]
    [Tooltip("闪光弹最长存在时间")][Range(0, 10f)]public float FlashBangExistTime;
    [Tooltip("闪光弹移动速度")]public float FlashBangSpeed;
    #endregion

    #region 受击移动速度
    [Header("受击移动速度")]
    public float smallStunSpeed;
    public float normalStunSpeed;
    public float bigStunSpeed;
    #endregion
    [Header("运动曲线")]
    [Tooltip("翻滚运动曲线")] public AnimationCurve fastRollCruve;
    [Tooltip("疾跑运动曲线")]public AnimationCurve RunCruve;
    //每段攻击的位移变化曲线
    public AnimationCurve L1AtkCruve;
    public AnimationCurve L2AtkCruve;
    public AnimationCurve L3AtkCruve;
    public AnimationCurve L4AtkCruve;

    public AnimationCurve CatchChanceCruve;
    public AnimationCurve BackAttackCruve;
    public AnimationCurve RightAttackCruve;
    public AnimationCurve SmallStunCruve;
    public AnimationCurve NormalStunCruve;
    public AnimationCurve BigStunCruve;

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
        playerRb = GetComponent<Rigidbody2D>();
        playerData = GetComponent<PlayerData>();
        playerLight = GetComponentInChildren<Light2D>();
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
        playerRb.velocity = (isSameDirection ? 1 : 0.9f) * moveSpeed * MoveAxis;
    }
    public void Run(float time)
    {
        playerRb.velocity = RunCruve.Evaluate(time) * runSpeed * MoveAxis;
    }
    public void Charge_Move()
    {
        playerRb.velocity = (isSameDirection ? 1 : 0.9f) * ChargeSpeedFix * moveSpeed * MoveAxis;
    }
    #endregion
    #region 翻滚
    public void FastRoll(Vector2 FaceDir,float time)
    {
        playerRb.velocity = FaceDir * fastRollSpeed * fastRollCruve.Evaluate(time);
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
        else if(RollTimer < 0)
        {
            RollCount = 0;
            RollTimer = 0;
        }
    }
    public void SkillDisplace(Skill_Physics skill, Vector2 FaceDir, float time)
    {
        switch (skill)
        {
            case Skill_Physics.CatchChance:
                playerRb.velocity = FaceDir * catchChanceSpeed * CatchChanceCruve.Evaluate(time);
            break;
            case Skill_Physics.BackAttack:
                playerRb.velocity = FaceDir * backAttackSpeed * BackAttackCruve.Evaluate(time); 
            break;
            case Skill_Physics.RightAttack:
                playerRb.velocity = FaceDir * RightAttackSpeed * RightAttackCruve.Evaluate(time);
            break;  
        }
    }
    public void OnHurtDisplace(Stun_Physics stun, Vector2 FaceDir, float time)
    {
        switch (stun)
        {
            case Stun_Physics.SmallStun:
                break;
            case Stun_Physics.NormalStun:
                break;
            case Stun_Physics.BigStun:
                break;
        }
    }
    /// <summary>
    /// 更新玩家光照半径
    /// </summary>
    public void UpdateLightRadius()
    {
        playerLight.pointLightInnerRadius = playerData.FinalLightRadius;
        playerLight.pointLightOuterRadius = Mathf.Min(playerData.FinalLightRadius * 2f, 2f);
    }
}
public enum Skill_Physics
{
    CatchChance,
    BackAttack,
    RightAttack,
}
public enum Stun_Physics
{
    SmallStun,
    NormalStun,
    BigStun,
}
