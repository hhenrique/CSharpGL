using DataSource;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Viewer
{
    public static class Program
    {
        public static void ProcessArea()
        {
            var source = new SurfaceStore();
            source.LoadCSV(@"superficie");

            // a PNG file with all points
            Exporter.SaveAsMonochromePng(source.Points, @"superficie_bw");
            Exporter.SaveAsPng(source.Points, @"superficie");
            Exporter.SaveAsObj(source.Points, @"superficie", "superficie");

            //var enumerator = source.Points.GetEnumerator();
            //enumerator.MoveNext();
            //var p0 = enumerator.Current;
            //while (enumerator.MoveNext())
            //{
            //    var p = enumerator.Current;
            //    Console.WriteLine("({0}, {1}) ({2}, {3}) delta:{4}, {5}",
            //        p0.GridX, p0.GridY, p.GridX, p.GridY,
            //        p.GridX - p0.GridX, p.GridY - p0.GridY);
            //    p0 = p;
            //}
        }
    }
}
