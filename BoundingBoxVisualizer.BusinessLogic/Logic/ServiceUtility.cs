﻿using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.UI;
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

        public void AddServer(UIDocument uiDocument, GeometryElement geometryElement)
        {
            var boundingBox = geometryElement.GetBoundingBox();
            var geometryCreater = new GeometryCreator();
            Solid solid = geometryCreater.CreateGeometryFromBoundingBox(boundingBox);
            
            //Transparency is value between 0-255
            var colorRed = new ColorWithTransparency(255, 0, 0, 200);
            var colorGreen = new ColorWithTransparency(0, 255, 0, 200);

            var painterGeometry = new Painter(uiDocument, solid, colorGreen);

            SetServer(painterGeometry);
        }

        public void RemoveAllServers(Document document)
        {
            MultiServerService directContext3DService = service as MultiServerService;

            if (directContext3DService == null)
            {
                return;
            }

            var registeredIds = directContext3DService.GetRegisteredServerIds();

            foreach(var id in registeredIds)
            {
                Painter painter = directContext3DService.GetServer(id) as Painter;

                if(painter.Document.GetHashCode() == document.GetHashCode())
                {
                    // TODO SK: Remove server
                }
            }
        }

        private bool SetServer(Painter painter)
        {
            try
            {
                service.AddServer(painter);

                MultiServerService msDirectContext3DService = service as MultiServerService;

                if(msDirectContext3DService == null)
                {
                    return true;
                }

                IList<Guid> serverIds = msDirectContext3DService.GetActiveServerIds();
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
