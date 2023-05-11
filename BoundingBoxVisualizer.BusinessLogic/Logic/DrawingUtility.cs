using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using BoundingBoxVisualizer.BusinessLogic.Logic.Model;
using System;
using System.Collections.Generic;

namespace BoundingBoxVisualizer.BusinessLogic.Logic
{
    internal class DrawingUtility
    {
        public void Setup(GeometryElement geometryElement)
        {
            ExternalServiceId serviceId = ExternalServices.BuiltInExternalServices.DirectContext3DService;
            ExternalService service = ExternalServiceRegistry.GetService(serviceId);

            var painter = new Painter(geometryElement);
            service.AddServer(painter);


            // TODO SK: Check if needed. Probably only for multiple servers;
            MultiServerService msDirectContext3DService = service as MultiServerService;
            IList<Guid> serverIds = msDirectContext3DService.GetActiveServerIds();
            serverIds.Add(painter.GetServerId());
            msDirectContext3DService.SetActiveServers(serverIds);
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
