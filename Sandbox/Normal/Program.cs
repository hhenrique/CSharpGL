using DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Viewer;

namespace Normal
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var source = new SurfaceStore();
            source.LoadCSV(@"superficie");

            Exporter.SaveAsPng(source.Points, @"superficie");
            Exporter.SaveAsObj(source.Points, @"superficie", "superficie");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
