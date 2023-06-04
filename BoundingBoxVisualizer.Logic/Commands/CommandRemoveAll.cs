using System;
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
            bool successul = false;

            try
            {
                successul = new ServiceUtility().RemoveAllServers(commandData.Application.ActiveUIDocument.Document);

                commandData.Application.ActiveUIDocument.UpdateAllOpenViews();
            }
            catch(Exception ex)
            {
                Application.Logger.Error("Failed to remoce all existing Direct Context servers.");
            }

            if (!successul)
            {
                return Result.Failed;
            }
            return Result.Succeeded;
        }
    }
}
