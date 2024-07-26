using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonoManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance != null)
            return instance;
            //MyDebug.LogError("获取不到单例" + " 单例ID = " + typeof(T).Name);
            return null;
        }
    }
    protected virtual void Awake()
    {
        instance = this as T;
    }

}
