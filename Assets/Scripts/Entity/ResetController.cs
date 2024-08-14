using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResetController : MonoBehaviour,Iinteractive
{
    private SingleText text;
    public UnityEvent OnReset;
    public GameObject[] trashes;
    public bool isable = true;
    public void Enable()
    {
        isable = true;
    }
    public void Disable()
    {
        isable = false;
    }
    private void Start()
    {
         text = (UIManager.Instance.GetUI(UIManager.TextUIName) as TextUI).Show("重置器", transform, 0.5f * Vector3.down);

        text.DisShow();
    }
    public void OnPlayerEnter(PlayerMgr mgr)
    {
       text.Show();
    }

    public void ReSetTrashes()
    {
       

        if (trashes == null) return;

        for (int i = 0; i < trashes.Length; i++)
        {
            trashes[i].gameObject.SetActive(true);
        }
    }
    public void OnPlayerEnterInteractive()
    {
        text.ChangeText("重置器");
        if (!isable)
        {
            text.ChangeText("重置器（缺少电源）");
            return;
        }

        OnReset?.Invoke();
    }

    public void OnPlayerExit()
    {
        text.DisShow();
    }

    public void OnPlayerExitInteractive()
    {
       
    }

    public void OnPlayerInteractive()
    {
      
    }

    
}
