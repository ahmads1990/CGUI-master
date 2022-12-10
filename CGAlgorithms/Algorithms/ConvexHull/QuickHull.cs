using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            Point minimumX = points[0];
            Point maximumX = points[0];
            //find min and max (X) in points
            foreach(Point point in points)
            {
                if(minimumX.X > point.X)
                {
                    minimumX = point;
                }
                if(maximumX.X < point.X)
                {
                    maximumX = point;
                }
            }
            Line line = new Line(minimumX, maximumX);
            Line reverse_line = new Line(maximumX, minimumX);
            List<Point> points_left_line = new List<Point>();
            List<Point> points_right_line = new List<Point>();
            //check points (in left or right) of line 
            foreach (Point p in points)
            {
                Enums.TurnType type = HelperMethods.CheckTurn(line, p);
                if (type == Enums.TurnType.Left)
                {
                    points_left_line.Add(p);
                }
                if (type == Enums.TurnType.Right)
                {
                    points_right_line.Add(p);
                }
            }
            List<Point> extreme_Left = QuickHullAlgorithm(points_left_line, line);
            List<Point> extreme_Right = QuickHullAlgorithm(points_right_line, reverse_line);
            extreme_Left.Add(minimumX);
            extreme_Left.Add(maximumX);
            extreme_Left.AddRange(extreme_Right);
            List<Point> result = new List<Point>();
            foreach (Point p in extreme_Left)
            {
                if (!result.Contains(p))
                {
                    result.Add(p);
                }
            }
            outPoints = result;

        }
        public List<Point> QuickHullAlgorithm(List<Point> points, Line line)
        {
            if (points.Count == 0)
            {
                return new List<Point>();
            }
            Point max_point_in_distance = MaxPoint(points, line);
            Line line1 = new Line(line.Start, max_point_in_distance);
            List<Point> left_points1 = LeftPointsFromLine(points, line1);
            List<Point> R1 = QuickHullAlgorithm(left_points1, line1);


            Line line2 = new Line(max_point_in_distance, line.End);
            List<Point> left_points2 = LeftPointsFromLine(points, line2);
            List<Point> R2 = QuickHullAlgorithm(left_points2, line2);

            R1.Add(max_point_in_distance);
            R1.AddRange(R2);
            List<Point> result = new List<Point>();
            foreach (Point p in R1)
            {
                if (!result.Contains(p))
                {

                    result.Add(p);

                }
            }
            return result;
        }
        public Point MaxPoint(List<Point> points, Line line)
        {
            Point point_with_max_distance = points[0];
            double max_distance = -10000000.0;
            double x1 = line.Start.X;
            double y1 = line.Start.Y;
            double x2 = line.End.X;
            double y2 = line.End.Y;
            double x0, y0, line_defined_by_two_points;
            foreach (Point p in points)
            {
                
                x0 = p.X;
                y0 = p.Y;
                double xx = x2 - x1, yy = y2 - y1;
                line_defined_by_two_points = Math.Abs(((x2 - x1) * (y1 - y0)) - ((x1 - x0) * (y2 - y1))) / Math.Sqrt((xx * xx) + (yy * yy));

                if (line_defined_by_two_points > max_distance)
                {
                    max_distance = line_defined_by_two_points;
                    point_with_max_distance = p;
                }
            }
            return point_with_max_distance;
        }

        public List<Point> LeftPointsFromLine(List<Point> points, Line line)
        {
            List<Point> left_points = new List<Point>();

            foreach (Point p in points)
            {

                Enums.TurnType type = HelperMethods.CheckTurn(line, p);
                if (type == Enums.TurnType.Left)
                {
                    left_points.Add(p);
                }


            }
            return left_points;
        }



        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
