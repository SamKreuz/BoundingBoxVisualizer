using Autodesk.Revit.DB;
using Autodesk.Revit.DB.DirectContext3D;
using System.Collections.Generic;

namespace BoundingBoxVisualizer.Logic.Extensions
{
    public static class MeshExtensions
    {
        public static List<VertexPositionColored> VertexPositionsColored(this Mesh mesh, ColorWithTransparency color)
        {
            List<VertexPositionColored> vertices = new List<VertexPositionColored>();

            foreach(var vertex in mesh.Vertices)
            {
                vertices.Add(new VertexPositionColored(vertex, color));
            }

            return vertices;
        }
    }
}
