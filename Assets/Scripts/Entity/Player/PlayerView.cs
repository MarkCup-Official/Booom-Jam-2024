using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerView : MonoBehaviour
{
    public GameObject viewObject;

    public Animator viewAnimator;
    private float FlipTimer;
    private PlayerMgr mgr;
    public ParticleSystem landParticle;
    private float EmissionTimer;
    public Transform SpringCenter;
    public int targetDir = 1;
    public float OriginHeight = 0.8f;
    public float SpringAddHeight = 0.4f;

    public Sprite[] characterSprites;

    public void Init(PlayerMgr mgr)
    {
        this.mgr = mgr;
        mgr.moveController.onLandAction += () =>
        {
            landParticle.Emit(20);
        };
      
    }
    private void Start()
    {
        FlipTimer = Time.time;
        EmissionTimer = Time.time;
    }
    private void Update()
    {
        float deltaY = SpringCenter.position.y - transform.position.y;
        float height = OriginHeight + SpringAddHeight * deltaY;
        height = Mathf.Clamp(height, 0.5f, 2f);
        viewObject.transform.localScale = new Vector3(targetDir * 1 / height, height, 1);

        if (mgr.moveController.IsGround && mgr.moveController.GetHorizontalSpeed() != 0)
        {
            if (EmissionTimer + 0.2f < Time.time)
            {
                EmissionTimer = Time.time;
                landParticle.Emit(2);
            }
        }
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
            targetDir = 1;
        }
        if (dir < 0f)
        {
            targetDir = -1;
        }
    }
    public void PlayAnimation(string animName)
    {
        viewAnimator.Play(animName);
    }
    

}
