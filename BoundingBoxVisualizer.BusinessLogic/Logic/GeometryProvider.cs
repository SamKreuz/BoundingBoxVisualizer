using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using BoundingBoxVisualizer.BusinessLogic.Logic.Model;
using System.Collections.Generic;

namespace BoundingBoxVisualizer.BusinessLogic.Logic
{
    internal class GeometryProvider
    {
        public void Setup()
        {
            ExternalServiceId serviceId = ExternalServices.BuiltInExternalServices.DirectContext3DService;
            ExternalService service = ExternalServiceRegistry.GetService(serviceId);

            var painter = new Painter();

            service.AddServer(painter);
        }

        //public void CreateGeometry(BoundingBoxXYZ boundingBox)
        //{
        //    XYZ min = boundingBox.Min;
        //    XYZ max = boundingBox.Max;

        //    var dist = max - min;

        //    var middle = min + dist;

        //    Mesh me = new Mesh(middle);

        //    GeometryCreationUtilities.

        //}

        //public void CreateSphere(XYZ center)
        //{
        //    Frame frame = new Frame(center, XYZ.BasisX, XYZ.BasisY, XYZ.BasisZ);

        //    Arc arc

        //}

        public Geometry GetGeometry()
        {
            //  TODO SK
            return new Geometry();
        }

        private Solid CreateExtrudedGeometry(CurveLoop loop)
        {
            List<CurveLoop> loops = new List<CurveLoop>();
            Solid geometry = GeometryCreationUtilities.CreateExtrusionGeometry(loops, XYZ.BasisZ, 20);

            return geometry;
        }

        private List<Mesh> GetMeshes(Solid solid)
        {
            List<Mesh> meshes = new List<Mesh>();
            foreach(Face face in solid.Faces)
            {
                meshes.Add(face.Triangulate());
            }
            return meshes;
        }
    }
}
