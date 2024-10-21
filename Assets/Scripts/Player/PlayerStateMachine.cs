using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerStateMachine : StateMachine
{
    [SerializeField] Camera m_camera;
    public Transform playerTransform;
    public Transform attacker;

    [SerializeField] PlayerState[] stateTable;
    [SerializeField] float[] energyCost;
    Dictionary<Type, float> energyCostDict;
    #region 鼠标相关
    [Header("鼠标相关")]
    [SerializeField] float mousedegree;
    [SerializeField] Vector3 mouseDistance;
    #endregion
    #region 特定时间节点
    [Header("特定时间节点")]
    [Tooltip("见切完美判定和无敌帧判定区间中点")]public float CatchChancepoint;
    [Tooltip("无硬直无敌时间")] public float noStunUndamageableTime;
    //无敌帧计时器
    private float noStunTimer;
    #endregion
    #region 体力恢复相关参数
    [Tooltip("体力开始恢复计时器")][SerializeField]private float energyRecoverTimer;
    [Tooltip("消耗体力到体力开始恢复的延迟时间")][SerializeField]private float energyRecoverDelay;
    #endregion

    #region 组件
    //获取组件的方式之后可以调整
    public PlayerInput playerInput;
    public PlayerController playerController;
    public Animator playerAnimator;
    public SpriteRenderer playerRenderer;
    public Player player;
    public PlayerData playerData;
    public PlayerShooter shooter;
    public GameObject bigLight;

    public GameObject LightAtk_1;
    public GameObject LightAtk_2;
    public GameObject LightAtk_3;
    public GameObject LightAtk_4;
    public GameObject BackAttack;
    public GameObject RightAttack;
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
    #region 事件
    [Tooltip("魔法中的发射事件")] public UnityEvent magicEvent;
    #endregion
    public float MouseDegree => mousedegree;
    public Vector3 MouseDistance => mouseDistance;
    private void Awake()
    {
        #region 组件获取
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
            playerState.Initialization(playerInput, this,playerController,playerAnimator,playerRenderer,player,playerData,shooter,bigLight,LightAtk_1,LightAtk_2,LightAtk_3,LightAtk_4,BackAttack,RightAttack);
            dict.Add(playerState.GetType(), playerState);
        }
    }

    private void Start()
    {
        SwitchOn(dict[typeof(PlayerState_Idle)]);
        noStunTimer = -1;
    }

    protected override void Update()
    {
        base.Update();
        UpdateMouseDegree();
        if(noStunTimer > 0 && noStunTimer > -1)
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

    public override void SwitchState(Type newState)
    {
        if(playerData.CurrentEnergy >= 0 || energyCostDict[newState] == 0)
        {
            if(energyCostDict[newState] != 0)
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
        mouseDistance = m_camera.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;
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
    }

    public void ReturnAnimatorValue_OnStart()
    {
        bool isUp = mouseDistance.y >= 0;
        bool isHorizontal = Mathf.Abs(mouseDistance.x) > Mathf.Abs(mouseDistance.y);
        playerAnimator.SetFloat("isUp", isUp ? 1 : 0);
        playerAnimator.SetFloat("isHorizontal", isHorizontal ? 1 : 0);
    }

    public void ReturnAnimatorValue_OnHurt()
    {
        Vector3 direction = (attacker.position - playerTransform.position).normalized;
        bool isUp = direction.y >= 0;
        bool isRight = direction.x > 0;
        playerAnimator.SetFloat("isUp", isUp ? 1 : 0);
        playerRenderer.flipX = isRight;
    }

    //接受输入帧
    public void AcceptInput()
    {
        CanAcceptInput = true;
    }
    //动画最早打断时间
    public void AcceptStateSwitch()
    {
        CanStateSwitch = true;
    }
    //魔法事件触发
    public void MagicInvoke()
    {
        magicEvent?.Invoke();
    }
    #region 受伤和死亡时触发的方法
    public void OnNoStun(Transform attacker)
    {
        player.damageableIndex = 1;
        noStunTimer = noStunUndamageableTime;
    }

    public void OnSmallStun(Transform attacker)
    {
        this.attacker = attacker;
        SwitchState(typeof(PlayerState_SmallStun));
    }

    public void OnNormalStun(Transform attacker)
    {

    }

    public void OnBigStun(Transform attacker)
    {

    }
    public void OnDead()
    {

    }
    #endregion
    #region 触发器相关
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Interaction"))
        {
            CanInterAction = true;
            interactionObj = collision.gameObject.GetComponent<InteractiveComponent>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interaction"))
        {
            CanInterAction = false;
            interactionObj = null;
        }
    }
    #endregion
}


