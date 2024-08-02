using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float MoveSpeed;
    public float JumpForce;
    private float jumpTimer;
    private float jumpCD = 0.1f;
    private Rigidbody2D rb;
    public Rigidbody2D springRb;
    private Collider2D _collider;
    private bool isGround;
    public bool IsGround { get { return isGround; } }
    private float coyoteTimeTimer;//土狼时间
    private PhysicsMaterial2D normalPhysicMat2D;
    private PhysicsMaterial2D smoothPhysicMat2D;

    public Transform[] rayCheckPoints;
    public System.Action onLandAction;

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
    public void OnLand()
    {
        onLandAction?.Invoke();
    }
    public void HorizontalMove(float value)
    {
       
        if (value == 0f) return;
        rb.velocity = new Vector2(value * MoveSpeed, rb.velocity.y);


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
        if(isGround)
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
            if (Physics2D.Raycast(rayCheckPoints[i].position, Vector2.down, 0.1f, (1 << 7)))
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

        if (IsGetKeyDown && (coyoteTimeTimer >0f|| isGround) && jumpTimer + jumpCD <= Time.time)
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
        if (InputMgr.GetSpace() && rb.velocity.y > 0)
        {
            rb.gravityScale = 1f;
        }
        else
        {
            rb.gravityScale = 2;
        }
    }
    public float GetVerticalSpeed()
    {
        return rb.velocity.y;
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
        jumpTimer = value;
    }
}
