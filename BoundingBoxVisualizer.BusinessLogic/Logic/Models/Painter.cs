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
        private GeometryObject geometryObject;
        public Painter(GeometryObject geometryElement)
        {
            this.geometryObject = geometryElement;
            GeometryProvider = new GeometryManager();
            guid = Guid.NewGuid();
        }

        public GeometryManager GeometryProvider { get; }

        private Guid guid;
        private ExternalServiceId id = ExternalServices.BuiltInExternalServices.DirectContext3DService;

        public bool CanExecute(View dBView) { return true; }
        public bool UseInTransparentPass(View dBView) { return true; }  // TODO SK: Change
        public bool UsesHandles() { return false; }
        public string GetApplicationId() { return string.Empty; }
        public string GetDescription() { return "Draws geometry inside of a Revit model."; }
        public string GetName() { return "Revit Drawing Server"; }
        public string GetSourceId() { return string.Empty; }
        public string GetVendorId() { return "Samuel Kreuz"; }
        public Guid GetServerId() { return guid; }
        public ExternalServiceId GetServiceId() { return id; }

        public Outline GetBoundingBox(View dBView)
        {
            var boundingBox = new BoundingBoxXYZ();

            if (geometryObject != null)
            {
                try
                {
                    if (geometryObject is GeometryElement geometryElement)
                    {
                        boundingBox = geometryElement.GetBoundingBox();
                    }
                    else if(geometryObject is Solid solid)
                    {
                        boundingBox = solid.GetBoundingBox();
                    }
                }
                catch(Exception ex)
                {
                    // TODO SK: Log
                }
            }

            return new Outline(boundingBox.Min, boundingBox.Max);
        }

        public void RenderScene(View dBView, DisplayStyle displayStyle)
        {
            GeometryProvider.SetupData(geometryObject);
            GeometryData geometryData = GeometryProvider.GetData();

            if(geometryData == null)
            {
                // TODO SK: Log
                return;
            }

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
