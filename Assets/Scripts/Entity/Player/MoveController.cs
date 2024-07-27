using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float MoveSpeed = 10;
    public float JumpForce = 5;
    private float jumpTimer;
    private float jumpCD = 0.1f;
    private Rigidbody2D rb;
    private bool isGround;
    private float coyoteTimeTimer;//土狼时间

    public Transform[] rayCheckPoints;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        jumpTimer = Time.time;
    }
    private void Update()
    {
        isGround = CheckIsGround();

        BetterGravity();
    }

    public void HorizontalMove(float value)
    {
        if (value == 0f) return;
        rb.velocity = new Vector2(value * MoveSpeed, rb.velocity.y);
    }
    /// <summary>
    /// 检测角色是否在地面上
    /// </summary>
    /// <returns></returns>
    private bool CheckIsGround()
    {
        for (int i = 0; i < rayCheckPoints.Length; i++)
        {
            if (Physics2D.Raycast(rayCheckPoints[i].position, Vector2.down, 0.1f, ~(1 << 3)))
            {
                coyoteTimeTimer = 0.1f;
                return true;
            }
        }
        coyoteTimeTimer -= Time.deltaTime;
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
            jumpTimer = Time.time;
        }
    }
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
}
