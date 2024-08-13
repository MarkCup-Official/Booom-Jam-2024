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
    private float coyoteTimeTimer;//����ʱ��
    private PhysicsMaterial2D normalPhysicMat2D;
    private PhysicsMaterial2D smoothPhysicMat2D;

    public Transform[] rayCheckPoints;
    public Transform[] rayCheckPointsWall;
    public System.Action onLandAction;
    private const int groundLayerMask = (1 << 7)|(1<<13);

    //��·�ζ�
    public float WalkShakeStrength=1;
    public float WalkShakeFrequency= 1;

    //���ȼ��
    public bool OnFan { get; set; }
    public Vector3 OnFanDirection { get; set; }
    public float OnFanSpeed { get; set; }

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
        //��������
        rb.AddForce(-rb.velocity * airResistanceCoefficient);
        if(IsGround && GetHorizontalSpeed()!=0f)
        springRb.AddForce(WalkShakeStrength * Vector2.up * Mathf.Sin(Time.time * WalkShakeFrequency));
    }
    /// <summary>
    /// ��ص�˲�䴥��
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
    public void AddForce(Vector2 value)
    {
        if (!isUnderControl) return;
        if (value == Vector2.zero) return;
        rb.velocity += value;
    }
    public void LimitVelocity(float maxSpeed)
    {
        float currentSpeed = rb.velocity.magnitude;
        if (currentSpeed > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
    public void DisableGravity()
    {
        isUsingGravity = false;
    }
    public void EnableGravity()
    {
        isUsingGravity = true;
    }
    /// <summary>
    /// �ڿ����л��ɹ⻬������ʣ���ֹ��ǽ
    /// </summary>
    private void ChangePhysicMat()
    {
        if (isGround) _collider.sharedMaterial = normalPhysicMat2D;
        else _collider.sharedMaterial = smoothPhysicMat2D;
    }
    public void ChangePhysicMatSmooth(bool v)
    {
        if (!v) _collider.sharedMaterial = normalPhysicMat2D;
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
            if (Physics2D.Raycast(rayCheckPoints[i].position, Vector2.down, 0.1f, groundLayerMask))
            {
                if (!isGround)
                    OnLand();

                return true;
            }
        }

        return false;
    }
    public float CheckIsWall()
    {
        for (int i = 0; i < rayCheckPointsWall.Length; i++)
        {
            if (Physics2D.Raycast(rayCheckPointsWall[i].position, Vector2.left, 0.8f, groundLayerMask))
            {
                return -1;
            }
        }
        for (int i = 0; i < rayCheckPointsWall.Length; i++)
        {
            if (Physics2D.Raycast(rayCheckPointsWall[i].position, Vector2.right, 0.8f, groundLayerMask))
            {
                return 1;
            }
        }

        return 0;
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
