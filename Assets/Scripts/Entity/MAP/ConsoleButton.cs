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
        Set(isActive,false);
    }
    public void Set(bool IsActive,bool playSound = true)
    {
        isActive = IsActive;
        if (IsActive)
        {
            if(playSound)
            SoundManager.Instance.PlaySound("takeBattery",0.3f);
            _renderer.sprite = ActiveSprite;
        }
        else
        {
            if (playSound)
                SoundManager.Instance.PlaySound("putBattery", 0.3f);
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
