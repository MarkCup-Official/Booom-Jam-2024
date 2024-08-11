using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopPlatform : MonoBehaviour
{
    public float LeftLimit;
    public float RightLimit;
    private Vector2 originPos;

    public float moveSpeed;
    private int direction = 1;
    // Update is called once per frame
    private void Start()
    {
        originPos = transform.position;
    }
    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        

        Vector2 leftPos = (Vector2)transform.position - new Vector2(LeftLimit,0);
        Vector2 rightPos = (Vector2)transform.position + new Vector2(RightLimit,0);
        Debug.DrawLine(leftPos, rightPos,Color.red);
    }
    void Update()
    {

        if (transform.position.x > originPos.x + RightLimit)
            direction = -1;
        else
        if (transform.position.x < originPos.x - LeftLimit)
            direction = 1;
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed * direction);
    }
    
}
