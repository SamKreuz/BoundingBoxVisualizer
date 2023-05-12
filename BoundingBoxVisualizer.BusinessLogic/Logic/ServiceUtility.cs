using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using BoundingBoxVisualizer.BusinessLogic.Logic.Model;
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

            var painterElement = new Painter(geometryElement);
            //var painterGeometry = new Painter(solid);

            service.AddServer(painterElement);
            //service.AddServer(painterGeometry);

            // TODO SK: Check if needed. Probably only for multiple servers;
            MultiServerService msDirectContext3DService = service as MultiServerService;
            
            IList<Guid> serverIds = msDirectContext3DService.GetActiveServerIds();
            serverIds.Add(painterElement.GetServerId());
            //serverIds.Add(painterGeometry.GetServerId());

            msDirectContext3DService.SetActiveServers(serverIds);
        }
    }
}
