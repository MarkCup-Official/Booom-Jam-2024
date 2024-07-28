using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerView : MonoBehaviour
{
    public GameObject viewObject;
    private float FlipTimer;

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
            viewObject.transform.DOScaleX(1, FlipTime);
        }
         if (dir < 0f)
        {
            viewObject.transform.DOScaleX(-1, FlipTime);
        }
   }
}
