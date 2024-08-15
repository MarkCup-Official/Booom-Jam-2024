using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterReceiver : MonoBehaviour
{
    private SpriteRenderer _renderer;
    public bool isActive;
    public Sprite ActiveSprite;
    public Sprite DisactiveSprite;
    private void Start()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, 1 << 11);
        if (hit.collider == null)
        {
            Set(false);
        }
        else
        {
            Set(true);
        }
    }
    public void Set(bool IsActive)
    {
        isActive = IsActive;
        if (IsActive)
        {
            _renderer.sprite = ActiveSprite;
        }
        else
        {
            _renderer.sprite = DisactiveSprite;
        }
    }
}
