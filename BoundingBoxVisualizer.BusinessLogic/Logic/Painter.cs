using Autodesk.Revit.DB;
using Autodesk.Revit.DB.DirectContext3D;
using Autodesk.Revit.DB.ExternalService;
using BoundingBoxVisualizer.BusinessLogic.Logic.Model;
using System;

namespace BoundingBoxVisualizer.BusinessLogic.Logic
{
    internal class Painter : IDirectContext3DServer
    {
        public Painter()
        {
            Position = XYZ.Zero;
            GeometryProvider = new GeometryProv(Position);
        }

        public GeometryProv GeometryProvider { get; }
        public XYZ Position { get; set; }

        public bool CanExecute(View dBView)
        {
            throw new NotImplementedException();
        }

        public string GetApplicationId()
        {
            throw new NotImplementedException();
        }

        public Outline GetBoundingBox(View dBView)
        {
            throw new NotImplementedException();
        }

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public Guid GetServerId()
        {
            throw new NotImplementedException();
        }

        public ExternalServiceId GetServiceId()
        {
            throw new NotImplementedException();
        }

        public string GetSourceId()
        {
            throw new NotImplementedException();
        }

        public string GetVendorId()
        {
            throw new NotImplementedException();
        }

        public void RenderScene(View dBView, DisplayStyle displayStyle)
        {
            GeometryData geometryData = GeometryProvider.GetData();

            try
            {
                DrawContext.FlushBuffer(
                    geometryData.VertexBuffer,
                    geometryData.VertexCount,
                    geometryData.IndexBuffer,
                    geometryData.IndexCount,
                    geometryData.VertexFormat,
                    geometryData.EffectInstance,
                    geometryData.PrimitiveType,
                    geometryData.Start,
                    geometryData.PrimitiveCount);

            }catch(Exception e)
            {
                // TODO SK
            }


            throw new NotImplementedException();
        }

        public bool UseInTransparentPass(View dBView)
        {
            throw new NotImplementedException();
        }

        public bool UsesHandles()
        {
            throw new NotImplementedException();
        }
    }
}
