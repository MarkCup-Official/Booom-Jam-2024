using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugTool
{

    public static void DrawBox(Vector2 position)
    {
        DrawBox(position, Color.white);
    }
    public static void DrawBox(Vector2 position, Color color)
    {
        DrawBox(position, new Vector2(1, 1), color);
    }
    public static void DrawBox(Vector2 position,Vector2 size,Color color)
    {
        int x = (int)position.x;
        int y = (int)position.y;

        Vector2 topLeft = new Vector2(x, y + size.y);
        Vector2 topRight = new Vector2(x + size.x, y + size.y);
        Vector2 bLeft = new Vector2(x, y);
        Vector2 bRight = new Vector2(x + size.x, y);
        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bRight, color);
        Debug.DrawLine(bRight, bLeft, color);
        Debug.DrawLine(bLeft, topLeft, color);
    }
}