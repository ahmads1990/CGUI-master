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
            if (points.Count <= 3) { outPoints = points; return; }
            points = points.OrderBy(x => x.Y).ToList();
            int n = points.Count;
            Point a = points[0];
            Point b;

            // draw line behind first point to get the second point
            Point newp = new Point(a.X, a.Y + 10);
            Line first_line = new Line(a, newp);

            outPoints.Add(a);
            //find next point

            double angleHolder = 0;
            int index = -1;


            for (int i = 1; i < n; i++)
            {
                Line test_line = new Line(a, points[i]);
                Point vectorBA = HelperMethods.GetVector(first_line);
                Point vectorBC = HelperMethods.GetVector(test_line);

                double Numerator = vectorBA.X * vectorBC.X + vectorBA.Y * vectorBC.Y;
                double denominator = vectorBA.Magnitude() * vectorBC.Magnitude();

                double angle = Math.Acos(Numerator / denominator);
                if (angle >= angleHolder)
                {
                    // cheak if the point have the same max angle value
                    // but in the middle of the line i have aleady 
                    // I ignore the point in this case
                    if (angle == angleHolder)
                    {
                        if (HelperMethods.PointOnSegment(points[i], points[index], a)) continue;
                    }
                    angleHolder = angle;
                    index = i;
                }
            }
            b = points[index];
            outPoints.Add(b);

            //search
            for (int i = 2; i < n; i++)
            {
                angleHolder = 0;
                Line ba = new Line(b, a);
                for (int c = 0; c < n; c++)
                {
                    Line bc = new Line(b, points[c]);

                    Point vectorBA = HelperMethods.GetVector(ba);
                    Point vectorBC = HelperMethods.GetVector(bc);


                    double Numerator = vectorBA.X * vectorBC.X + vectorBA.Y * vectorBC.Y;
                    double denominator = vectorBA.Magnitude() * vectorBC.Magnitude();
                    double angle = Math.Acos(Numerator / denominator);

                    if (angle >= angleHolder)
                    {
                        // cheak if the point have the same max angle value
                        // but in the middle of the line i have aleady 
                        // I ignore the point in this case
                        if (angle == angleHolder)
                        {
                            if (HelperMethods.PointOnSegment(points[c], points[index], b)) continue;
                        }
                        angleHolder = angle;
                        index = c;
                    }
                }
                //new point
                a = b;
                b = points[index];
                // chcak if the point already in the outbut list or not
                if (outPoints.Contains(b) == false) outPoints.Add(b);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}