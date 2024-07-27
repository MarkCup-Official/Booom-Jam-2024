using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    private Transform playerTransform;
    public float followSpeed;
    

    public void SetPlayer(Transform target)
    {
        playerTransform = target;
    }
    public void FollowPlayer()
    {
        
        Vector2 pos2D =Vector2.Lerp(transform.position, playerTransform.position, followSpeed *Time.deltaTime);
        transform.position = new Vector3(pos2D.x, pos2D.y, -10);
    }
}
