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
            //1,2,3 special case
            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }
            //store points count
            int n = points.Count;
            //first pick point i
            for (int i = 0; i < n; i++)
            {
                //second point j
                for (int j = 0; j < n; j++)
                {
                    //i != j
                    if (i == j) continue;
                    // point i isnt equal point j
                    if (points[i].Equals(points[j])) continue;
                    //line ij and its turn direction start null
                    Line lineIJ = new Line(points[i], points[j]);
                    Enums.TurnType? lineTurn = null;
                    //test with other points k
                    for (int k = 0; k < n; k++)
                    {
                        // k != i or j
                        if (k == i || k == j) continue;
                        //test lineIJ direction with point k
                        Enums.TurnType testResult = HelperMethods.CheckTurn(lineIJ, points[k]);
                        //if its first point to test just store turn type
                        if (testResult == Enums.TurnType.Colinear)
                        {
                            //if k on same lineIJ
                            double distanceIJ = getDistance(points[i], points[j]);
                            double distanceIk = getDistance(points[i], points[k]);
                            double distanceJk = getDistance(points[j], points[k]);
                            //check point k inside line ij then this point passes the test
                            if (distanceIk + distanceJk <= distanceIJ)
                            {
                                continue;
                            }
                            //point outside lineIJ point will not pass the test and break
                        }
                        else
                        {
                            //if its first point to test with store its result direction if not colinear
                            if (lineTurn == null) lineTurn = testResult;
                        }
                        //check new turn with lineIJ direction
                        if (testResult != lineTurn)
                        {
                            //isnt the segment needed just break
                            lineTurn = null;
                            break;
                        }
                    }
                    //survived all tests
                    if (lineTurn != null)
                    {
                        //save point i,j
                        outPoints.Add(points[i]);
                        outPoints.Add(points[j]);
                        break;
                    }
                }
            }
            //remove duplicate points from the list
            outPoints = outPoints.Distinct().ToList();
            //Sometimes list has 2 duplicate and the end of it remove one
            if (outPoints[outPoints.Count - 1].Equals(outPoints[outPoints.Count - 2]))
            {

                outPoints.Remove(outPoints.Last());
            }
        }
        //get distance between 2 points
        private double getDistance(Point i, Point j)
        {
            return Math.Sqrt(Math.Pow(i.Y - j.Y, 2) + Math.Pow(i.X - j.X, 2));
        }
        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}