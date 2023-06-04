using Autodesk.Revit.DB;
using System;

namespace BoundingBoxVisualizer.Logic.Logic
{
    public static class ColorProvider
    {
        public static ColorWithTransparency GetRandomColor(Random random)
        {

            uint red = (uint)random.Next(0, 256);
            uint green = (uint)random.Next(0, 256);
            uint blue = (uint)random.Next(0, 256);
            uint transparency = 0;

            return new ColorWithTransparency(red, green, blue, transparency);
        }
    }
}
