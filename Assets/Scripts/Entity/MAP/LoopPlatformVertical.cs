using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopPlatformVertical : MonoBehaviour
{
    public float downLimit;
    public float upLimit;
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


        Vector2 leftPos = (Vector2)transform.position - new Vector2(0,downLimit);
        Vector2 rightPos = (Vector2)transform.position + new Vector2(0,upLimit);
        Debug.DrawLine(leftPos, rightPos, Color.red);
    }
    void Update()
    {

        if (transform.position.y > originPos.y + upLimit)
            direction = -1;
        else
        if (transform.position.y < originPos.y - downLimit)
            direction = 1;
        transform.Translate(Vector3.up * Time.deltaTime * moveSpeed * direction,Space.World);
    }

}
