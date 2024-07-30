using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerView : MonoBehaviour
{
    public GameObject viewObject;
    private float FlipTimer;
    private PlayerMgr mgr;
    public GameObject landParticlePrefab;
    public void Init(PlayerMgr mgr)
    {
        this.mgr = mgr;
        mgr.moveController.onLandAction += () =>
        {
            GameObject go = ObjectPool.Instance.GetObject(landParticlePrefab);
            go.transform.position = viewObject.transform.position +Vector3.up * 0.1f;
        };
    }
    private void Start()
    {
        FlipTimer = Time.time;
    }
    public void Flip(float dir)
    { 
        
        const float FlipTime = 0.2f;
        
        if (FlipTimer + FlipTime > Time.time)
        {
            return;
        }
        FlipTimer = Time.time;
        if (dir > 0f)
        {
            //viewObject.transform.localScale = new Vector3(1, 1, 1);
            viewObject.transform.DOScaleX(1, FlipTime);
        }
         if (dir < 0f)
        {
            //viewObject.transform.localScale = new Vector3(-1, 1, 1);
            viewObject.transform.DOScaleX(-1, FlipTime);
        }
   }
    

}
