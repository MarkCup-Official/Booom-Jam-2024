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
    public Rigidbody2D springRb;
    public GameObject blueLight;
    public GameObject rotatrSprite;

    public Sprite[] characterSprites;

    public void Init(PlayerMgr mgr)
    {
        this.mgr = mgr;
        mgr.moveController.onLandAction += () =>
        {
            if (mgr.moveController.GetVerticalSpeed() < -5f)
            landParticle.Emit(20);
        };
      
    }
    private void Start()
    {
        blueLight.SetActive(false);
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
    public void FlipNow(float dir)
    {
        if (dir > 0.1f)
        {
            targetDir = 1; 
            viewObject.transform.localScale = new Vector3(targetDir * 1 , 1, 1);
        }
        else if (dir < -0.1f)
        {
            targetDir = -1;
            viewObject.transform.localScale = new Vector3(targetDir * 1 , 1, 1);
        }
    }

    public void Ratate(float dir)
    {
        rotatrSprite.transform.localEulerAngles=new Vector3(0, 0, dir);
    }

    public void PlayAnimation(string animName)
    {
        viewAnimator.Play(animName);
    }
    public void SetSpringRbIsKinematic(bool isKinematic)
    {
        springRb.isKinematic = isKinematic;
    }
    public void SetSpringCenterBackToOriginPos()
    {
        springRb.isKinematic = true;
        springRb.transform.localPosition = new Vector3(0, 0.45f, 0);
        springRb.isKinematic = false;
    }
    

}
