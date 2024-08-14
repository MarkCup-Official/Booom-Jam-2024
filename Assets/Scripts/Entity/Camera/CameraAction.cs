using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    private Transform playerTransform;
    public float followSpeed;
    private Camera _camera;
    private MoveController controller;
    private PlayerMgr mgr;
    public float DefaultCameraSize = 5;
    public float MaxCameraSizeOffset = 1;
    public float CameraOffsetStrength = 1;
    public float MaxCameraRot = 5;
    public float RotSpeed = 3;
    public float RotStrength = 1;
    public AnimationCurve smoothCurve;
    [Range(0,1f)]public float lerpTarget;
    
    public Transform target;
    private Vector3 targetPos;
    //private Vector2 targetPosition;
    public float addSize;
    public float LerpMultiplier = 1;
    public void Init(PlayerMgr mgr)
    {
        this.mgr = mgr;
        controller = mgr.moveController;
        _camera = GetComponent<Camera>();
    }
    
    public void SetPlayer(Transform target)
    {
        playerTransform = target;
        targetPos = target.position;
    }
    public void FollowPlayer()
    {


        targetPos = Vector3.Lerp(targetPos, playerTransform.position + lerpTarget * (target.transform.position - playerTransform.position), LerpMultiplier * Time.deltaTime); 
       // Debug.DrawRay(targetPos, Vector2.down, Color.green, 1);
        
        float followSpeedMultiplier = Mathf.Pow(transform.position.y - targetPos.y,2);
        followSpeedMultiplier = Mathf.Clamp(followSpeedMultiplier, 1, 100);
        float x = Mathf.Lerp(transform.position.x, targetPos.x ,1.2f * Time.deltaTime);
        Vector2 pos2D =Vector2.Lerp(transform.position, targetPos, LerpMultiplier * followSpeed * followSpeedMultiplier * Time.deltaTime);
        transform.position = new Vector3(x, pos2D.y, -10);
    }
    private void Update()
    {
       

        if(LerpMultiplier < 1)
        LerpMultiplier +=  Time.deltaTime;
        if (controller == null) return;

       
        float fade = Mathf.Clamp(1 - addSize / 20f, 0, 1f);
        float speed = controller.GetSpeed().magnitude;
        float FOV = _camera.fieldOfView;
        FOV = Mathf.Lerp(FOV, addSize + DefaultCameraSize + fade * Mathf.Clamp(CameraOffsetStrength * speed,0, MaxCameraSizeOffset) , LerpMultiplier * Time.deltaTime);
        _camera.fieldOfView = FOV;


        float yrot = transform.localEulerAngles.y;
        if (yrot > 180)
        {
            yrot -= 360;
        }


        yrot = Mathf.Lerp(yrot, controller.GetSpeed().x * RotStrength * fade, RotSpeed * Time.deltaTime);
        yrot = Mathf.Clamp(yrot, -MaxCameraRot, MaxCameraRot );
        transform.rotation = Quaternion.Euler(0, yrot, 0);

    }
}
