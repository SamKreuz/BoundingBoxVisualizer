using Autodesk.Revit.DB;
using Autodesk.Revit.DB.DirectContext3D;
using BoundingBoxVisualizer.BusinessLogic.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoundingBoxVisualizer.BusinessLogic.Logic.Model
{
    internal class GeometryProvider
    {
        private GeometryData geometry;
        private List<int> numVerticesInMeshesBefore = new List<int> { 0 };

        public void SetupData(GeometryElement geometryElement)
        {
            List<Mesh> meshes = GetMeshes(geometryElement);
            List<VertexPosition> vertices = GetVertexPositions(meshes);

            geometry = new GeometryData();

            geometry.Meshes = meshes;
            geometry.Start = 0;
            geometry.PrimitiveType = PrimitiveType.TriangleList;
            geometry.VertexFormatBits = VertexFormatBits.PositionColored;
            geometry.VertexFormat = new VertexFormat(geometry.VertexFormatBits);
            geometry.EffectInstance = new EffectInstance(geometry.VertexFormatBits);
            geometry.PrimitiveCount = CountTriangles(meshes);
            geometry.VertexCount = CountVertices(meshes);

            geometry.VertexBuffer = CreateVertexBuffer(meshes, geometry.VertexCount);

            geometry.IndexCount = GetIndicesAsShortInts(geometry.PrimitiveCount);
            geometry.IndexBuffer = CreateIndexBuffer(meshes, geometry.IndexCount);

        }

        public GeometryData GetData()
        {
            return geometry;
        }

        private List<Mesh> GetMeshes(GeometryElement geometryElement)
        {
            List<Mesh> meshes = new List<Mesh>();
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

        private int GetIndicesAsShortInts(int primitiveCount)
        {
            return primitiveCount * IndexTriangle.GetSizeInShortInts();
        }

        private IndexBuffer CreateIndexBuffer(List<Mesh> meshes, int indexCount)
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

        private VertexBuffer CreateVertexBuffer(List<Mesh> meshes, int vertexCount)
        {
            int bufferSize = VertexPositionColored.GetSizeInFloats() * vertexCount;

            var color = new ColorWithTransparency(255, 0, 0, 0);
            var buffer = new VertexBuffer(bufferSize);

            buffer.Map(bufferSize);

            VertexStreamPositionColored vertexStream = buffer.GetVertexStreamPositionColored();

            foreach (Mesh mesh in meshes)
            {

                foreach (var vertex in mesh.Vertices)
                {
                    try
                    {
                        // TODO SK: Delete extension .AddVertices
                        vertexStream.AddVertex(new VertexPositionColored(vertex, color));
                    }
                    catch (Exception ex)
                    {
                        // TODO SK
                    }

                }


                numVerticesInMeshesBefore.Add(numVerticesInMeshesBefore.Last() + mesh.Vertices.Count);
            }


            buffer.Unmap();

            return buffer;
        }
    }
}