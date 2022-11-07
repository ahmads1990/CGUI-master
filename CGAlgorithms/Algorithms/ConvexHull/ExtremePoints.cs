using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count <= 3)
            {
                outPoints = points;
                return;
            }
            //1,2,3 special case
            //first pick point i
            int n = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                bool result=true;
                //pick 3 other points
                int j, k, l;        
                j = i+1;
                k = j + 1;
                l = k + 1;
                //loop j,k,l
                for (j = 0; j < n; j++)
                {
                    if (j == i || j==k || j==l) { continue; }
                    for ( k = 0; k < n; k++)
                    {
                        if (k == i || k == j || k == l) { continue; }
                        for ( l = 0; l < n; l++)
                        {
                            if (l == i || l == j || l == k) { continue; }
                            Enums.PointInPolygon state = HelperMethods.PointInTriangle(points[i],
                                points[j], points[k], points[l]);
                            //t => remove i not extreme point
                            if (state == Enums.PointInPolygon.Inside
                                || state == Enums.PointInPolygon.OnEdge
                                )
                            {
                                result = false;
                                break;
                            }
                        }
                        if (!result) { break; }
                    }
                    if (!result)
                    {
                        //remove point and continue
                        points.RemoveAt(i);
                        n--;
                        i--;
                        break;
                    }
                }


                //survived test
                if(result)outPoints.Add(points[i]);
            }
            Console.WriteLine();
        }
        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
