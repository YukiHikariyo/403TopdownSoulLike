using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStateMachine : StateMachine
{
    [SerializeField] Camera m_camera;
    public Transform playerTransform;
    public Transform attacker;

    [SerializeField] PlayerState[] stateTable;
    #region 动作消耗索引表
    [Header("动作消耗索引表")]
    [Tooltip("奔跑每秒消耗")] public float runEnergyCost;
    [Tooltip("蓄力每秒消耗")] public float chargeEnergyCost;
    [Tooltip("允许进入奔跑的最低耐力")] public float runEnergyLimit;
    [Tooltip("动作体力值基础消耗")][SerializeField] float[] energyCost;
    [Tooltip("法术魔力值基础消耗")] public float[] manaCost;
    Dictionary<Type, float> energyCostDict;
    #endregion
    #region 鼠标相关
    [Header("鼠标相关")]
    [SerializeField] float mousedegree;
    [SerializeField] Vector3 mouseDistance;
    #endregion
    #region 特定时间节点
    [Header("特定时间节点")]
    [Tooltip("见切完美判定和无敌帧判定区间中点")]public float CatchChancepoint;
    [Tooltip("无硬直无敌时间")] public float noStunUndamageableTime;
    [Tooltip("反击是否命中")] public bool successfulBack;
    //无敌帧计时器
    private float noStunTimer;
    #endregion
    #region 体力恢复相关参数
    [Header("体力恢复相关参数")]
    [Tooltip("体力开始恢复计时器")][SerializeField]private float energyRecoverTimer;
    [Tooltip("消耗体力到体力开始恢复的延迟时间")][SerializeField]private float energyRecoverDelay;
    #endregion
    #region 魔法冷却计时
    [Header("魔法冷却计时器")]
    [Tooltip("法术计时器")] public float[] magicTimer;
    [Tooltip("法术冷却时间")] public float[] magicColdDown;
    #endregion
    #region 蓄力相关
    [Header("蓄力相关")]
    public int chargeStage;
    [Tooltip("蓄力每秒消耗体力")] public float chargeCost;
    [Tooltip("是否完成上次蓄力")] public bool hasReleasedRight;
    #endregion
    #region 组件
    [Header("组件")]
    //获取组件的方式之后可以调整
    public PlayerInput playerInput;
    public PlayerController playerController;
    public Animator playerAnimator;
    public SpriteRenderer playerRenderer;
    public Player player;
    public PlayerData playerData;
    public PlayerShooter shooter;
    public GameObject bigLight;
    public SpriteRenderer playerTip;

    public GameObject LightAtk_1;
    public GameObject LightAtk_2;
    public GameObject LightAtk_3;
    public GameObject LightAtk_4;
    public GameObject BackAttack;

    public GameObject RightAttack_0;  
    public GameObject RightAttack_1;
    public GameObject RightAttack_2;
    public GameObject RightAttack_3;

    public GameObject RightAttack;
    [Tooltip("蓄力提示VFX")]
    public ParticleSystem chargeVFX;
    [Tooltip("蓄力提示物体")]
    public GameObject chargeVFXobj;
    #endregion
    #region 接受输入窗口
    [Tooltip("是否接收输入")]public bool CanAcceptInput { get; set; }
    #endregion

    #region 预输入和后摇开始时刻
    public InputMemory memory;
    [Tooltip("是否允许状态切换")]public bool CanStateSwitch {  get; set; }
    #endregion

    #region 场景交互
    /// <summary>
    /// 是否可以与场景物体交互
    /// </summary>
    public bool CanInterAction;

    public InteractiveComponent interactionObj = null;
    #endregion
    #region SFX
    [Space(16)]
    [Header("SFX")]
    [Space(16)]

    public AudioClip attack_1;
    public AudioClip attack_4;
    public AudioClip fs;
    public AudioClip gun;
    #endregion
    #region 事件
    [Tooltip("魔法中的发射事件")] public UnityEvent magicEvent;
    #endregion
    public float MouseDegree => mousedegree;
    public Vector3 MouseDistance => mouseDistance;
    private void Awake()
    {
        #region 组件获取
        m_camera = Camera.main;
        attacker = null;
        chargeVFX = chargeVFXobj.GetComponent<ParticleSystem>();
        #endregion
        //
        memory = InputMemory.None;
        //
        CanAcceptInput = true;
        //
        CanInterAction = false;
        //
        dict = new Dictionary<Type, IState>(stateTable.Length);
        //
        energyCostDict = new Dictionary<Type, float>(energyCost.Length);
        //
        playerTransform = transform;
        foreach (PlayerState playerState in stateTable)
        {
            playerState.Initialization(playerInput, this, playerController, playerAnimator, playerRenderer, player, playerData, shooter, bigLight, playerTip, LightAtk_1, LightAtk_2, LightAtk_3, LightAtk_4, BackAttack, RightAttack_1,RightAttack_2,RightAttack_3);
            dict.Add(playerState.GetType(), playerState);
        }
        for(int i = 0;i < stateTable.Length; ++i)
        {
            energyCostDict.Add(stateTable[i].GetType(), energyCost[i]);
        }
    }

    private void Start()
    {
        SwitchOn(dict[typeof(PlayerState_Idle)]);
        noStunTimer = -1;
        player.damageableIndex = 0;
    }

    protected override void Update()
    {
        base.Update();
        UpdateMouseDegree();
        //更新法术冷却UI
        for(int i = 0;i < magicTimer.Length; i++)
        {
            if (playerData.magicUnlockState[i])
            {
                if (magicTimer[i] > 0)
                    magicTimer[i]-=Time.deltaTime;

                UpdateMagicUI(i);
            }
        }

        //无硬直无敌
        if(noStunTimer > -1)
        {
            noStunTimer -= Time.deltaTime;
            if(noStunTimer < 0)
            {
                player.damageableIndex = 0;
                noStunTimer = -1;
            }
        }

        //体力恢复
        if(energyRecoverTimer <= 0)
        {
            if (playerData.CurrentEnergy >= 0)
                playerData.CurrentEnergy += playerData.FinalEnergyRecovery * Time.deltaTime;
            else
                playerData.CurrentEnergy += (playerData.FinalEnergyRecovery / 2) * Time.deltaTime;
        }
        else
        {
            energyRecoverTimer -= Time.deltaTime;
        }
    }
    public bool ContinuousConsumeEnergy(float value)
    {
        if(playerData.CurrentEnergy < 0)
        {
            return false;
        }
        else
        {
            playerData.CurrentEnergy -= value;
            energyRecoverTimer = energyRecoverDelay;
            return true;
        }
    }
    public override void SwitchState(Type newState)
    {
        if (newState == typeof(PlayerState_RightAttack))
        {
            playerData.CurrentEnergy -= energyCostDict[newState] * playerData.energyCostMultiplication;
            energyRecoverTimer = energyRecoverDelay;
            base.SwitchState(newState);
        }
        else if (playerData.CurrentEnergy >= 0 || energyCostDict[newState] == 0)
        {
            if (energyCostDict[newState] != 0)
            {
                playerData.CurrentEnergy -= energyCostDict[newState] * playerData.energyCostMultiplication;
                energyRecoverTimer = energyRecoverDelay;
            }
            base.SwitchState(newState);
        }
    }


    /// <summary>
    /// 计算鼠标和玩家的夹角值
    /// </summary>
    private void UpdateMouseDegree()
    {
        Vector3 mmouseDistance = m_camera.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;
        mouseDistance = new Vector3(mmouseDistance.x, mmouseDistance.y, 0f);
        float degree = Mathf.Atan2(mouseDistance.y, mouseDistance.x) * Mathf.Rad2Deg;
        mousedegree = degree>=0?degree:360f+degree;
    }
    /// <summary>
    /// 计算在有转向限制下的夹角值
    /// </summary>
    /// <param name="lastobj"></param>
    /// <returns>返回最终物体应该朝向的角度</returns>
    public float RestrictedRotation(GameObject lastobj)
    {
        float mousedeg = mousedegree;
        float result;
        float z = lastobj.transform.localEulerAngles.z >= 0?lastobj.transform.localEulerAngles.z:360f + lastobj.transform.localEulerAngles.z;
        if (Mathf.Abs(mousedeg - z) > playerController.LightAtkRotateAngle)
        {
            result = z + (mousedeg - z > 0 ? playerController.LightAtkRotateAngle : -playerController.LightAtkRotateAngle);
        }
        else
        {
            result = mousedeg;
        }
        return result;
    }
    public void ReturnAnimatorValue_Update()
    {
        bool isUp = mouseDistance.y >= 0;
        bool isHorizontal = Mathf.Abs(mouseDistance.x) > Mathf.Abs(mouseDistance.y);
        bool isSameDirtction = Vector3.Dot(mouseDistance,playerController.MoveAxis) > 0;
        playerAnimator.SetFloat("isUp",isUp ? 1 : 0);
        playerAnimator.SetFloat("isHorizontal", isHorizontal ? 1 : 0);
        playerAnimator.SetFloat("isSameDirection",isSameDirtction ? 1 : 0);
        playerController.isSameDirection = isSameDirtction;
    }

    public void ReturnAnimatorValue_OnStart_Mouse()
    {
        bool isUp = mouseDistance.y >= 0;
        bool isHorizontal = Mathf.Abs(mouseDistance.x) > Mathf.Abs(mouseDistance.y);
        playerAnimator.SetFloat("isUp", isUp ? 1 : 0);
        playerAnimator.SetFloat("isHorizontal", isHorizontal ? 1 : 0);
    }

    public void ReturnAnimatorValue_OnStart_Input(Vector2 faceDir)
    {
        bool isUp = faceDir.y > 0;
        bool isHorizontal = Mathf.Abs(faceDir.y) <= Mathf.Abs(faceDir.x);
        playerAnimator.SetFloat("isUp", isUp ? 1 : 0);
        playerAnimator.SetFloat("isHorizontal", isHorizontal ? 1 : 0);
    }
    public void ReturnAnimatorValue_OnHurt()
    {
        Vector3 direction = (attacker.position - playerTransform.position).normalized;
        bool isUp = direction.y >= 0;
        bool isRight = direction.x > 0;
        bool isHorizontal = Mathf.Abs(direction.y) <= Mathf.Abs(direction.x);
        playerAnimator.SetFloat("isUp", isUp ? 1 : 0);
        playerAnimator.SetFloat("isHorizontal", isHorizontal ? 1 : 0);
        playerRenderer.flipX = isRight;
    }

    /// <summary>
    /// 接受输入帧
    /// </summary>
    public void AcceptInput()
    {
        CanAcceptInput = true;
    }
    /// <summary>
    /// 动画最早打断时间
    /// </summary>
    public void AcceptStateSwitch()
    {
        CanStateSwitch = true;
    }
    /// <summary>
    /// 魔法事件触发
    /// </summary>
    public void MagicInvoke()
    {
        magicEvent?.Invoke();
    }

    public void BackAttackOnAttack(IDamageable damageable)
    {
        successfulBack = true;
    }
    /// <summary>
    /// 更新冷却时间
    /// </summary>
    /// <param name="id">魔法索引</param>
    public void UpdateMagicUI(int id)
    {
        if (magicTimer[id] > 0)
        {
            MagicUIManager.Instance.UpdateMask(id, magicTimer[id] / magicColdDown[id]);
        }
        else
        {
            MagicUIManager.Instance.UpdateMask(id, 0);
        }
    }
    /// <summary>
    /// 蓄力开始时根据蓄力段数更换攻击判定物体
    /// </summary>
    public void SetStage()
    {
        switch (chargeStage) { 
            case 0:
                RightAttack = RightAttack_0;
                break;
            case 1:
                RightAttack = RightAttack_1;
                break;
            case 2:
                RightAttack = RightAttack_2;
                break;
            case 3:
                RightAttack = RightAttack_3;
                break;
        }
    }

    public void PlayRtAtkAnimation()
    {
        Animator animator = RightAttack.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Enabled");
        }
    }
    /// <summary>
    /// 启用或禁用重击攻击判定
    /// </summary>
    public void Set_RTAtkTrigger()
    {
        Collider2D collider = RightAttack.GetComponent<Collider2D>();
        if (collider != null)
        {
            if (collider.enabled)
            {
                collider.enabled = false;
            }
            else
                collider.enabled = true;
        }
    }
    #region 受伤和死亡时触发的方法
    /// <summary>
    /// 无硬直触发方法
    /// </summary>
    /// <param name="attacker">攻击者</param>
    public void OnNoStun(Transform attacker)
    {
        player.damageableIndex = 1;
        noStunTimer = noStunUndamageableTime;
    }
    /// <summary>
    /// 小硬直触发方法
    /// </summary>
    /// <param name="attacker">攻击者</param>
    public void OnSmallStun(Transform attacker)
    {
        this.attacker = attacker;
        SwitchState(typeof(PlayerState_SmallStun));
    }
    /// <summary>
    /// 普通硬直触发方法
    /// </summary>
    /// <param name="attacker">攻击者</param>
    public void OnNormalStun(Transform attacker)
    {
        this.attacker = attacker;
        SwitchState(typeof(PlayerState_NormalStun));
    }
    /// <summary>
    /// 大硬直触发方法
    /// </summary>
    /// <param name="attacker">攻击者</param>
    public void OnBigStun(Transform attacker)
    {
        this.attacker = attacker;
        SwitchState(typeof(PlayerState_BigStun));
    }
    /// <summary>
    /// 死亡触发方法
    /// </summary>
    public void OnDead()
    {
        SwitchState(typeof(PlayerState_Dead));
    }
    #endregion

    #region 事件中音效相关

    public void PlayAtk1()
    {
        AudioManager.Instance.PlaySFX(attack_1, transform.position);
    }
    public void PlayAtk4()
    {
        AudioManager.Instance.PlaySFX(attack_4, transform.position);
    }

    #endregion
    public void GameOver()
    {
        GameManager.Instance.PlayerDeath();
    }
    
}


