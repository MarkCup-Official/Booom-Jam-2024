using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class EventMgr
{

    private static Dictionary<int, Action> _listenersDic = new Dictionary<int, Action>();

    public static void AddListener<T>(Action action) where T : IEventMessage
    {
        int hashcode = typeof(T).GetHashCode();

        if (!_listenersDic.ContainsKey(hashcode))
        {
            _listenersDic.Add(hashcode, action);
        }
        else
        {
            _listenersDic[hashcode] += action;
        }
    
    }
    public static void RemoveListener<T>()
    {
        int hashCode = typeof(T).GetHashCode();
        if (!_listenersDic.ContainsKey(hashCode)) return;
        _listenersDic.Remove(hashCode);
    } 
    public static void RemoveListener<T>(Action action)
    {
       System.Type type = typeof(T);
       RemoveListener(type,action); 
    }
    public static void RemoveListener(System.Type type,Action action)
    {
        int hashCode = type.GetHashCode();
        if (!_listenersDic.ContainsKey(hashCode)) return;
        _listenersDic[hashCode] -= action;
    }
  
    public static void Trigger<T>() where T : IEventMessage
    {
        int hashcode = typeof(T).GetHashCode();
        if (!_listenersDic.ContainsKey(hashcode)) return;
        _listenersDic[hashcode]?.Invoke();
    }
    public static void CleanAll()
    {
        _listenersDic.Clear();
    }
}
