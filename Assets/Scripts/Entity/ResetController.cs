using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResetController : MonoBehaviour,Iinteractive
{
    private SingleText text;
    public UnityEvent OnReset;

    private void Start()
    {
         text = (UIManager.Instance.GetUI(UIManager.TextUIName) as TextUI).Show("÷ÿ÷√∆˜", transform, 0.5f * Vector3.down);
        text.DisShow();
    }
    public void OnPlayerEnter(PlayerMgr mgr)
    {
       text.Show();
    }

    public void OnPlayerEnterInteractive()
    {
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
