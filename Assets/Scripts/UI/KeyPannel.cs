using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.UI;
public class KeyPannel : BasePannel
{
    private Transform target;
    private Vector3 offset;
    private bool isActive;
    public GameObject img;
    private Camera playerCamera;

    public override void Init(PannelLayer layer)
    {
        base.Init(layer);
        playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMgr>().targetCamera.GetComponent<Camera>();
    }
    public void Show(Transform target,Vector3 offset)
    {
        img.SetActive(true);
        isActive = true;
        this.target = target;
        this.offset = offset;
    }
    public void DisShow()
    {
        isActive = false;
        img.SetActive(false);
    }

    public override void OnClose()
    {
         gameObject.SetActive(false);
    }

    public override void OnShowUp()
    {
        gameObject.SetActive(true);
    }
    private void Update()
    {
        OnUpdate();
    }
    public override void OnUpdate()
    {
        if(isActive)
        img.transform.position = playerCamera.WorldToScreenPoint(target.transform.position + offset);
    }
}
