using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MoveablePlatform : MonoBehaviour
{
    public float MoveInterval = 1f;
    public Transform[] leftCheck;
    public Transform[] rightCheck;
    public Transform[] upCheck;
    public Transform[] downCheck;
    private Vector2 startPos;
   
    float MoveTimer;
    private void Start()
    {
        MoveTimer = Time.time;
        startPos = transform.position;
    }
    public void ResetPos()
    {
        transform.position = startPos;
    }
    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        DrawRay(leftCheck, Vector2.left);
        DrawRay(rightCheck, Vector2.right);
        DrawRay(upCheck, Vector2.up);
        DrawRay(downCheck, Vector2.down);
    }
    private void DrawRay(Transform[] transforms,Vector2 direction)
    {

        if (transforms == null) return;
        for (int i = 0; i < transforms.Length; i++)
        {
            Debug.DrawRay(transforms[i].position, direction * 0.5f,Color.red);
        }
    }

    public void MoveLeft()
    {
        MoveHorizontal(-1);
    }
    public void MoveRight()
    {
        MoveHorizontal(1);
    }
    public void MoveHorizontal(int targetDir)
    {
        if (MoveTimer + MoveInterval < Time.time)
        {
            MoveTimer = Time.time;
            if (IsHorizontalMoveable(targetDir))
            {
                const float moveTime = 0.4f;
                SoundManager.Instance.PlaySound("wood2",0.3f);
                transform.DOMoveX(transform.position.x + targetDir, moveTime);
            }
        }
    }
    public void MoveVertical(int targetDir)
    {
        if (MoveTimer + MoveInterval < Time.time)
        {
            MoveTimer = Time.time;
            if (IsVerticalMoveable(targetDir))
            {
                const float moveTime = 0.4f;
                SoundManager.Instance.PlaySound("wood2", 0.3f);
                transform.DOMoveY(transform.position.y + targetDir, moveTime);
            }
        }
    }

    public bool IsHorizontalMoveable(int dir)
    {

        if (dir == -1)
        {
            if (leftCheck == null) return false;
            for (int i = 0; i < leftCheck.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(leftCheck[i].position, Vector2.right * dir, 0.5f, 1 << 7);
                if (hit)
                    return false;
            }
        }
        else if(dir == 1)
        {
            if (rightCheck == null) return false;
            for (int i = 0; i < rightCheck.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(rightCheck[i].position, Vector2.right * dir, 0.5f, 1 << 7);
                if (hit)
                    return false;
            }
        }
        return true;

    }

    public bool IsVerticalMoveable(int dir)
    {

        if (dir == -1)
        {
            if (downCheck == null) return false;
            for (int i = 0; i < downCheck.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(downCheck[i].position, Vector2.up * dir, 0.5f, 1 << 7);
                if (hit)
                    return false;
            }
        }
        else if (dir == 1)
        {
            if (upCheck == null) return false;
            for (int i = 0; i < upCheck.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(upCheck[i].position, Vector2.up * dir, 0.5f, 1 << 7);
                if (hit)
                    return false;
            }
        }
        return true;

    }

}
