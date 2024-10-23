using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{   InGameInput input;

    private bool isPlayerInputEnable = false;

    public bool IsPlayerInputEnable
    {
        get
        {
            return isPlayerInputEnable;
        }
        set
        {
            isPlayerInputEnable=value;
        }
    }

    #region 玩家输入

    public bool MoveUp => input.Player.MoveUp.IsPressed();

    public bool UpCheck => input.Player.MoveUp.WasPressedThisFrame();

    public bool MoveDown => input.Player.MoveDown.IsPressed();

    public bool DownCheck => input.Player.MoveDown.WasPressedThisFrame();

    public bool MoveLeft => input.Player.MoveLeft.IsPressed();

    public bool LeftCheck => input.Player.MoveLeft.WasPressedThisFrame();

    public bool MoveRight => input.Player.MoveRight.IsPressed();

    public bool RightCheck => input.Player.MoveRight.WasPressedThisFrame();

    public bool IsRun => input.Player.Run.IsPressed();

    public bool LightAttack => input.Player.LightAttack.WasPressedThisFrame();

    public bool RightAttack => input.Player.RightAttack.WasPressedThisFrame();

    public bool Charging => input.Player.RightAttack.IsPressed();

    public bool ChargeRelease => input.Player.RightAttack.WasReleasedThisFrame();

    public bool Magic_1 => input.Player.Magic_1.WasPressedThisFrame();

    public bool Magic_2 => input.Player.Magic_2.WasPressedThisFrame();

    public bool Magic_3 => input.Player.Magic_3.WasPressedThisFrame();
    public bool Roll => input.Player.Roll.WasPressedThisFrame();

    public bool UseHealthBottle => input.Player.Health.WasPressedThisFrame();

    public bool UseManaBottle => input.Player.Mana.WasPressedThisFrame();

    public bool Interaction => input.Player.Interaction.WasPressedThisFrame();
    #endregion

    #region 输入检测拓展

    public bool WantsMove => MoveUp || MoveDown || MoveLeft || MoveRight;
    #endregion

    private void Awake()
    {
        input = new InGameInput();
        EnablePlayerInput();
    }

    public void EnablePlayerInput()
    {
        input.Player.Enable();
        IsPlayerInputEnable = true;
    }

    public void DisablePlayerInput() 
    {
        input.Player.Disable();
        IsPlayerInputEnable = false;
    }
}


public enum InputMemory
{
    None,
    Roll,
    LightAttack,
    RightAttack,
    Direction,
}

