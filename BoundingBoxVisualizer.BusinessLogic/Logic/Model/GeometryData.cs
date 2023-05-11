using Autodesk.Revit.DB.DirectContext3D;
using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace BoundingBoxVisualizer.BusinessLogic.Logic.Model
{
    public class GeometryData
    {
        public int Start { get; }
        public int PrimitiveCount { get; }
        public int VertexCount { get; }
        public int IndexCount { get; }
        public VertexBuffer VertexBuffer { get; }
        public IndexBuffer IndexBuffer { get; }
        public VertexFormat VertexFormat { get; }
        public EffectInstance EffectInstance { get; }
        public List<Mesh> Meshes { get; }
        public PrimitiveType PrimitiveType { get; }
        public VertexFormatBits VertexFormatBits { get; }
    }
}
