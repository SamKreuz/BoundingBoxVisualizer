using Autodesk.Revit.DB;
using Autodesk.Revit.DB.DirectContext3D;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.UI;
using BoundingBoxVisualizer.Logic.Logic.Model;
using System;

namespace BoundingBoxVisualizer.Logic.Logic
{
    internal class Painter : IDirectContext3DServer
    {
        public Document Document { get { return uiDocument.Document; } }

        private GeometryObject geometryObject;
        private ColorWithTransparency color;
        private UIDocument uiDocument;
        public Painter(UIDocument uiDocuemnt, GeometryObject geometryElement, ColorWithTransparency color)
        {
            this.uiDocument = uiDocuemnt;
            this.geometryObject = geometryElement;
            this.color = color;
            GeometryProvider = new GeometryManager();
            guid = Guid.NewGuid();
        }

        public GeometryManager GeometryProvider { get; }

        private Guid guid;
        private ExternalServiceId id = ExternalServices.BuiltInExternalServices.DirectContext3DService;

        public bool CanExecute(View dBView) { return true; }
        public bool UseInTransparentPass(View dBView) { return false; }
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
                    else if (geometryObject is Solid solid)
                    {
                        boundingBox = solid.GetBoundingBox();
                    }
                }
                catch (Exception ex)
                {
                    Application.Logger.Error("Failed to get bounding box.", ex);
                }
            }

            return new Outline(boundingBox.Min, boundingBox.Max);
        }

        public void RenderScene(View dBView, DisplayStyle displayStyle)
        {
            if (DrawContext.IsTransparentPass())
            {
                GeometryProvider.SetupData(geometryObject, color);
                GeometryData facesData = GeometryProvider.GetFacesData();
                GeometryData edgesData = GeometryProvider.GetEdgesData();

                if (facesData == null)
                {
                    Application.Logger.Error("Failed to get faces data.");
                    return;
                }

                try
                {
                    DrawContext.FlushBuffer(
                        facesData.VertexBuffer,
                        facesData.VertexCount,
                        facesData.IndexBuffer,
                        facesData.IndexCount,
                        facesData.VertexFormat,
                        facesData.EffectInstance,
                        facesData.PrimitiveType,
                        facesData.Start,
                        facesData.PrimitiveCount);

                    DrawContext.FlushBuffer(
                        edgesData.VertexBuffer,
                        edgesData.VertexCount,
                        edgesData.IndexBuffer,
                        edgesData.IndexCount,
                        edgesData.VertexFormat,
                        edgesData.EffectInstance,
                        edgesData.PrimitiveType,
                        edgesData.Start,
                        edgesData.PrimitiveCount);
                }
                catch (Exception ex)
                {
                    Application.Logger.Error("Failed to render scene.", ex);
                }
            }
        }
    }
}
