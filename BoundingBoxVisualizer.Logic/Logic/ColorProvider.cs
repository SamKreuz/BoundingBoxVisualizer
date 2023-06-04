using Autodesk.Revit.DB;
using System;

namespace BoundingBoxVisualizer.Logic.Logic
{
    public static class ColorProvider
    {
        public static ColorWithTransparency GetRandomColor(Random random)
        {

            uint red = (uint)random.Next(0, 255);
            uint green = (uint)random.Next(0, 255);
            uint blue = (uint)random.Next(0, 255);
            uint transparency = 0;

            return new ColorWithTransparency(red, green, blue, transparency);
        }
    }
}
