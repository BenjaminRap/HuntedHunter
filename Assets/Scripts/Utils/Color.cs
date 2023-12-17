using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Color
{
    public static Color32   SetAlpha(Color32 color, int alpha)
    {
        Color32 newColor;

        if (alpha < 0)
            newColor = new Color32(color.r, color.g, color.b, 0);
        else if (alpha > 255)
            newColor = new Color32(color.r, color.g, color.b, 255);
        else
            newColor = new Color32(color.r, color.g, color.b, (byte)(alpha));
        return (newColor);
    }
}
