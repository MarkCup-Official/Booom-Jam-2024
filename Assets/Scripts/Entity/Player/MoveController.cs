using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private float MoveSpeed = 3.5f;
    private float JumpForce = 10f;
    public float airResistanceCoefficient = 1;
    private float jumpTimer;
    private float jumpCD = 0.1f;
    private bool isUsingGravity = true;

    private Rigidbody2D rb;
    public Rigidbody2D springRb;
    private Collider2D _collider;
    private bool isUnderControl = true;

    private bool isGround;
    public bool isTouchWater { get { return WaterTouchedCount > 0; } }
    public int WaterTouchedCount { get; set; }
    public bool isWearingDivingSuit;
    public bool isTouchLadder { get; set; }
    public bool IsGround { get { return isGround; } }
    private float coyoteTimeTimer;//土狼时间
    private PhysicsMaterial2D normalPhysicMat2D;
    private PhysicsMaterial2D smoothPhysicMat2D;

    public Transform[] rayCheckPoints;
    public System.Action onLandAction;
    private const int groundLayerMask = (1 << 7)|(1<<13);

    //走路晃动
    public float WalkShakeStrength=1;
    public float WalkShakeFrequency= 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        normalPhysicMat2D = Resources.Load<PhysicsMaterial2D>("Config/NormalMat");
        smoothPhysicMat2D = Resources.Load<PhysicsMaterial2D>("Config/SmoothMat");
    }
    private void Start()
    {
        jumpTimer = Time.time;
    }
    private void Update()
    {
        isGround = CheckIsGround();
        CoyoteTimer();
        ChangePhysicMat();
        BetterGravity();

    }
    private void FixedUpdate()
    {
        //空气阻力
        rb.AddForce(-rb.velocity * airResistanceCoefficient);
        if(IsGround && GetHorizontalSpeed()!=0f)
        springRb.AddForce(WalkShakeStrength * Vector2.up * Mathf.Sin(Time.time * WalkShakeFrequency));
    }
    /// <summary>
    /// 落地的瞬间触发
    /// </summary>
    public void OnLand()
    {
        onLandAction?.Invoke();
    }
    public void HorizontalMove(float value)
    {
        if (!isUnderControl) return;

        if (value == 0f) return;
        rb.velocity = new Vector2(value * MoveSpeed, rb.velocity.y);
    }
    public void VerticalMove(float value)
    {
        if (!isUnderControl) return;
        if (value == 0f) return;
        rb.velocity = new Vector2(rb.velocity.x, value);

    }
    /// <summary>
    /// 在空中切换成光滑物理材质，防止卡墙
    /// </summary>
    private void ChangePhysicMat()
    {
        if (isGround) _collider.sharedMaterial = normalPhysicMat2D;
        else _collider.sharedMaterial = smoothPhysicMat2D;
    }
    private void CoyoteTimer()
    {
        if (isGround)
            coyoteTimeTimer = 0.1f;
        else
            coyoteTimeTimer -= Time.deltaTime;
    }
    /// <summary>
    /// 检测角色是否在地面上
    /// </summary>
    /// <returns></returns>
    private bool CheckIsGround()
    {
        for (int i = 0; i < rayCheckPoints.Length; i++)
        {
            if (Physics2D.Raycast(rayCheckPoints[i].position, Vector2.down, 0.1f, groundLayerMask))
            {
                if (!isGround)
                    OnLand();

                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// handle jump logic
    /// </summary>

    public void JumpLogic(bool IsGetKeyDown)
    {
        if (!isUnderControl) return;
        if (IsGetKeyDown && (coyoteTimeTimer > 0f || isGround || isTouchWater) && jumpTimer + jumpCD <= Time.time)
        {
            coyoteTimeTimer = 0f;
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            springRb.velocity = new Vector2(springRb.velocity.x, 2 * JumpForce);
            jumpTimer = Time.time;
        }
    }
    /// <summary>
    /// 当玩家长按空格，跳跃高度会越高
    /// </summary>
    public void BetterGravity()
    {

        if (!isUsingGravity)
        {
            rb.gravityScale = 0f;
            return;
        }

        if (InputMgr.GetSpace() && rb.velocity.y > 0 || isTouchWater)
        {
            rb.gravityScale = 1.5f;
        }
        else
        {
            rb.gravityScale = 2.5f;
        }
    }
    public float GetVerticalSpeed()
    {
        return rb.velocity.y;
    }
    public float GetHorizontalSpeed()
    {
        return rb.velocity.x;
    }
    public Vector2 GetSpeed()
    {
        return rb.velocity;
    }
    public void SetSpeed(float value)
    {
        this.MoveSpeed = value;
    }
    public void SetJumpForce(float value)
    {
        JumpForce = value;
    }

    public Rigidbody2D GetRigidBody()
    {
        return rb;
    }
    public void SetGravity(bool isUsingGravity)
    {
        this.isUsingGravity = isUsingGravity;
    }

    public void SetIsUnderControl( bool isUnderControl)
    {
        this.isUnderControl = isUnderControl;
    }
}
