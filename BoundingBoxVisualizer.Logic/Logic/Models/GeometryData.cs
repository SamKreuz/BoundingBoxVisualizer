using Autodesk.Revit.DB.DirectContext3D;
using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace BoundingBoxVisualizer.Logic.Logic.Model
{
    public class GeometryData
    {
        public int Start { get; set; }
        public int PrimitiveCount { get; set; }
        public int VertexCount { get; set; }
        public int IndexCount { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public VertexFormat VertexFormat { get; set; }
        public EffectInstance EffectInstance { get; set;  }
        public List<Mesh> Meshes { get; set; }
        public PrimitiveType PrimitiveType { get; set; }
        public VertexFormatBits VertexFormatBits { get; set; }
    }
}
