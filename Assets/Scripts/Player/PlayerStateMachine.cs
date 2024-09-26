using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [SerializeField] Camera m_camera;
    private Transform playerTransform;
    [SerializeField] float mousedegree;
    [SerializeField]PlayerState[] stateTable;
    [SerializeField] Vector3 mouseDistance;
    #region 组件
    //获取组件的方式之后可以调整
    public PlayerInput playerInput;
    public PlayerController playerController;
    public Animator playerAnimator;
    public SpriteRenderer playerRenderer;
    public GameObject LightAtk_1;
    public GameObject LightAtk_2;
    public GameObject LightAtk_3;
    #endregion

    #region 接受输入窗口
    public bool CanAcceptInput { get; set; }
    #endregion
    public float MouseDegree => mousedegree;
    public Vector3 MouseDistance => mouseDistance;
    private void Awake()
    {
        CanAcceptInput = true;
        //
        dict = new Dictionary<System.Type, IState>(stateTable.Length);
        //
        playerTransform = transform;
        foreach (PlayerState playerState in stateTable)
        {
            playerState.Initialization(playerInput, this,playerController,playerAnimator,playerRenderer,LightAtk_1,LightAtk_2,LightAtk_3);
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
        mousedegree = degree;
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
}
