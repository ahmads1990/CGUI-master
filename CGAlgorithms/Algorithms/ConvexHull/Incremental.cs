using CGUtilities;
using CGUtilities.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public static double CalculateAngels(Point p1, Point p2, Point p3, Point p4)
        {
            double theta1 = Math.Atan2(p2.Y - p1.Y, p1.X - p2.X);
            double theta2 = Math.Atan2(p4.Y - p3.Y, p3.X - p4.X);
            return (theta1 - theta2) * (180 / Math.PI);
        }


        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            // If we have 3 points or less
            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }

            // To store the sorted points by angle
            OrderedSet<Tuple<double, int>> orderedPointsByAngle = new OrderedSet<Tuple<double, int>>();
            // To get the middle point between the first three points
            Point p = new Point((points[0].X + points[1].X) / 2, (points[0].Y + points[1].Y) / 2);
            p.X = (p.X + points[2].X) / 2;
            p.Y = (p.Y + points[2].Y) / 2;

            double k = 1000;
            // The new point
            Point newPoint = new Point(p.X + k, p.Y);
            // The line between the middle point and the new point
            Line baseLine = new Line(p, newPoint);

            for (int i = 0; i < 3; i++)
            {
                //Storing every point in the ordered set
                double angle = CalculateAngels(baseLine.Start, baseLine.End, baseLine.Start, points[i]);
                orderedPointsByAngle.Add(new Tuple<double, int>(angle, i));
            }

            for (int i = 3; i < points.Count; i++)
            {
                // The pre and next of the points 
                KeyValuePair<Tuple<double, int>, Tuple<double, int>> preAndNext = new KeyValuePair<Tuple<double, int>, Tuple<double, int>>();
                double angle = CalculateAngels(baseLine.Start, baseLine.End, p, points[i]);
                preAndNext = orderedPointsByAngle.DirectUpperAndLower(new Tuple<double, int>(angle, i));
                
                // initialize the next of the point
                Tuple<double, int> next = preAndNext.Key;
                next = preAndNext.Key;
                if (next == null)
                {
                    //if it's null so take the first one 
                    next = orderedPointsByAngle.GetFirst();
                }

                Tuple<double, int> pre = preAndNext.Value;
                if (pre == null)
                {
                    //if it's null so take the last one 
                    pre = orderedPointsByAngle.GetLast();
                }


                Line anotherLine = new Line(points[pre.Item2], points[next.Item2]);
                Enums.TurnType checkTurn = HelperMethods.CheckTurn(anotherLine, points[i]);

                if(checkTurn == Enums.TurnType.Right) // outside the polygon
                {
                    // The second pre and next to get the new pre of the point in case we use it 
                    KeyValuePair<Tuple<double, int>, Tuple<double, int>> secondPreAndNext = new KeyValuePair<Tuple<double, int>, Tuple<double, int>>();
                    secondPreAndNext = orderedPointsByAngle.DirectUpperAndLower(pre);
                    Tuple<double, int> newPre = secondPreAndNext.Value;
                
                    if (newPre == null)
                    {
                        newPre = orderedPointsByAngle.GetLast();
                    }
                    anotherLine = new Line(points[i], points[pre.Item2]);
                    checkTurn = HelperMethods.CheckTurn(anotherLine, points[newPre.Item2]);

                    while (checkTurn == Enums.TurnType.Left || checkTurn == Enums.TurnType.Colinear)
                    {
                        // Deleting the old pre and use the new pre
                        orderedPointsByAngle.Remove(pre);
                        pre = newPre;
                        secondPreAndNext = orderedPointsByAngle.DirectUpperAndLower(pre);
                        newPre = secondPreAndNext.Value;
                        if (newPre == null)
                        {
                            newPre = orderedPointsByAngle.GetLast();
                        }
                        anotherLine = new Line(points[i], points[pre.Item2]);
                        checkTurn = HelperMethods.CheckTurn(anotherLine, points[newPre.Item2]);
                    }

                    // The third pre and next to get the new next of the point in case we use it 
                    KeyValuePair<Tuple<double, int>, Tuple<double, int>> thirdPreAndNext = new KeyValuePair<Tuple<double, int>, Tuple<double, int>>();
                    Tuple<double, int> newNext = null;
                    thirdPreAndNext = orderedPointsByAngle.DirectUpperAndLower(next);
                    newNext = thirdPreAndNext.Key;
                    if (newNext == null)
                    {
                        newNext = orderedPointsByAngle.GetFirst();
                    }
                    anotherLine = new Line(points[i], points[next.Item2]);
                    checkTurn = HelperMethods.CheckTurn(anotherLine, points[newNext.Item2]);

                    while (checkTurn == Enums.TurnType.Right || checkTurn == Enums.TurnType.Colinear) 
                    {
                        // Deleting the old next and use the new next
                        orderedPointsByAngle.Remove(next);
                        next = newNext;
                        thirdPreAndNext = orderedPointsByAngle.DirectUpperAndLower(next);
                        newNext = thirdPreAndNext.Key;
                        if (newNext == null)
                        {
                            newNext = orderedPointsByAngle.GetFirst();
                        }
                        anotherLine = new Line(points[i], points[next.Item2]);
                        checkTurn = HelperMethods.CheckTurn(anotherLine, points[newNext.Item2]);
                    }
                    orderedPointsByAngle.Add(new Tuple<double, int>(angle, i));


                }
            }

            
            //Adding the points to the output points 
            for (int i = 0; i < orderedPointsByAngle.Count; i++)
            {
                outPoints.Add(points[orderedPointsByAngle[i].Item2]);
            }

            // Printing the output points
            int d = 1;
            for (int i = 0; i < orderedPointsByAngle.Count; i++)
            {
                Console.Write("Output point " + d + " : (" + outPoints[i].X + ",");
                Console.Write(outPoints[i].Y + ")\n");
                d++;
            }
            

        }
        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}


