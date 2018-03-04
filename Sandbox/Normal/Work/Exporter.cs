using DataSource;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Viewer
{
    public static partial class Exporter
    {
        private const string PNG = ".png";
        private const string OBJ = ".obj";
        private const string MTL = ".mtl";

        const int STRIDE = 4;

        public static void SaveAsPng(List<SurfacePoint> points, string filename)
        {
            SaveAsPng(points, filename, false);
        }

        public static void SaveAsMonochromePng(List<SurfacePoint> points, string filename)
        {
            SaveAsPng(points, filename, true);
        }

        private static void SaveAsPng(List<SurfacePoint> points, string filename, bool monochrome)
        {
            var boundingBox = new BoundingBox();
            boundingBox.Set(points);

            Console.WriteLine("normalization. min:{0}|{1} max:{2}|{3}",
                boundingBox.MinZ, boundingBox.NormalizeZ(boundingBox.MinZ),
                boundingBox.MaxZ, boundingBox.NormalizeZ(boundingBox.MaxZ));

            var width = (int)(boundingBox.Width / STRIDE) + 1;
            var height = (int)(boundingBox.Height / STRIDE) + 1;

            using (Bitmap b = new Bitmap(width, height))
            {
                points.ForEach(point =>
                {
                    var x = (int) ((point.GridX - boundingBox.MinX) / STRIDE);
                    var y = (int) ((height - 1) - ((point.GridY - boundingBox.MinY) / STRIDE)); // y coordinate is flipped.
                    
                    var normalized = boundingBox.NormalizeZ(point.Height);
                    if (normalized < 0f || normalized > 1f) { Console.WriteLine("ERROR"); }
                    var colorIndex = (int)Math.Floor(255d * normalized);
                    if (monochrome)
                    {
                        b.SetPixel(x, y, Color.FromArgb(255, colorIndex, colorIndex, colorIndex));
                    }
                    else
                    {
                        var color = ColorPallete.Viridis.Value[colorIndex];
                        b.SetPixel(x, y, Color.FromArgb(colorIndex, color[0], color[1], color[2]));
                    }
                });

                b.Save(filename + PNG, ImageFormat.Png);
            }
        }

        public static void SaveAsObj(List<SurfacePoint> points, string filename, string textureFilename)
        {
            var boundingBox = new BoundingBox();
            boundingBox.Set(points);
            var mesh = GetMesh(points, boundingBox);

            using (StreamWriter writer = new StreamWriter(File.Open(filename + MTL, FileMode.Create)))
            {
                writer.WriteLine("newmtl volume");
                writer.WriteLine("Ns 10.0000");
                writer.WriteLine("Ni 1.5000");
                writer.WriteLine("d 1.0000");
                writer.WriteLine("Tr 0.0000");
                writer.WriteLine("Tf 1.0000 1.0000 1.0000");
                writer.WriteLine("illum 2");
                writer.WriteLine("Ka 0.0000 0.0000 0.0000");
                writer.WriteLine("Kd 0.0 0.0 0.0");
                writer.WriteLine("Ks 0.0000 0.0000 0.0000");
                writer.WriteLine("Ke 0.0000 0.0000 0.0000");
                writer.WriteLine("map_Ka {0}", textureFilename + PNG);
                writer.WriteLine("map_Kd {0}", textureFilename + PNG);
            }

            using (StreamWriter writer = new StreamWriter(File.Open(filename + OBJ, FileMode.Create)))
            {
                writer.WriteLine("o volume");
                writer.WriteLine("mtllib superficie.mtl");
                writer.WriteLine("# vertices (x,y,z)");
                foreach(var t in mesh)
                {
                    writer.WriteLine("v {0} {1} {2}", t.a.x, t.a.y, t.a.z);
                    writer.WriteLine("v {0} {1} {2}", t.b.x, t.b.y, t.b.z);
                    writer.WriteLine("v {0} {1} {2}", t.c.x, t.c.y, t.c.z);
                }
                writer.WriteLine();

                writer.WriteLine("# normals (i,j,k)");
                foreach (var t in mesh)
                {
                    writer.WriteLine("vn {0} {1} {2}", t.a.i, t.a.j, t.a.k);
                    writer.WriteLine("vn {0} {1} {2}", t.b.i, t.b.j, t.b.k);
                    writer.WriteLine("vn {0} {1} {2}", t.c.i, t.c.j, t.c.k);
                }
                writer.WriteLine();

                writer.WriteLine("# texture coordinates (u,v)");
                foreach (var t in mesh)
                {
                    writer.WriteLine("vt {0} {1}", t.a.u, t.a.v);
                    writer.WriteLine("vt {0} {1}", t.b.u, t.b.v);
                    writer.WriteLine("vt {0} {1}", t.c.u, t.c.v);
                }
                writer.WriteLine();

                writer.WriteLine("# faces");
                writer.WriteLine("g volume");
                writer.WriteLine("usemtl volume");
                int index = 1; // indices starts with 1
                foreach (var t in mesh)
                {
                    writer.WriteLine("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}", index, index + 1, index + 2);
                    index += 3;
                }
            }
        }

        #region points to mesh

        class Vertex
        {
            public float x, y, z; // position
            public float u, v;    // texture coordinate
            public float i, j, k; // normal
        }

        class Triange
        {
            public Vertex a, b, c;
        }

        private static List<Triange> GetMesh(List<SurfacePoint> points, BoundingBox boundingBox)
        {
            var mesh = new List<Triange>();

            // builds the list of vertices based on the points grid
            var rows = GetRows(points, boundingBox);

            // builds the list of triangle for each row of vertices
            for (var row = 0; row < rows.Count - 1; row += 1)
            {
                var rowA = rows[row]; var widthRowA = rowA.Count;
                var rowB = rows[row + 1]; var widthRowB = rowB.Count;

                var i = 0;
                var j = 0;
                while (i < widthRowA - 1 && j < widthRowB - 1)
                {
                    var t0 = new Triange
                    {
                        a = Clone(rowA[i]),
                        b = Clone(rowA[i + 1]),
                        c = Clone(rowB[j])
                    };
                    SetNormals(t0);
                    mesh.Add(t0);

                    var t1 = new Triange
                    {
                        a = Clone(rowA[i + 1]),
                        b = Clone(rowB[j + 1]),
                        c = Clone(rowB[j])
                    };
                    SetNormals(t1);
                    mesh.Add(t1);

                    i += 1;
                    j += 1;

                    //if (i == 5 || j == 5) { break; }
                }
                //if (row == 5) { break; }
            }

            return mesh;
        }

        private static List<List<Vertex>> GetRows(List<SurfacePoint> points, BoundingBox boundingBox)
        {
            var rows = new List<List<Vertex>>();

            var lastY = float.MinValue;
            var row = new List<Vertex>();
            foreach (var point in points)
            {
                var vertex = new Vertex
                {
                    // Z is up
                    //x = point.GridX,
                    //y = point.GridY,
                    //z = point.Height,
                    //u = boundingBox.NormalizeX(point.GridX),
                    //v = boundingBox.NormalizeY(point.GridY)

                    // Y is up
                    x = point.GridX,
                    y = point.Height,
                    z = point.GridY,
                    u = boundingBox.NormalizeX(point.GridX),
                    v = boundingBox.NormalizeY(point.GridY)
                };
                if (lastY != point.GridY)
                {
                    lastY = point.GridY;
                    row = new List<Vertex>();
                    rows.Add(row);
                }
                row.Add(vertex);
            }

            return rows;
        }

        private static Vertex Clone(Vertex v)
        {
            return new Vertex
            {
                x = v.x,
                y = v.y,
                z = v.z,
                u = v.u,
                v = v.v,
                i = v.i,
                j = v.j,
                k = v.k
            };
        }

        #endregion

        private static void WriteVertices(StreamWriter writer, List<SurfacePoint> points)
        {
            writer.WriteLine("# vertices (x,y,z)");
            var lastY = float.MinValue;
            var rows = new List<List<SurfacePoint>>();
            var r = new List<SurfacePoint>();
            foreach (var point in points)
            {
                writer.WriteLine("v {0} {1} {2}",
                    point.GridX, point.GridY, point.Height);

                if (lastY != point.GridY)
                {
                    lastY = point.GridY;
                    r = new List<SurfacePoint>();
                    rows.Add(r);
                }
                r.Add(point);
            }
            writer.WriteLine();
        }

        private static void WriteTextureCoordinates(StreamWriter writer, List<SurfacePoint> points, BoundingBox boundingBox)
        {
            writer.WriteLine("# texture coordinates (u,v)");
            foreach (var point in points)
            {
                writer.WriteLine("vt {0} {1}",
                    boundingBox.NormalizeX(point.GridX), boundingBox.NormalizeY(point.GridY));
            }
            writer.WriteLine();
        }

        #region vector math

        private static void SetNormals(Triange t)
        {
            SetNormal(t.a, t.c, t.b);
            SetNormal(t.b, t.a, t.c);
            SetNormal(t.c, t.b, t.a);
            //t.a.i = 1f; t.a.j = 0f; t.a.k = 0f;
            //t.b.i = 0f; t.b.j = -1f; t.b.k = 0f;
            //t.c.i = 0f; t.c.j = -1f; t.c.k = 0f;
        }

        private static void SetNormal(Vertex v0, Vertex v1, Vertex v2)
        {
            var ax = v1.x - v0.x; var ay = v1.y - v0.y; var az = v1.z - v0.z;
            var bx = v2.x - v0.x; var by = v2.y - v0.y; var bz = v2.z - v0.z;

            // cross product
            var i = ay * bz - az * by;
            var j = az * bx - ax * bz;
            var k = ax * by - ay * bx;

            // normalize
            var d = (float)Math.Sqrt(i * i + j * j + k * k);

            if (d == 0f)
            {
                v0.i = 0f;
                v0.j = 1f; // Y is up
                v0.k = 0f;
            }
            else
            {
                v0.i = i / d;
                v0.j = j / d;
                v0.k = k / d;
            }
        }

        private static float[] NormalizedCrossProduct(float ax, float ay, float az, float bx, float by, float bz)
        {
            // cross product
            var x = ay * bz - az * by;
            var y = az * bx - ax * bz;
            var z = ax * by - ay * bx;

            // normal
            var d = (float)Math.Sqrt(x * x + y * y + z * z);

            if (d == 0)
            {
                return new float[] { 1f, 1f, 1f };
            }

            return new float[] { x / d, y / d, z / d };
        }

        #endregion
    }
}
