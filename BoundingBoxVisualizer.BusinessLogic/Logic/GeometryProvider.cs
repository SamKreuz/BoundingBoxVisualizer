using Autodesk.Revit.DB;
using Autodesk.Revit.DB.DirectContext3D;
using System;
using System.Collections.Generic;

namespace BoundingBoxVisualizer.BusinessLogic.Logic.Model
{
    internal class GeometryProvider
    {
        private GeometryData geometry;

        public GeometryProvider(GeometryElement geometryElement)
        {
            int floatSize = VertexPosition.GetSizeInFloats();
            List<Mesh> meshes = GetMeshes(geometryElement);
            List<VertexPosition> vertices = GetVertexPositions(meshes);

            geometry = new GeometryData();

            geometry.Meshes = meshes;

            geometry.VertexFormatBits = VertexFormatBits.Position;
            geometry.VertexFormat = new VertexFormat(geometry.VertexFormatBits);
            geometry.EffectInstance = new EffectInstance(geometry.VertexFormatBits);

            geometry.PrimitiveCount = CountTriangles(meshes);
            geometry.VertexCount = CountVertices(meshes);
            geometry.IndexCount = CountIndices(meshes, geometry.PrimitiveCount);

            geometry.IndexBuffer = CreateIndexBuffer();
            geometry.VertexBuffer = CreateVertexBuffer();
        }

        public GeometryData GetData()
        {
            return geometry;
        }

        public Outline GetBoundingBox()
        {
            // TODO SK
            var boundingBox = new BoundingBoxXYZ();

            Outline outline = new Outline(boundingBox.Min, boundingBox.Max);

            return outline;
        }

        private List<Mesh> GetMeshes(GeometryElement geometryElement)
        {
            List<Mesh> meshes = new List<Mesh>();
            List<Solid> solids = new List<Solid>();

            foreach(GeometryObject geomObject in geometry.Meshes)
            {
                if(geomObject is Solid)
                {
                    var solid = (Solid)geomObject;

                    if(solid.Volume > 0)
                    {
                        solids.Add(solid);
                    }
                }
            }

            if(solids.Count > 0)
            {
                foreach(var solid in solids)
                {
                    foreach(Face face in solid.Faces)
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
           
            foreach(Mesh mesh in meshes)
            {
                foreach(var vertex in mesh.Vertices)
                {
                    vertices.Add(new VertexPosition(vertex));
                }
            }

            return vertices;
        }

        private int CountTriangles(List<Mesh> meshes) 
        {
            int numberOfTriangles = 0;

            foreach(Mesh mesh in meshes)
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

        private int CountIndices(List<Mesh> meshes, int primitiveCount)
        {
            int indexBufferSize = primitiveCount * IndexTriangle.GetSizeInShortInts();

            return indexBufferSize;
        }

        private IndexBuffer CreateIndexBuffer()
        {

        }

        private VertexBuffer CreateVertexBuffer(int bufferSize, List<VertexPosition> vertices)
        {
            var buffer = new VertexBuffer(bufferSize);

            buffer.Map(bufferSize);

            try
            {
                VertexStreamPosition vertexStream = buffer.GetVertexStreamPosition();
                vertexStream.AddVertices(vertices);
            }
            catch(Exception ex)
            {
                // TODO SK
            }

            buffer.Unmap();

            return buffer;
        }
    }
}