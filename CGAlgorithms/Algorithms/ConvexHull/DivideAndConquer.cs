using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
         public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
			points = points.OrderBy(x => x.Y).ToList();
            points = points.OrderBy(x => x.X).ToList();

			List<Point> solution = getHull(points);
			outPoints = new List<Point>();
			outPoints= solution;
		}

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }
		public List<Point> getHull(List<Point> pointList)
		{
			if (pointList.Count == 1)
			{
				return pointList;
			}
			
			List<Point> leftpoints = new List<Point>();
			List<Point> rightpoints = new List<Point>();
			for (int i = 0; i < pointList.Count/2 ; i++)
				leftpoints.Add(pointList[i]);
			for (int i = pointList.Count/2 ; i < pointList.Count; i++)
				rightpoints.Add(pointList[i]);
			List<Point> leftHull = getHull(leftpoints);
			List<Point> rightHull = getHull(rightpoints);
			return merge(leftHull, rightHull);
		}
		public List<Point> merge(List<Point> left, List<Point> right)
		{
			List<Point> points = new List<Point>();
			int rightMost = 0;
			int leftMost = 0;
			for(int i = 1; i < left.Count; i++)
            {
                if (left[i].X > left[rightMost].X)
                {
					rightMost = i;
                }
				else if(left[i].X == left[rightMost].X && left[i].Y > left[rightMost].Y)
                {
					rightMost=i;
                }
            }
			for (int i = 1; i < right.Count; i++)
			{
				if (right[i].X < right[leftMost].X)
				{
					leftMost = i;
				}
				else if (right[i].X == right[leftMost].X && right[i].Y < right[leftMost].Y)
				{
					leftMost = i;
				}
			}
            
            int ULP = rightMost;
            int URP = leftMost;
            int DLP = rightMost;
            int DRP = leftMost;

            bool Changed = false;
            Line line;
            do
            {
                Changed = true;

                line = new Line(right[URP], left[ULP]);
                while (HelperMethods.CheckTurn(line, left[(ULP + 1) % left.Count]) == Enums.TurnType.Right)
                {
                    ULP = (ULP + 1) % left.Count;
                    Changed = false;
                    line = new Line(right[URP], left[ULP]);
                }

                line = new Line(right[URP], left[ULP]);
                if (Changed && HelperMethods.CheckTurn(line, left[(ULP + 1) % left.Count]) == Enums.TurnType.Colinear)
                {
                    ULP = (ULP + 1) % left.Count;
                }
                

                line = new Line(left[ULP], right[URP]);
                while (HelperMethods.CheckTurn(line, right[(right.Count + URP - 1) % right.Count]) == Enums.TurnType.Left)
                {
                    URP = (right.Count + URP - 1) % right.Count;
                    Changed = false;
                    line = new Line(left[ULP], right[URP]);
                }

                line = new Line(left[ULP], right[URP]);
                if (Changed && HelperMethods.CheckTurn(line, right[(right.Count + URP - 1) % right.Count]) == Enums.TurnType.Colinear)
                {
                    URP = (URP + right.Count - 1) % right.Count;
                }

            } while (!Changed);

            
            Changed = false;
            do
            {
                Changed = true;
                line = new Line(right[DRP], left[DLP]);

                while (HelperMethods.CheckTurn(line, left[(DLP + left.Count - 1) % left.Count]) == Enums.TurnType.Left)
                {
                    DLP = (DLP + left.Count - 1) % left.Count;
                    Changed = false;
                    line = new Line(right[DRP], left[DLP]);
                }

                line = new Line(right[DRP], left[DLP]);
                if (Changed && HelperMethods.CheckTurn(line, left[(DLP + left.Count - 1) % left.Count]) == Enums.TurnType.Colinear)
                {
                    DLP = (DLP + left.Count - 1) % left.Count;
                }

                line = new Line(left[DLP], right[DRP]);
                while (HelperMethods.CheckTurn(line, right[(DRP + 1) % right.Count]) == Enums.TurnType.Right)
                {
                    DRP = (DRP + 1) % right.Count;
                    Changed = false;
                    line = new Line(left[DLP], right[DRP]);
                }

                line = new Line(left[DLP], right[DRP]);
                if (Changed && HelperMethods.CheckTurn(line, right[(DRP + 1) % right.Count]) == Enums.TurnType.Colinear)
                {
                    DRP = (DRP + 1) % right.Count;
                }


            } while (!Changed);


            int x = ULP;

			points.Add(left[ULP]);
			while (x != DLP)
			{
				x = (x + 1) % left.Count;
				points.Add(left[x]);
			}

			x = DRP;
			if (!points.Contains(right[DRP]))
			{
				points.Add(right[DRP]);
			}

			while (x != URP)
			{
				x = (x + 1) % right.Count;
				if (!points.Contains(right[x]))
				{
					points.Add(right[x]);
				}

			}


			return points;
		}
	}
	
}

