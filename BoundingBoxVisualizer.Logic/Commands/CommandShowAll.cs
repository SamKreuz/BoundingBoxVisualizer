using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BoundingBoxVisualizer.Logic.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoundingBoxVisualizer.Logic.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class CommandShowAll : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;

            var geometryElements = GetAllElementGeometries(document);

            try
            {
                foreach (var geometry in geometryElements)
                {
                    new ServiceUtility().AddServer(uiDocument, geometry);
                }
            }
            catch (Exception ex)
            {
                // TODO SK: Log
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        private List<GeometryElement> GetAllElementGeometries(Document document)
        {
            var elements = new List<Element>();

            List<BuiltInCategory> categories = new List<BuiltInCategory>
            {
                BuiltInCategory.OST_DuctCurves,
                BuiltInCategory.OST_DuctFitting,
                BuiltInCategory.OST_PipeCurves,
                BuiltInCategory.OST_PipeFitting
            };

            var filter = new ElementMulticategoryFilter(categories);

            using (var coll = new FilteredElementCollector(document))
            {
                elements = coll.WhereElementIsNotElementType().WherePasses(filter).ToElements().ToList();
            }

            List<GeometryElement> elementGeometry = elements.Select(x => x.get_Geometry(new Options()))
                                                            .Where(y => y != null)
                                                            .ToList();

            return elementGeometry;
        }
    }
}
