using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public PlayerInput playerInput;
    public PlayerController playerController;
    [SerializeField]PlayerState[] stateTable;
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

}
