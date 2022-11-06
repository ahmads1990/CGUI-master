using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            //1,2,3 special case
            //more points?
            //first pick point i
            //pick 3 other points than i and make a triangle
            //check if i is in the traingle 
            //t => remove i not extreme point
            //f => check all the other triangles
            //how to check point lie in trangle
            //orientation test
            //triangle jkl vectors jk,kl,lj
            //test each vector with i
            //all same sign inside else outside
            
            //orintation test
            //p q r
            //pq, pr
            //deternmenant
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
