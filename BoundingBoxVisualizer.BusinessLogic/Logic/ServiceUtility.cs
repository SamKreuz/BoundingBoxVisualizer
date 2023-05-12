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

            Solid solid = new GeometryCreator().CreateCenterbasedBox(XYZ.Zero, 10);
            
            //Transparency is value between 0-255
            var color = new ColorWithTransparency(255, 0, 0, 200);

            var painterElement = new Painter(geometryElement, color);
            var painterGeometry = new Painter(solid, color);

            service.AddServer(painterElement);
            service.AddServer(painterGeometry);

            // TODO SK: Check if needed. Probably only for multiple servers;
            MultiServerService msDirectContext3DService = service as MultiServerService;
            
            IList<Guid> serverIds = msDirectContext3DService.GetActiveServerIds();
            serverIds.Add(painterElement.GetServerId());
            serverIds.Add(painterGeometry.GetServerId());

            msDirectContext3DService.SetActiveServers(serverIds);
        }
    }
}
