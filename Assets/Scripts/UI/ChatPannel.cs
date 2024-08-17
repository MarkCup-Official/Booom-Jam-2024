using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.UI;
public class ChatPannel : BasePannel
{
    public GameObject chatWindow;
    public Animator _animator;
    public TextPrinter printer;
    
    public void Show(List<string> list)
    {

        chatWindow.gameObject.SetActive(true);
        _animator.Play("Enter");
        if (printer.Set(list))
        {
            printer.StartPrint();
        }
    }
    public override void Init(PannelLayer layer)
    {
        _animator = GetComponent<Animator>();
        printer.Init();
    }
    public void Close()
    {
        TimeDelay.Instance.Delay(1,()=> chatWindow.gameObject.SetActive(false));
        _animator.Play("Exit");
    }

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
