using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BatteryBase : MonoBehaviour,Iinteractive
{
    private SingleText text;
    public Box battery;
    private PlayerMgr playerMgr;
    public UnityEvent OnBatteryEquipped;
    public UnityEvent OnBatteryTaken;
    public UnityEvent OnBatteryEquippedUpdate;
    public bool isBatteryAbleToMove = true;
    
    private void Start()
    {
        text = (UIManager.Instance.GetUI(UIManager.TextUIName) as TextUI).Show("µç³Ø»ù×ù", transform, 0.5f * Vector3.down);
        text.DisShow();
    }

    private void Update()
    {
        if (battery != null)
        {
            OnBatteryEquippedUpdate?.Invoke();
        }
    }
    public void OnPlayerEnter(PlayerMgr mgr)
    {
        text.Show();
        playerMgr = mgr;
    }

    public void OnPlayerEnterInteractive()
    {
        if (battery != null)
        {
            // battery.UnLock();
           
            if (!isBatteryAbleToMove)
            {
                battery.transform.SetParent(null);
                battery.UnLock();
            }
            SoundManager.Instance.PlaySound("takeBattery", 0.5f);
            playerMgr.EquipBattery(battery);
            battery.gameObject.SetActive(false);
            battery = null;
            OnBatteryTaken?.Invoke();
        }
        else
        {
            // battery.Lock();
           
            battery = playerMgr.RemoveBattery(); 
            
            if (battery == null) return;
            if (!isBatteryAbleToMove)
            { battery.Lock();
                battery.transform.SetParent(transform);
            }
            SoundManager.Instance.PlaySound("putBattery",0.5f);
            battery.gameObject.SetActive(true);
            battery.transform.position = transform.position; 
            OnBatteryEquipped?.Invoke();
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
