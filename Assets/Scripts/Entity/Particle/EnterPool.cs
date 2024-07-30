using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPool : MonoBehaviour
{
    
    public float FadeTime;
    private void OnEnable()
    {
        Invoke(nameof(GoPool), FadeTime);
    }
    public void GoPool()
    {
        ObjectPool.Instance.PushObject(gameObject);
    }
}
