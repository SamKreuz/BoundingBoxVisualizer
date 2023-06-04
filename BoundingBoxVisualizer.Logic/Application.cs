using Autodesk.Revit.UI;
using BoundingBoxVisualizer.Logic.Logic;
using BoundingBoxVisualizer.Logic.Logic.Logger;
using System;

namespace BoundingBoxVisualizer.Logic
{
    public class Application : IExternalApplication
    {
        public static Application Instance { get; private set; }
        public static ILogger Logger { get; private set; }
        public Random Random { get; } = new Random();

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            Instance = this;
            Logger = new ConsoleLogger();

            return Result.Succeeded;
        }
    }
}
