﻿using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace BoundingBoxVisualizer.BusinessLogic.Logic
{
    internal class GeometryCreator
    {
        //public Solid CreateSphere()
        //{
        //    GeometryCreationUtilities.CreateExtrusionGeometry()
        //}

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

            return GeometryCreationUtilities.CreateExtrusionGeometry(profileloops, extrusiondir, extrusiondist);
        }
    }
}
