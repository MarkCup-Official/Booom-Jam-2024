using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignBoard : MonoBehaviour, Iinteractive
{
    [Multiline]
    public string content;
    private SingleText text;
    private void Start()
    {
        text = (UIManager.Instance.GetUI(UIManager.TextUIName) as TextUI).Show(content, transform, Vector3.up * 0.5f);
        text.DisShow();
    }
    public void OnPlayerEnter(PlayerMgr mgr)
    {
        text.Show();
    }

    public void OnPlayerEnterInteractive()
    {
       
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
