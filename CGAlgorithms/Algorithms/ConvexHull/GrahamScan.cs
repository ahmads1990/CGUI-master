using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            //init
            int n = points.Count;
            //get corner start point
            Point p = points[0];
            for (int i = 0; i < n; i++)
            {
                if (points[i].Y < p.Y) p = points[i];
            }
            points.Remove(p);

        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
