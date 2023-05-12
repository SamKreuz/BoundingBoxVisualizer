using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using System;
using System.Collections.Generic;

namespace BoundingBoxVisualizer.BusinessLogic.Logic
{
    internal class ServiceUtility
    {
        public void Setup(GeometryElement geometryElement)
        {
            ExternalServiceId serviceId = ExternalServices.BuiltInExternalServices.DirectContext3DService;
            ExternalService service = ExternalServiceRegistry.GetService(serviceId);
            var geometryCreater = new GeometryCreator();
            var bb = geometryElement.GetBoundingBox();
            var t = bb.Transform;

            Solid solid = geometryCreater.CreateGeometryFromBoundingBox(bb);
            //Solid solid = geometryCreater.CreateCenterbasedBox(XYZ.Zero, 10);
            
            //Transparency is value between 0-255
            var colorRed = new ColorWithTransparency(255, 0, 0, 200);
            var colorGreen = new ColorWithTransparency(0, 255, 0, 200);

            //var painterElement = new Painter(geometryElement, colorRed);
            var painterGeometry = new Painter(solid, colorGreen);

            //service.AddServer(painterElement);
            service.AddServer(painterGeometry);

            // TODO SK: Check if needed. Probably only for multiple servers;
            MultiServerService msDirectContext3DService = service as MultiServerService;
            
            IList<Guid> serverIds = msDirectContext3DService.GetActiveServerIds();
            //serverIds.Add(painterElement.GetServerId());
            serverIds.Add(painterGeometry.GetServerId());

            msDirectContext3DService.SetActiveServers(serverIds);
        }
    }
}
