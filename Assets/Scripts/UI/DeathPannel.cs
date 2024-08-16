using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.UI;
public class DeathPannel : BasePannel
{
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
