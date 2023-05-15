using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BoundingBoxVisualizer.Logic.Logic;

namespace BoundingBoxVisualizer.Logic.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class CommandRemoveAll : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            bool result = new ServiceUtility().RemoveAllServers(commandData.Application.ActiveUIDocument.Document);

            if(!result)
            {
                return Result.Failed;
            }
            return Result.Succeeded;
        }
    }
}
