using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private float MoveSpeed = 3.5f;
    private float JumpForce = 10f;
    public float BuoyancyForce = 10;
    public float WaterResistance = 0.5f;
    public float WaterHeight = 0;
    private float jumpTimer;
    private float jumpCD = 0.1f;
    private bool isUsingGravity = true;
    private Rigidbody2D rb;
    public Rigidbody2D springRb;
    private Collider2D _collider;
    private bool isGround;
    public bool isTouchWater { get; set; }
    public bool isTouchLadder { get; set; }
    public bool IsGround { get { return isGround; } }
    private float coyoteTimeTimer;//����ʱ��
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
    public void VerticalMove(float value)
    {
        if (value == 0f) return;
        rb.velocity = new Vector2(rb.velocity.x, value);
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
        if (isGround)
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

        if (IsGetKeyDown && (coyoteTimeTimer > 0f || isGround || isTouchWater) && jumpTimer + jumpCD <= Time.time)
        {
            coyoteTimeTimer = 0f;
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            springRb.velocity = new Vector2(springRb.velocity.x, 2 * JumpForce);
            jumpTimer = Time.time;
        }
    }
    /// <summary>
    /// ����ҳ����ո���Ծ�߶Ȼ�Խ��
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

    //���ˮ�ĸ��� �� ����
    public void AddWaterForce()
    {
        if (!isTouchWater) return;

        float distance = WaterHeight - transform.position.y + 1;
        distance = Mathf.Clamp(distance, -1, 1);
        if(isTouchWater && distance > 0)
        rb.AddForce(new Vector2(0, BuoyancyForce * distance));

        rb.AddForce(-WaterResistance * rb.velocity);
    }
    public Rigidbody2D GetRigidBody()
    {
        return rb;
    }
    public void SetGravity(bool isUsingGravity)
    {
        this.isUsingGravity = isUsingGravity;
    }
}
