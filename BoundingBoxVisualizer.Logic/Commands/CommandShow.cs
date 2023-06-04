using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using BoundingBoxVisualizer.Logic.Logic;
using System;

namespace BoundingBoxVisualizer.Logic.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class CommandShow : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;

            Element element = PickElement(uiDocument);

            if (element == null)
            {
                return Result.Failed;
            }

            GeometryElement geometry = element.get_Geometry(new Options());

            try
            {
                new ServiceUtility().AddServer(uiDocument, geometry);
            }
            catch (Exception ex)
            {
                Application.Logger.Error("Failed to create new server.", ex);
            }

            uiDocument.UpdateAllOpenViews();

            return Result.Succeeded;
        }

        private Element PickElement(UIDocument uiDocument)
        {
            Reference elementReference = null;

            try
            {
                elementReference = uiDocument.Selection.PickObject(ObjectType.Element);
            }
            catch (Exception ex)
            {
                Application.Logger.Error("Failed to pick object.", ex);
                return null;
            }

            if (elementReference == null)
            {
                Application.Logger.Error("Failed to pick object.");
                return null;
            }

            Element element = uiDocument.Document.GetElement(elementReference);

            return element;
        }
    }
}
