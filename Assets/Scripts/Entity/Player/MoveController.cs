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
    private Collider2D _collider;
    private bool isGround;
    public bool IsGround { get { return isGround; } }
    private float coyoteTimeTimer;//����ʱ��
    private PhysicsMaterial2D normalPhysicMat2D;
    private PhysicsMaterial2D smoothPhysicMat2D;

    public Transform[] rayCheckPoints;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
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

    public void HorizontalMove(float value)
    {
        if (value == 0f) return;
        rb.velocity = new Vector2(value * MoveSpeed, rb.velocity.y);
    }
    /// <summary>
    /// �ڿ����л��ɹ⻬������ʣ���ֹ��ǽ
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
    /// ����ɫ�Ƿ��ڵ�����
    /// </summary>
    /// <returns></returns>
    private bool CheckIsGround()
    {
        for (int i = 0; i < rayCheckPoints.Length; i++)
        {
            if (Physics2D.Raycast(rayCheckPoints[i].position, Vector2.down, 0.1f, (1 << 7)))
            {
               
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
            jumpTimer = Time.time;
        }
    }
    /// <summary>
    /// ����ҳ����ո���Ծ�߶Ȼ�Խ��
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
    public void SetSpeed(float value)
    {
        this.MoveSpeed = value;
    }
    public void SetJumpForce(float value)
    {
        jumpTimer = value;
    }
}
