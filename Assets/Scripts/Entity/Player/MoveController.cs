using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    private float moveSpeed;
    public float JumpForce = 5;
    private float jumpTimer;
    private float jumpCD = 0.1f;
    private Rigidbody2D rb;
    private bool isGround;
    
    public Transform[] rayCheckPoints;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
      
    }
    private void Start()
    {
        //Test
        MoveSpeed = 5;  
        jumpTimer = Time.time;
    }
    private void Update()
    {
        isGround = CheckIsGround();
        JumpLogic();
        BetterGravity();
    }
    private void FixedUpdate()
    {
        HorizontalMove(InputMgr.GetHorizontal());
        
    }
    public void HorizontalMove(float value)
    {
        if (value == 0f) return;
            rb.velocity = new Vector2(value * moveSpeed, rb.velocity.y);
    }
    /// <summary>
    /// 检测角色是否在地面上
    /// </summary>
    /// <returns></returns>
    private bool CheckIsGround()
    {
        for (int i = 0; i < rayCheckPoints.Length; i++)
        {
            if (Physics2D.Raycast(rayCheckPoints[i].position, Vector2.down, 0.1f,~(1<<3)))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// handle jump logic
    /// </summary>
   
    public void JumpLogic()
    {
       
        if (InputMgr.GetSpace() && isGround && jumpTimer + jumpCD <= Time.time)
        {
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
