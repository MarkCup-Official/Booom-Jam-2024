using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ReceiverMgr : MonoBehaviour
{
    public List<WaterReceiver> receivers = new List<WaterReceiver>();
    public bool isOk;
    public UnityEvent OnEnterOK;
    public UnityEvent OnExitOK;
    private void Update()
    {
        if (isAllReceiverActive())
        {
            if (!isOk)
            {
                isOk = true;
                OnEnterOK?.Invoke();
            }

        }
        else
        {
            if (isOk)
            {
                isOk = false;
                OnExitOK?.Invoke();
            }
        }
    }
    public bool isAllReceiverActive()
    {

        for (int i = 0; i < receivers.Count; i++)
        {
            if (!receivers[i].isActive)
            {
                return false;
            }
        }
        return true;
    }
}
