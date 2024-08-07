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

    public void Init(PlayerMgr mgr)
    {
        this.mgr = mgr;
        controller = mgr.moveController;
        _camera = GetComponent<Camera>();
    }
    
    public void SetPlayer(Transform target)
    {
        playerTransform = target;
    }
    public void FollowPlayer()
    {
        float Yoffset = 0;
        int direction = mgr.playerView.targetDir;
        if (InputMgr.GetHorizontal() == 0f && InputMgr.GetVertical() == 0f&&mgr.moveController.IsGround)
        if (!Physics2D.Raycast(playerTransform.position + Vector3.down + Vector3.right * direction, Vector2.down, 3,1<<7))
        {
                Yoffset = - 2;
        }
        float followSpeedMultiplier = 1;
        followSpeedMultiplier = Mathf.Abs(transform.position.y - playerTransform.transform.position.y);
        followSpeedMultiplier = Mathf.Clamp(followSpeedMultiplier, 1, 100);
        float x = Mathf.Lerp(transform.position.x, playerTransform.transform.position.x , 1.2f * Time.deltaTime);
        Vector2 pos2D =Vector2.Lerp(transform.position, playerTransform.position + new Vector3(0, Yoffset,0), followSpeed* followSpeedMultiplier * Time.deltaTime);
        transform.position = new Vector3(x, pos2D.y, -10);
    }
    private void Update()
    {
       

        if (controller == null) return;

        float speed = controller.GetSpeed().magnitude;
        float s = _camera.fieldOfView;
        s = Mathf.Lerp(s, DefaultCameraSize + Mathf.Clamp(CameraOffsetStrength * speed,0, MaxCameraSizeOffset) , 1 * Time.deltaTime);
        _camera.fieldOfView = s;
        float yrot = transform.localEulerAngles.y;
        if (yrot > 180)
        {
            yrot -= 360;
        }

        yrot = Mathf.Lerp(yrot, controller.GetSpeed().x * RotStrength, RotSpeed * Time.deltaTime);
        yrot = Mathf.Clamp(yrot, -MaxCameraRot, MaxCameraRot);
        transform.rotation = Quaternion.Euler(0, yrot, 0);
        
    }
}
