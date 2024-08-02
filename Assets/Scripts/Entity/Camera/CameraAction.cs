using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    private Transform playerTransform;
    public float followSpeed;
    private Camera _camera;
    private MoveController controller;
    public float DefaultCameraSize = 5;
    public float MaxCameraSizeOffset = 1;
    public float CameraOffsetStrength = 1;
   
    public void Init(MoveController moveController)
    {
        controller = moveController;
        _camera = GetComponent<Camera>();
    }
    
    public void SetPlayer(Transform target)
    {
        playerTransform = target;
    }
    public void FollowPlayer()
    {
        
        Vector2 pos2D =Vector2.Lerp(transform.position, playerTransform.position, followSpeed *Time.deltaTime);
        transform.position = new Vector3(pos2D.x, pos2D.y, -10);
    }
    private void Update()
    {
       

        if (controller == null) return;

        float speed = controller.GetSpeed().magnitude;
        float s = _camera.orthographicSize;
        s = Mathf.Lerp(s, DefaultCameraSize + Mathf.Clamp(CameraOffsetStrength * speed,0, MaxCameraSizeOffset) , 1 * Time.deltaTime);
        _camera.orthographicSize = s;
    }
}
