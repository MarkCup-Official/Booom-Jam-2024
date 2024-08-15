using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ClothChanger : MonoBehaviour, Iinteractive
{
    private SingleText text;
    private PlayerMgr mgr;
    private bool isEquiped;
    public UnityEvent onEquipCloth;
    public UnityEvent onQuitCloth;
    public int StartState = 1;
    public int EquipState = 0;
    private void Start()
    {
        text =  (UIManager.Instance.GetUI(UIManager.TextUIName) as TextUI).Show("»»ÒÂÌ¨", transform, Vector3.down * 0.5f);
        text.DisShow();
    }
    public void OnPlayerEnter(PlayerMgr mgr)
    {
        text.Show();
        this.mgr = mgr;
    }

    public void OnPlayerEnterInteractive()
    {
        isEquiped = !isEquiped;
       if (isEquiped)
        {
            mgr.fsm.SwitchState(EquipState);
            onEquipCloth?.Invoke();
        }
       else
        {
            mgr.fsm.SwitchState(StartState);
            onQuitCloth?.Invoke();

        }
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
