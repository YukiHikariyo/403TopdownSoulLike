using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [SerializeField] Camera m_camera;
    private Transform playerTransform;
    [SerializeField] float mousedegree;
    public PlayerInput playerInput;
    public PlayerController playerController;
    [SerializeField]PlayerState[] stateTable;

    public float MouseDegree => mousedegree;
    private void Awake()
    {
        //
        dict = new Dictionary<System.Type, IState>(stateTable.Length);
        //

        foreach (PlayerState playerState in stateTable)
        {
            playerState.Initialization(playerInput, this,playerController);
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
        Vector2 vec = m_camera.WorldToScreenPoint(Input.mousePosition) - playerTransform.position;
        float degree = Mathf.Atan2(vec.y,vec.x) * Mathf.Deg2Rad + (playerTransform.localScale.x > 0?0:180f);
        mousedegree = degree;
    }
}
