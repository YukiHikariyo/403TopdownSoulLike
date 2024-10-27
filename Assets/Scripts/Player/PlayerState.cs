using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerState : ScriptableObject, IState
{
    
    # region 组件声明
    protected PlayerInput playerInput;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerController playerController;
    protected Animator playerAnimator;
    protected SpriteRenderer playerRenderer;
    protected Player player;
    protected PlayerData playerData;
    protected PlayerShooter shooter;
    protected GameObject bigLight;
    protected AttackArea backAttackArea;
    protected SpriteRenderer playerTip;

    protected GameObject lightAtk_1;
    protected GameObject lightAtk_2;
    protected GameObject lightAtk_3;
    protected GameObject lightAtk_4;
    protected GameObject BackAttack;

    protected GameObject RightAttack_1;
    protected GameObject RightAttack_2;
    protected GameObject RightAttack_3;
    #endregion

    #region 通用变量
    //记录动画开始时间
    protected float stateStartTime;
    //计算动画播放时间
    protected float StateDuration => (Time.time - stateStartTime) * playerAnimator.speed;

    protected float AnimationLength => playerAnimator.GetCurrentAnimatorStateInfo(0).length;
    protected bool IsAnimationEnd => StateDuration >= AnimationLength;
    //指定方向且在动作进行中不能切换的动作的方向
    protected Vector2 FaceDir;
    #endregion

    public void Initialization
        (//常规组件
        PlayerInput playerInput,
        PlayerStateMachine playerStateMachine,
        PlayerController playerController,
        Animator playerAnimator,
        SpriteRenderer playerRenderer,
        Player player,
        PlayerData playerData,
        PlayerShooter shooter,
        GameObject bigLight,
        SpriteRenderer playerTip,
        //攻击Trigger
        GameObject lightAtk_1,
        GameObject lightAtk_2,
        GameObject lightAtk_3,
        GameObject lightAtk_4,
        GameObject BackAttack,
        GameObject RightAttack_1,
        GameObject RightAttack_2,
        GameObject RightAttack_3
        )
    {
        this.playerInput = playerInput;
        this.playerStateMachine = playerStateMachine;
        this.playerController = playerController;
        this.playerAnimator = playerAnimator;
        this.playerRenderer = playerRenderer;
        this.player = player;
        this.playerData = playerData;
        this.shooter = shooter;
        this.bigLight = bigLight;
        this.playerTip = playerTip;

        this.lightAtk_1 = lightAtk_1;
        this.lightAtk_2 = lightAtk_2;
        this.lightAtk_3 = lightAtk_3;
        this.lightAtk_4 = lightAtk_4;
        this.BackAttack = BackAttack;
        backAttackArea = BackAttack.GetComponent<AttackArea>();
        this.RightAttack_1 = RightAttack_1;
        this.RightAttack_2 = RightAttack_2;
        this.RightAttack_3 = RightAttack_3;
    }
    public virtual void Enter()
    {
        stateStartTime = Time.time;
        playerStateMachine.memory = InputMemory.None;
    }

    public virtual void Exit()
    {
        playerStateMachine.CanAcceptInput = true;
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicUpdate()
    {
        
    }

    private void ChangeScale()
    {
        if(playerStateMachine.MouseDistance.x < 0)
            playerRenderer.flipX = false;
        else
            playerRenderer.flipX = true;
    }
    /// <summary>
    /// 仅在动作开始时更新面朝方向,方向面向鼠标方向
    /// </summary>
    protected void SetAnimator_OnStart_Mouse()
    {
        ChangeScale();
        playerStateMachine.ReturnAnimatorValue_OnStart_Mouse();
    }
    /// <summary>
    /// 仅在动作开始时更新面朝方向,方向为输入方向或鼠标移动受限状态时使用
    /// </summary>
    protected void SetAnimator_OnStart_Input()
    {
        if (FaceDir.x >= 0)
            playerRenderer.flipX = true;
        else
            playerRenderer.flipX = false;
        playerStateMachine.ReturnAnimatorValue_OnStart_Input(FaceDir);
    }
    /// <summary>
    /// 逐帧更新面朝方向
    /// </summary>
    protected void SetAnimator_Update()
    {
        ChangeScale();
        playerStateMachine.ReturnAnimatorValue_Update();
    }
    /// <summary>
    /// 受伤时的面朝方向
    /// </summary>
    protected void SetAnimator_OnHurt()
    {
        playerStateMachine.ReturnAnimatorValue_OnHurt();
    }
    //  所有切换逻辑

/*  //切换至移动
        if (PlayerInput.WantsMove)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Move));
        }
    //切换至常态
        if (!playerInput.WantsMove)
            playerStateMachine.SwitchState(typeof(PlayerState_Idle));
    //切换至第一段翻滚
        //等待添加耐力限制
        if(playerInput.Roll)
            playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
*/
}

