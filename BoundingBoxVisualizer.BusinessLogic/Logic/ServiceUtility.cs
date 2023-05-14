using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using System;
using System.Collections.Generic;

namespace BoundingBoxVisualizer.BusinessLogic.Logic
{
    internal class ServiceUtility
    {
        private ExternalService service;
        public ServiceUtility()
        {
            ExternalServiceId serviceId = ExternalServices.BuiltInExternalServices.DirectContext3DService;
            service = ExternalServiceRegistry.GetService(serviceId);
        }

        public void AddServer(GeometryElement geometryElement)
        {
            var boundingBox = geometryElement.GetBoundingBox();
            var transform = boundingBox.Transform;
            
            var geometryCreater = new GeometryCreator();
            Solid solid = geometryCreater.CreateGeometryFromBoundingBox(boundingBox);
            //Solid solid = geometryCreater.CreateCenterbasedBox(XYZ.Zero, 10);
            
            //Transparency is value between 0-255
            var colorRed = new ColorWithTransparency(255, 0, 0, 200);
            var colorGreen = new ColorWithTransparency(0, 255, 0, 200);

            //var painterElement = new Painter(geometryElement, colorRed);
            var painterGeometry = new Painter(solid, colorGreen);

            SetServer(painterGeometry);
        }

        private bool SetServer(Painter painter)
        {
            try
            {
                service.AddServer(painter);

                MultiServerService msDirectContext3DService = service as MultiServerService;

                IList<Guid> serverIds = msDirectContext3DService.GetActiveServerIds();
                //serverIds.Add(painterElement.GetServerId());
                serverIds.Add(painter.GetServerId());

                msDirectContext3DService.SetActiveServers(serverIds);
            }catch(Exception e)
            {
                //TODO SK: Log
                return false;
            }

            return true;
        }
    }
}
