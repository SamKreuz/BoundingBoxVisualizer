using Autodesk.Revit.DB;
using Autodesk.Revit.DB.DirectContext3D;
using System;
using System.Collections.Generic;

namespace BoundingBoxVisualizer.BusinessLogic.Logic.Model
{
    internal class GeometryProvider
    {
        private GeometryData geometry;

        public GeometryProvider()
        {
            geometry = new GeometryData();

            //TODO SK: SetUp Geometry


            //Meshes = meshes;

            //PrimitiveType = PrimitiveType.TriangleList;
            //vertexFormatBits = VertexFormatBits.Position;

            //VertexFormat = new VertexFormat(vertexFormatBits);
            //EffectInstance = new EffectInstance(vertexFormatBits);

            //List<VertexPositionColored> vertices = new List<VertexPositionColored>();

            //foreach(Mesh mesh in meshes)
            //{
            //    vertices.Add(new VertexPositionColored())
            //}

            //SetUpVertexStream
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

        private ColorWithTransparency color = new ColorWithTransparency(255, 0, 0, 0);

        public void ProcessFace()
        {

        }

        public VertexStreamPositionColored SetUpVertexStream(List<VertexPositionColored> vertexList)
        {
            VertexStreamPositionColored stream = VertexBuffer.GetVertexStreamPositionColored();
            
             try
            {
                stream.AddVertices(vertexList);
            }
            catch(Exception e)
            {
                // TODO SK
            }

            return stream;
        }
    }
}