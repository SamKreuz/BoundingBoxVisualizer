using Autodesk.Revit.DB;
using Autodesk.Revit.DB.DirectContext3D;
using System.Collections.Generic;

namespace BoundingBoxVisualizer.BusinessLogic.Extensions
{
    public static class MeshExtensions
    {
        public static List<VertexPosition> VertexPositions(this Mesh mesh)
        {
            List<VertexPosition> vertices = new List<VertexPosition>();

            foreach(var vertex in mesh.Vertices)
            {
                vertices.Add(new VertexPosition(vertex));
            }

            return vertices;
        }
    }
}
