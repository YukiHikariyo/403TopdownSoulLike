using System.Collections;
using System.Collections.Generic;
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

    protected GameObject lightAtk_1;
    protected GameObject lightAtk_2;
    protected GameObject lightAtk_3;
    #endregion

    #region 通用变量
    //记录动画开始时间
    protected float stateStartTime;
    //计算动画播放时间
    protected float StateDuration => Time.time - stateStartTime;

    protected float AnimationLength => playerAnimator.GetCurrentAnimatorStateInfo(0).length;
    protected bool IsAnimationEnd => StateDuration >= AnimationLength;
    //指定方向且在动作进行中不能切换的动作的方向
    protected Vector2 FaceDir;
    #endregion

    public void Initialization(PlayerInput playerInput,
        PlayerStateMachine playerStateMachine,
        PlayerController playerController,
        Animator playerAnimator,
        SpriteRenderer playerRenderer,
        GameObject lightAtk_1,
        GameObject lightAtk_2,
        GameObject lightAtk_3)
    {
        this.playerInput = playerInput;
        this.playerStateMachine = playerStateMachine;
        this.playerController = playerController;
        this.playerAnimator = playerAnimator;
        this.playerRenderer = playerRenderer;
        this.lightAtk_1 = lightAtk_1;
        this.lightAtk_2 = lightAtk_2;
        this.lightAtk_3 = lightAtk_3;
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
            playerRenderer.flipX = true;
        else
            playerRenderer.flipX = false;
    }

    protected void SetAnimator_OnStart()
    {
        ChangeScale();
        playerStateMachine.ReturnAnimatorValue_OnStart();
    }

    protected void SetAnimator_Update()
    {
        ChangeScale();
        playerStateMachine.ReturnAnimatorValue_Update();
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

