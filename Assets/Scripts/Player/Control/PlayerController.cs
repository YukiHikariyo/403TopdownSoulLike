using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public PlayerInput inputs;

    [Header("玩家移动属性")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fastRollSpeed;
    [SerializeField] private float slowRollSpeed;
    [SerializeField] private float lastRollDuration;
    [SerializeField] private float L1AtkSpeed;
    [SerializeField] private float L2AtkSpeed;
    [SerializeField] private float L3AtkSpeed;
    [SerializeField] private float L4AtkSpeed;
    [SerializeField] public float LightAtkRotateAngle;
    [Header("运动曲线")]
    public AnimationCurve fastRollCruve;
    public AnimationCurve slowRollCruve;
    //每段攻击的位移变化曲线
    public AnimationCurve L1AtkCruve;
    public AnimationCurve L2AtkCruve;
    public AnimationCurve L3AtkCruve;
    public AnimationCurve L4AtkCruve;
    private float nowSpeed;
    //二段翻滚的移动速度会由快变慢，该参数用于表示减速度，为了方便，向属性传递动作持续时间，属性内部会自行计算减速度
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
        playerRb.velocity = MoveAxis * moveSpeed;
    }
    #endregion
    #region 翻滚
    public void FastRoll(Vector2 FaceDir,float time)
    {
        playerRb.velocity = FaceDir * fastRollSpeed * fastRollCruve.Evaluate(time);
    }
  
    public void SlowRoll(Vector2 FaceDir, float time)
    {
        playerRb.velocity = FaceDir * slowRollSpeed * slowRollCruve.Evaluate(time);
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
    }
}
