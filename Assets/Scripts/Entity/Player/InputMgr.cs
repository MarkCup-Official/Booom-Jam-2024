using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputMgr
{
    private static bool isActive = true;
    public static float GetHorizontal()
    {
        if (!isActive) return 0;
        return Input.GetAxis("Horizontal");
    }
    public static float GetVertical()
    {
        if (!isActive) return 0;
        return Input.GetAxis("Vertical");
    }
    public static bool GetSpace()
    {
        if (!isActive) return false;
        return Input.GetKey(KeyCode.Space);
    }
    public static bool GetSpaceDown()
    {
        if (!isActive) return false;
        return Input.GetKeyDown(KeyCode.Space);
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
