using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;

namespace BoundingBoxVisualizer.BusinessLogic
{
    internal class Run : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;

            Element element = PickElement(uiDocument);

            if(element == null)
            {
                return Result.Failed;
            }

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
                // TODO SK;
            }

            Element element = uiDocument.Document.GetElement(elementReference);

            return element;
        }
    }
}
