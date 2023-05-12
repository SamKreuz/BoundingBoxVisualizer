using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoundingBoxVisualizer.BusinessLogic.Logic.Models
{
    internal class Line
    {
        public XYZ PointA { get; set; }
        public XYZ PointB { get; set; }
    }
}
