using Autodesk.Revit.UI;
using BoundingBoxVisualizer.Logic.Logic;

namespace BoundingBoxVisualizer.Logic
{
    public class Application : IExternalApplication
    {
        public static ILogger Logger { get; private set; }

        public Result OnShutdown(UIControlledApplication application)
        {
            throw new System.NotImplementedException();
        }

        public Result OnStartup(UIControlledApplication application)
        {
            //Logger = new Logger();

            //throw new System.NotImplementedException();

            return Result.Succeeded;
        }
    }
}
