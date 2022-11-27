using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count <= 3) { outPoints = points;return; }
            //
            Enums.TurnType turnDirection = Enums.TurnType.Right;
            points = points.OrderBy(x => x.Y).ToList();
            int n = points.Count;
            Point a = points[0];
            Point b;
            outPoints.Add(a);
            //find next point
            double angleHolder = 0;
            int index = -1;
            for (int i = 1; i < n; i++)
            {
                double y = Math.Abs(a.Y - points[i].Y);
                double x = Math.Abs(a.X - points[i].X);
                double angle = Math.Atan2(y, x);
                if (angle > angleHolder)
                {
                    angleHolder = angle;
                    index = i;
                }
            }
            b = points[index];
            outPoints.Add(b);

            //search
            for (int i = 2; i < n; i++)
            {
                Line ba = new Line( b,a);
                angleHolder = 0;
                for (int c = 0; c < n; c++)
                {
                    Line bc = new Line(b, points[c]);
                    Point vectorBA = HelperMethods.GetVector(ba);
                    Point vectorBC = HelperMethods.GetVector(bc);
                    double angle = Math.Acos(
                            (vectorBA.X * vectorBC.X + vectorBA.Y * vectorBC.Y) /
                            vectorBA.Magnitude() * vectorBC.Magnitude()
                        );
                    if(angle > angleHolder)
                    {
                        angleHolder = angle;
                        index = c;
                    }
                }
                //new point
                a = b;
                b=points[index];
                outPoints.Add(b);
            }
            Console.WriteLine();
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
