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
    public string doorName = "自动门";
    private void Start()
    {


        animator = GetComponent<Animator>();
       
        text = (UIManager.Instance.GetUI(UIManager.TextUIName) as TextUI).Show($"{doorName}[已锁住]", transform, Vector3.down);
        text.DisShow();

        if (isLocked)
            OnDoorClose();
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
        SoundManager.Instance.PlaySound("door_1", 0.4f);
    }
    private void OnDoorClose()
    {
        if (isOpen &&!isLocked)
        {
            SoundManager.Instance.PlaySound("door_1", 0.4f);
        }
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
