using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputMgr
{
    private static bool isActive = true;

    public static SimpleTouchController touchController;

    public static Dictionary<string,bool> key = new Dictionary<string, bool>() { { "f", false }, { "a",false} };
    public static Dictionary<string, bool> keyDown = new Dictionary<string, bool>() { { "f", false }, { "a", false } };

    public static float GetHorizontal()
    {
        if (!isActive) return 0;
        if (touchController != null)
        {
            if (touchController.GetTouchPosition.x != 0)
            {
                return Mathf.Clamp( touchController.GetTouchPosition.x,-1,1);
            }
        }
        return Input.GetAxis("Horizontal");
    }
    public static float GetVertical()
    {
        if (!isActive) return 0;
        if (touchController != null)
        {
            if (touchController.GetTouchPosition.y != 0)
            {
                return Mathf.Clamp(touchController.GetTouchPosition.y, -1, 1);
            }
        }
        return Input.GetAxis("Vertical");
    }
    public static bool GetSpace()
    {
        if (!isActive) return false;
        if (key["a"])
        {
            return true;
        }
        return Input.GetKey(KeyCode.Space);
    }
    public static bool GetSpaceDown()
    {
        if (!isActive) return false;
        if (keyDown["a"])
        {
            return true;
        }
        return Input.GetKeyDown(KeyCode.Space);
    }
    public static bool IsCatchButtonDown()
    {
        if (!isActive) return false;
        if (keyDown["f"])
        {
            return true;
        }
        return Input.GetKeyDown(KeyCode.F);
    }
    
    public static void Enable()
    {
        isActive = true;
    }
    public static void Disable()
    {
        isActive = false;
    }
  
}
