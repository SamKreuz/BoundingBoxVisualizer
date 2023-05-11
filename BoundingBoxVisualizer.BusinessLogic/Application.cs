using Autodesk.Revit.UI;

namespace BoundingBoxVisualizer.BusinessLogic
{
    public class Application : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            throw new System.NotImplementedException();
        }

        public Result OnStartup(UIControlledApplication application)
        {
            throw new System.NotImplementedException();
        }
    }
}
