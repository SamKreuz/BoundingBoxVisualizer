using Autodesk.Revit.DB;
using Autodesk.Revit.DB.DirectContext3D;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.UI;
using BoundingBoxVisualizer.BusinessLogic.Logic.Model;
using System;

namespace BoundingBoxVisualizer.BusinessLogic.Logic
{
    internal class Painter : IDirectContext3DServer
    {
        public Painter()
        {
            GeometryProvider = new GeometryProvider();
            guid = Guid.NewGuid();
        }

        public GeometryProvider GeometryProvider { get; }

        private Guid guid;
        private ExternalServiceId id = ExternalServices.BuiltInExternalServices.DirectContext3DService;

        public bool CanExecute(View dBView) { return true; }
        public bool UseInTransparentPass(View dBView) { return false; }
        public bool UsesHandles() { return false; }
        public string GetApplicationId() { return string.Empty; }
        public string GetDescription() { return string.Empty; }
        public string GetName() { return string.Empty; }
        public string GetSourceId() { return string.Empty; }
        public string GetVendorId() { return string.Empty; }
        public Guid GetServerId() { return guid; }
        public ExternalServiceId GetServiceId() { return id; }

        public Outline GetBoundingBox(View dBView)
        {
            return GeometryProvider.GetBoundingBox();
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

            }
            catch (Exception e)
            {
                // TODO SK
                TaskDialog.Show("Exception", e.Message);

                // TODO SK: Compare to
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
        }
    }
}
