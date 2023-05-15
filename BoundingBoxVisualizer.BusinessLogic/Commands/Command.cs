using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using BoundingBoxVisualizer.Logic.Logic;
using System;

namespace BoundingBoxVisualizer.Logic
{
    [Transaction(TransactionMode.Manual)]
    internal class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;

            Element element = PickElement(uiDocument);

            if(element == null)
            {
                return Result.Failed;
            }

            GeometryElement geometry = element.get_Geometry(new Options());
            
            new ServiceUtility().AddServer(uiDocument, geometry);

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
            catch(Exception e)
            {
                // TODO SK
                return null;
            }

            if(elementReference == null)
            {
                // TODO SK
                return null;
            }

            Element element = uiDocument.Document.GetElement(elementReference);

            return element;
        }
    }
}
