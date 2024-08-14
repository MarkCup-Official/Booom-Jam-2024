using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class PlatTrigger : MonoBehaviour, Iinteractive
{
    public SpriteRenderer[] renders;
    private PlayerMgr playerMgr;
    public GameObject trigger;
    public int state = 0;//0代表位于中间，1代表位于右，-1左
    
    public UnityEvent OnBarisOnLeft;
    public UnityEvent OnBarisOnRight;
    public int ControlPos = 1;
    public bool isable = true;

    public void Enable()
    {
        isable = true;
    }
    public void Disable()
    {
        isable = false;
    }
    
    private void Update()
    {
        if (!isable) return;

        trigger.transform.localRotation = Quaternion.Euler(0, 0, -state *40);
        if (state == 1)
        {
            OnBarisOnRight?.Invoke();
        }
        else if (state == -1)
        {
            OnBarisOnLeft?.Invoke();
        }

    }
    public void OnPlayerEnter(PlayerMgr mgr)
    {
        playerMgr = mgr;
        foreach (var item in renders)
        {
            item.material.SetVector("_Emission", new Vector4(2,2,2,2)); 
        }
    }

    public void OnPlayerEnterInteractive()
    {
        playerMgr.moveController.SetIsUnderControl(false);
        
        const float FadeTime = 0.4f;
       // float distance = playerMgr.transform.position.x - transform.position.x;
        if (ControlPos > 0)
        {
            playerMgr.transform.DOMove(transform.position + Vector3.up + Vector3.right, FadeTime);
            //playerMgr.transform.position = transform.position + Vector3.up + Vector3.right;
        }
        else
            playerMgr.transform.DOMove(transform.position + Vector3.up - Vector3.right, FadeTime);
        //playerMgr.transform.position = transform.position + Vector3.up - Vector3.right;

        TimeDelay.Instance.Delay(FadeTime, () => playerMgr.playerView.SetSpringCenterBackToOriginPos());
        


    }

    public void OnPlayerExit()
    {
        foreach (var item in renders)
        {
            item.material.SetColor("_Emission", Color.black);
        }
        OnPlayerExitInteractive();
    }
    public void OnPlayerExitInteractive()
    {
        playerMgr.moveController.SetIsUnderControl(true);
        state = 0;
    }

    public void OnPlayerInteractive()
    {
        if (playerMgr == null) return;

        float distance = InputMgr.GetHorizontal();
        if (distance > 0f) state = 1;
        else if (distance < 0f) state = -1;
        else state = 0;
    }
}
public interface Iinteractive
{
    public void OnPlayerEnter(PlayerMgr mgr);
    public void OnPlayerExit();

    public void OnPlayerEnterInteractive();

    public void OnPlayerExitInteractive();

    public void OnPlayerInteractive();
}