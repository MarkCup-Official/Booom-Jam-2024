using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AndroidButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public string button;

    public bool down;

    public void OnPointerDown(PointerEventData eventData)
    {
        InputMgr.key[button] = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        InputMgr.key[button] = false;
        down = false;
    }

    private void Update()
    {
        if (down && InputMgr.keyDown[button])
        {
            InputMgr.keyDown[button] = false;
        }
        if (InputMgr.key[button] && !down)
        {
            down = true;
            InputMgr.keyDown[button] = true;
        }
    }
}
