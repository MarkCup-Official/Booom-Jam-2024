using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerView : MonoBehaviour
{
    public GameObject viewObject;
    public SpriteRenderer spriteRenderer;
    private float FlipTimer;
    private PlayerMgr mgr;
    public GameObject landParticlePrefab;
    public Transform SpringCenter;
    private int targetDir = 1;
    public float OriginHeight = 0.8f;
    public float SpringAddHeight = 0.4f;

    public Sprite[] characterSprites;

    public void Init(PlayerMgr mgr)
    {
        this.mgr = mgr;
        mgr.moveController.onLandAction += () =>
        {
            GameObject go = ObjectPool.Instance.GetObject(landParticlePrefab);
            go.transform.position = viewObject.transform.position + Vector3.up * 0.1f;
        };
      
    }
    private void Start()
    {
        FlipTimer = Time.time;
    }
    private void Update()
    {
        float deltaY = SpringCenter.position.y - transform.position.y;
        float height = OriginHeight + SpringAddHeight * deltaY;
        height = Mathf.Clamp(height, 0.5f, 2f);
        viewObject.transform.localScale = new Vector3(targetDir * 1 / height, height, 1);
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

    public void ChangeSprite(int spriteIndex)
    {
        if (spriteIndex < 0 || spriteIndex >= characterSprites.Length) return;
        spriteRenderer.sprite = characterSprites[spriteIndex];
    }
    

}
