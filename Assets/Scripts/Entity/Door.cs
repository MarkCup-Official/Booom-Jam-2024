using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Rendering.Universal;
public class Door : MonoBehaviour, Iinteractive
{
    public bool isLocked;
    private bool isOpen = false;
    public BoxCollider2D _collider;
    private Animator animator;
    public Light2D signLight;
    public Color whenDoorIsLocked;
    public Color whenDoorIsUnLocked;
    private SingleText text;

    private void Start()
    {


        animator = GetComponent<Animator>();
       
        text = (UIManager.Instance.GetUI(UIManager.TextUIName) as TextUI).Show("自动门[已锁住]", transform, Vector3.down);
        text.DisShow();
    }
    public void Lock()
    {
        isLocked = true;
    }
    public void UnLock()
    {
        isLocked = false;
    }
    private void Update()
    {

        if (isLocked)
        {
            signLight.color = whenDoorIsLocked;

        }
        else
        {
            signLight.color = whenDoorIsUnLocked;
        }

    }
    private void OnDoorOpen()
    {
        isOpen = true;
        animator.Play("DoorOpen");
        _collider.enabled = false;
    }
    private void OnDoorClose()
    {
        isOpen = false;
        animator.Play("DoorClose");
        _collider.enabled = true;
    }

    public void OnPlayerEnter(PlayerMgr mgr)
    {
        if (isLocked)
        {
            text.Show();
        }
        else
        {
            OnDoorOpen();
        }

    }

    public void OnPlayerExit()
    {
        text.DisShow();
        OnDoorClose();
    }

    public void OnPlayerEnterInteractive()
    {

    }

    public void OnPlayerExitInteractive()
    {

    }

    public void OnPlayerInteractive()
    {

    }
}
