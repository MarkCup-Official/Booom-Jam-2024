using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : MonoBehaviour
{
    private MoveController moveController;
    public CameraAction targetCamera;
    private void Awake()
    {
        moveController = GetComponent<MoveController>();
        targetCamera.SetPlayer(transform);
    }
    private void Update()
    {
         moveController.JumpLogic(InputMgr.GetSpaceDown());
        
    }
    private void LateUpdate()
    {
        targetCamera.FollowPlayer();
    }
    private void FixedUpdate()
    {
        moveController.HorizontalMove(InputMgr.GetHorizontal());
       
    }
}
