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
            //special case
            if (points.Count <= 3) { outPoints = points; return; }
            //init data
            int n = points.Count;
            Stack<Point> stackPoints = new Stack<Point>();
            List<KeyValuePair<int, double>> pointAnglePairs;
            //get corner start point lowest Y point
            Point p = points[0];
            for (int i = 0; i < n; i++)
            {
                if (points[i].Y < p.Y) p = points[i];
            }
            //remove it from list of point and reduce number of points
            points.Remove(p);
            n--;
            //push starting point to the stack
            stackPoints.Push(p);
            //calculate angle between line (startPoint, eachPoint) and x axis
            pointAnglePairs = new List<KeyValuePair<int, double>>(n);
            for (int i = 0; i < n; i++)
            {
                //vector startpoint p to point i
                Point vector = p.Vector(points[i]);
                //get angle and convert radian to degree
                double angles = Math.Atan2(vector.Y, vector.X) * (180 / Math.PI);
                pointAnglePairs.Insert(i, new KeyValuePair<int, double>(i, angles));
            }
            //sort points according to its angle
            pointAnglePairs.Sort((x, y) => x.Value.CompareTo(y.Value));
            //push most right point to stack to begin with
            //start from second point
            stackPoints.Push(points[pointAnglePairs[0].Key]);
            for (int i = 1; i < n; i++)
            {
                /*comparison 
                * c => new point
                * b => point before c
                * a => point before a
                */
                Point c = points[pointAnglePairs[i].Key];
                Point b = stackPoints.Pop();
                Point a = stackPoints.Peek();
                //check if turn in tri abc 
                //not left => dump b and go back one point
                while (HelperMethods.CheckTurn(new Line(a, b), c) != Enums.TurnType.Left)
                {
                    if (stackPoints.Count == 1) break;
                    //b=a
                    b = stackPoints.Pop();
                    a = stackPoints.Peek();
                }
                //at the end push new point c and before it b to the stack
                stackPoints.Push(b);
                stackPoints.Push(c);
            }
            //done convert stack to list
            outPoints = stackPoints.ToList();
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
