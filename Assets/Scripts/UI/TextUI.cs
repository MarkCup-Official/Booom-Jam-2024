using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.UI;
public class TextUI : BasePannel
{
    public GameObject singleTextPrefab;
  
    public void Show(string TextContent, Transform target, Vector3 offset, float FadeTime)
    {

        GameObject go = ObjectPool.Instance.GetObject(singleTextPrefab);
        go.transform.SetParent(transform);
        if(go.TryGetComponent(out SingleText stext))
        {
            stext.Init(TextContent, target, offset, FadeTime);
        }
    }
    public SingleText Show(string TextContent, Transform target, Vector3 offset)
    {

        GameObject go = ObjectPool.Instance.GetObject(singleTextPrefab);
        go.transform.SetParent(transform);
        if (go.TryGetComponent(out SingleText stext))
        {
            stext.Init(TextContent, target, offset);
        }
        return stext;
    }
    public override void OnClose()
    {
        gameObject.SetActive(false);
    }

    public override void OnShowUp()
    {
        gameObject.SetActive(true);
    }

    public override void OnUpdate()
    {
       
    }
}
