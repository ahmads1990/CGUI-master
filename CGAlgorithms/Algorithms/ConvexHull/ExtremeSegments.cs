using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }
            //1,2,3 special case
            int n = points.Count;
            Enums.TurnType? convexHullTurn = null;
            //first pick point i
            for (int i = 0; i < n; i++)
            {
                //second point j
                for (int j = 0; j < n; j++)
                {
                    //i != j
                    if (i == j) continue;
                    //line ij
                    Line lineIJ = new Line(points[i], points[j]);
                    Enums.TurnType? lineTurn = null;
                    //test with other points k
                    for (int k = 0; k < n; k++)
                    {
                        if (k == i || k == j) continue;
                        Enums.TurnType testResult = HelperMethods.CheckTurn(lineIJ, points[k]);
                        if (lineTurn == null) lineTurn = testResult;
                        if(testResult != lineTurn || 
                            (convexHullTurn!=null&&convexHullTurn!=lineTurn))
                        {
                            lineTurn = null;
                            break;
                        }
                    }
                    //survived all tests
                    if (lineTurn != null)
                    {
                        if (convexHullTurn == null) { convexHullTurn = lineTurn; }
                        //save point j (line i to j) set start i to be end j and j = 0
                        if (outPoints.Count >0 && outPoints[0].Equals(points[j])) { return; }
                        outPoints.Add(points[j]);
                        i = j;
                        j = -1;
                    }
                    if (i >= 5)
                    {
                        Console.WriteLine();
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
