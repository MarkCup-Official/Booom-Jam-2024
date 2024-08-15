using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ConsoleButton : MonoBehaviour,Iinteractive
{
    private SpriteRenderer _renderer;
    public Sprite ActiveSprite;
    public Sprite DisactiveSprite;
    public bool isActive;
    private SingleText _text;
    public UnityEvent OnAble;
    public UnityEvent OnDisable;
    public UnityEvent OnSwitch;
    private void Start()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _text = (UIManager.Instance.GetUI(UIManager.TextUIName) as TextUI).Show("¿ØÖÆÌ¨", transform, Vector3.down * 0.5f);
        _text.DisShow();
        Set(isActive);
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
    public void OnPlayerEnter(PlayerMgr mgr)
    {
        _text.Show();
    }

    public void OnPlayerEnterInteractive()
    {
        isActive = !isActive;
        OnSwitch?.Invoke();
        Set(isActive);
       if (isActive)
        {
            OnAble?.Invoke();
        }
       else
        {
            OnDisable?.Invoke();
        }
    }

    public void OnPlayerExit()
    {
        _text.DisShow();
    }

    public void OnPlayerExitInteractive()
    {
       
    }

    public void OnPlayerInteractive()
    {
        
    }
}
