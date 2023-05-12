using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace BoundingBoxVisualizer.BusinessLogic.Logic
{
    internal class GeometryCreator
    {
        // Copied from Reivt SKD Sample 2023
        public Solid CreateCenterbasedBox(XYZ center, double edgelength)
        {
            double halfedgelength = edgelength / 2.0;

            List<CurveLoop> profileloops = new List<CurveLoop>();
            CurveLoop profileloop = new CurveLoop();
            profileloop.Append(Line.CreateBound(
               new XYZ(center.X - halfedgelength, center.Y - halfedgelength, center.Z - halfedgelength),
               new XYZ(center.X - halfedgelength, center.Y + halfedgelength, center.Z - halfedgelength)));
            profileloop.Append(Line.CreateBound(
               new XYZ(center.X - halfedgelength, center.Y + halfedgelength, center.Z - halfedgelength),
               new XYZ(center.X + halfedgelength, center.Y + halfedgelength, center.Z - halfedgelength)));
            profileloop.Append(Line.CreateBound(
               new XYZ(center.X + halfedgelength, center.Y + halfedgelength, center.Z - halfedgelength),
               new XYZ(center.X + halfedgelength, center.Y - halfedgelength, center.Z - halfedgelength)));
            profileloop.Append(Line.CreateBound(
               new XYZ(center.X + halfedgelength, center.Y - halfedgelength, center.Z - halfedgelength),
               new XYZ(center.X - halfedgelength, center.Y - halfedgelength, center.Z - halfedgelength)));
            profileloops.Add(profileloop);

            XYZ extrusiondir = new XYZ(0, 0, 1); // orthogonal

            double extrusiondist = edgelength;

            var solid = GeometryCreationUtilities.CreateExtrusionGeometry(profileloops, extrusiondir, extrusiondist);

            Transform transform = Transform.CreateRotation(XYZ.BasisZ, 0.785398);
            var transformedSolis = SolidUtils.CreateTransformed(solid, transform);

            return transformedSolis;
        }

        public Solid CreateGeometryFromBoundingBox(BoundingBoxXYZ boundingBox)
        {
            XYZ min = boundingBox.Min;
            XYZ max = boundingBox.Max;
            //var transform = boundingBox.Transform;

            // Offset to prevent z-fighting
            double offset = 0.02;
            XYZ offsetVal = new XYZ(offset, offset, offset);
            min -= offsetVal;
            max += offsetVal;

            XYZ pointA = new XYZ(min.X, min.Y, min.Z);
            XYZ pointB = new XYZ(min.X, max.Y, min.Z);
            XYZ pointC = new XYZ(max.X, max.Y, min.Z);
            XYZ pointD = new XYZ(max.X, min.Y, min.Z);

            CurveLoop profile = new CurveLoop();
            profile.Append(Line.CreateBound(pointA, pointB));
            profile.Append(Line.CreateBound(pointB, pointC));
            profile.Append(Line.CreateBound(pointC, pointD));
            profile.Append(Line.CreateBound(pointD, pointA));
            List<CurveLoop> profiles = new List<CurveLoop> { profile };

            XYZ extrusionDirection = XYZ.BasisZ;
            double extrusionDistance = max.Z - min.Z; 

            var solid = GeometryCreationUtilities.CreateExtrusionGeometry(profiles, extrusionDirection, extrusionDistance);

            return solid;
        }
    }
}
