using Autodesk.Revit.DB;
using Autodesk.Revit.DB.DirectContext3D;
using BoundingBoxVisualizer.Logic.Extensions;
using BoundingBoxVisualizer.Logic.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoundingBoxVisualizer.Logic.Logic
{
    internal class GeometryManager
    {
        private GeometryData faces;
        private GeometryData edges;
        private List<int> numVerticesInMeshesBefore = new List<int> { 0 };
        private List<int> numVerticesInEdgesBefore = new List<int> { 0 };
        private List<IList<XYZ>> edgePointGroups = new List<IList<XYZ>>();

        public void SetupData(GeometryObject geometryObject, ColorWithTransparency color)
        {
            List<Mesh> meshes = new List<Mesh>();

            faces = new GeometryData();
            edges = new GeometryData();

            if (geometryObject is GeometryElement geometryElement)
            {
                var solids = GetSolids(geometryElement);
                SetEdgeData(solids);
                meshes = GetMeshes(solids);
            }
            else if (geometryObject is Solid solid)
            {
                var solids = new List<Solid> { solid };
                SetEdgeData(solids);
                meshes = GetMeshes(solids);
            }

            //List<VertexPosition> vertices = GetVertexPositions(meshes);

            faces.Meshes = meshes;
            faces.Start = 0;
            faces.PrimitiveType = PrimitiveType.TriangleList;
            faces.VertexFormatBits = VertexFormatBits.PositionColored;
            faces.VertexFormat = new VertexFormat(faces.VertexFormatBits);
            faces.EffectInstance = new EffectInstance(faces.VertexFormatBits);
            faces.PrimitiveCount = CountTriangles(meshes);
            faces.VertexCount = CountVertices(meshes);
            faces.VertexBuffer = CreateVertexBufferPositionColor(meshes, faces.VertexCount, color);
            faces.IndexCount = GetIndexTriangleAsShortInts(faces.PrimitiveCount);
            faces.IndexBuffer = CreateIndexBufferTriangle(meshes, faces.IndexCount);

            edges.Start = 0;
            edges.PrimitiveType = PrimitiveType.LineList;   // TODO SK: Test point list
            edges.VertexFormatBits = VertexFormatBits.Position;
            edges.VertexFormat = new VertexFormat(edges.VertexFormatBits);
            edges.EffectInstance = new EffectInstance(edges.VertexFormatBits);
            edges.VertexBuffer = CreateVertexBufferEdges(edgePointGroups, edges.VertexCount);
            edges.IndexCount = GetIndexLineShortInts(edges.PrimitiveCount);
            edges.IndexBuffer = CreateIndexBufferLine(edgePointGroups, edges.IndexCount);
        }



        public GeometryData GetFacesData()
        {
            return faces;
        }

        public GeometryData GetEdgesData()
        {
            return edges;
        }

        private List<Solid> GetSolids(GeometryElement geometryElement)
        {
            List<Solid> solids = new List<Solid>();

            foreach (GeometryObject geomObject in geometryElement)
            {
                if (geomObject is Solid)
                {
                    var solid = (Solid)geomObject;

                    if (solid.Volume > 0)
                    {
                        solids.Add(solid);
                    }
                }
            }

            return solids;
        }

        private List<Mesh> GetMeshes(List<Solid> solids)
        {
            List<Mesh> meshes = new List<Mesh>();

            if (solids.Count > 0)
            {
                foreach (var solid in solids)
                {
                    foreach (Face face in solid.Faces)
                    {
                        Mesh mesh = face.Triangulate();

                        meshes.Add(mesh);
                    }
                }
            }
            return meshes;
        }

        private void SetEdgeData(List<Solid> solids)
        {
            int vertexCount = 0;
            int primitiveCount = 0;
            edgePointGroups.Clear();


            foreach (Solid solid in solids)
            {
                foreach(Edge edge in solid.Edges)
                {
                    IList<XYZ> points = edge.Tessellate();
                    
                    edgePointGroups.Add(points);
                    vertexCount += points.Count;
                    primitiveCount += points.Count - 1;
                }
            }

            edges.PrimitiveCount = primitiveCount;
            edges.VertexCount = vertexCount;
        }

        private List<VertexPosition> GetVertexPositions(List<Mesh> meshes)
        {
            List<VertexPosition> vertices = new List<VertexPosition>();

            foreach (Mesh mesh in meshes)
            {
                foreach (var vertex in mesh.Vertices)
                {
                    vertices.Add(new VertexPosition(vertex));
                }
            }

            return vertices;
        }

        private int CountTriangles(List<Mesh> meshes)
        {
            int numberOfTriangles = 0;

            foreach (Mesh mesh in meshes)
            {
                numberOfTriangles += mesh.NumTriangles;
            }

            return numberOfTriangles;
        }

        private int CountVertices(List<Mesh> meshes)
        {
            int numberOfVertices = 0;

            foreach (Mesh mesh in meshes)
            {
                numberOfVertices += mesh.Vertices.Count;
            }

            return numberOfVertices;
        }

        private int GetIndexTriangleAsShortInts(int primitiveCount)
        {
            return primitiveCount * IndexTriangle.GetSizeInShortInts();
        }

        private int GetIndexLineShortInts(int primitiveCount)
        {
            return primitiveCount * IndexLine.GetSizeInShortInts();
        }

        private IndexBuffer CreateIndexBufferTriangle(List<Mesh> meshes, int indexCount)
        {
            int meshNumber = 0;
            //int bufferSize = indexCount * IndexTriangle.GetSizeInShortInts();

            var buffer = new IndexBuffer(indexCount);

            buffer.Map(indexCount);

            IndexStreamTriangle stream = buffer.GetIndexStreamTriangle();

            foreach (Mesh mesh in meshes)
            {
                int startIndex = numVerticesInMeshesBefore[meshNumber];  // TODO SK: Apply as foreach loop?
                for (int i = 0; i < mesh.NumTriangles; i++)
                {
                    MeshTriangle mt = mesh.get_Triangle(i);

                    stream.AddTriangle(new IndexTriangle(
                                                startIndex + (int)mt.get_Index(0),
                                                startIndex + (int)mt.get_Index(1),
                                                startIndex + (int)mt.get_Index(2)));
                }
                meshNumber++;
            }
            buffer.Unmap();

            return buffer;
        }

        private IndexBuffer CreateIndexBufferLine(List<IList<XYZ>> edges, int indexCount)
        {
            int edgeNumber = 0;

            var buffer = new IndexBuffer(indexCount);

            buffer.Map(indexCount);

            IndexStreamLine stream = buffer.GetIndexStreamLine();

            foreach (IList<XYZ> edge in edges)
            {
                int startIndex = numVerticesInEdgesBefore[edgeNumber];  // TODO SK: Apply as foreach loop?
                for (int i = 1; i < edge.Count; i++)
                {
                    stream.AddLine(new IndexLine(startIndex + i - 1,
                                                 startIndex + i));
                }
                edgeNumber++;
            }

            buffer.Unmap();

            return buffer;
        }


        private VertexBuffer CreateVertexBufferPositionColor(List<Mesh> meshes, int vertexCount, ColorWithTransparency color)
        {
            int bufferSize = VertexPositionColored.GetSizeInFloats() * vertexCount;

            var buffer = new VertexBuffer(bufferSize);

            buffer.Map(bufferSize);

            VertexStreamPositionColored vertexStream = buffer.GetVertexStreamPositionColored();

            foreach (Mesh mesh in meshes)
            {
                try
                {
                    var vertexPositions = mesh.VertexPositionsColored(color);
                    vertexStream.AddVertices(vertexPositions);
                }
                catch (Exception ex)
                {
                    // TODO SK
                }

                numVerticesInMeshesBefore.Add(numVerticesInMeshesBefore.Last() + mesh.Vertices.Count);
            }


            buffer.Unmap();

            return buffer;
        }

        private VertexBuffer CreateVertexBufferEdges(List<IList<XYZ>> edges, int vertexCount)
        {
            int bufferSize = VertexPosition.GetSizeInFloats() * vertexCount;

            var buffer = new VertexBuffer(bufferSize);

            buffer.Map(bufferSize);

            VertexStreamPosition vertexStream = buffer.GetVertexStreamPosition();

            foreach (IList<XYZ> pointList in edges)
            {
                foreach(XYZ point in pointList)
                {
                    try
                    {
                        vertexStream.AddVertex(new VertexPosition(point));  // TODO SK: make extension
                    }
                    catch (Exception ex)
                    {
                        // TODO SK
                    }
                }

                numVerticesInEdgesBefore.Add(numVerticesInEdgesBefore.Last() + pointList.Count);
            }

            buffer.Unmap();

            return buffer;
        }
    }
}