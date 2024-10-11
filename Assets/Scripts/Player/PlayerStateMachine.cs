using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStateMachine : StateMachine
{
    [SerializeField] Camera m_camera;
    public Transform playerTransform;
    #region 鼠标相关
    [Header("鼠标相关")]
    [SerializeField] float mousedegree;
    [SerializeField]PlayerState[] stateTable;
    [SerializeField] Vector3 mouseDistance;
    #endregion
    #region 特定时间节点
    [Header("特定时间节点")]
    [Tooltip("见切完美判定和无敌帧判定区间中点")]public float CatchChancepoint; 
    #endregion
    #region 组件
    //获取组件的方式之后可以调整
    public PlayerInput playerInput;
    public PlayerController playerController;
    public Animator playerAnimator;
    public SpriteRenderer playerRenderer;
    public Player player;
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
        dict = new Dictionary<System.Type, IState>(stateTable.Length);
        //
        playerTransform = transform;
        foreach (PlayerState playerState in stateTable)
        {
            playerState.Initialization(playerInput, this,playerController,playerAnimator,playerRenderer,player,LightAtk_1,LightAtk_2,LightAtk_3,LightAtk_4,BackAttack,RightAttack);
            dict.Add(playerState.GetType(), playerState);
        }
    }

    private void Start()
    {
        SwitchOn(dict[typeof(PlayerState_Idle)]);
    }

    protected override void Update()
    {
        base.Update();
        UpdateMouseDegree();
    }

    private void UpdateMouseDegree()
    {
        mouseDistance = m_camera.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;
        float degree = Mathf.Atan2(mouseDistance.y, mouseDistance.x) * Mathf.Rad2Deg;
        mousedegree = degree>=0?degree:360f+degree;
    }

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
        bool isSameDirtction = Vector3.Dot(mouseDistance,playerController.MoveAxis) > 0;
        playerAnimator.SetFloat("isUp",isUp ? 1 : 0);
        playerAnimator.SetFloat("isSameDirection",isSameDirtction ? 1 : 0);
    }

    public void ReturnAnimatorValue_OnStart()
    {
        bool isUp = mouseDistance.y >= 0;
        playerAnimator.SetFloat("isUp", isUp ? 1 : 0);
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
    #region 受伤和死亡时触发的方法
    private void OnNoStun(Transform attacker)
    {

    }

    private void OnSmallStun(Transform attacker)
    {

    }

    private void OnNormalStun(Transform attacker)
    {

    }

    private void OnBigStun(Transform attacker)
    {

    }
    private void OnDead()
    {

    }
    #endregion
}


