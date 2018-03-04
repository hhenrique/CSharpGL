using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DataSource
{
    public class SurfaceStore
    {
        const string BIN = ".bin";
        const string CSV = ".csv";
        const string RPT = ".rpt";

        private readonly List<SurfacePoint> _points = new List<SurfacePoint>();

        public List<SurfacePoint> Points => _points;

        public SurfaceStore()
        {
        }

        #region RPT file

        public void LoadRpt(string filename)
        {
            if (!File.Exists(filename + RPT)) { throw new ArgumentNullException(filename + RPT); }

            Points.Clear();

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            var pointsIgnored = 0;
            using (var file = new StreamReader(filename + RPT))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    line = regex.Replace(line, " ");
                    var point = ProcessLine(line);
                    if (point == null)
                    {
                        Console.WriteLine(line);
                        pointsIgnored += 1;
                    }
                    else
                    {
                        Points.Add(point);
                    }
                }
            }

            if (pointsIgnored > 0)
            {
                Console.WriteLine("Ignored:{0}", pointsIgnored);
            }
        }

        private SurfacePoint ProcessLine(string line)
        {
            var tokens = line.Split(' ');
            if (tokens.Length == 9)
            {
                int equipmentId, id;
                float gridX, gridY, height, primeElevation;
                DateTime terrainTime;
                if (Int32.TryParse(tokens[0], out equipmentId)
                    && DateTime.TryParse(tokens[2] + " " + tokens[3], out terrainTime)
                    && float.TryParse(tokens[4], out gridX)
                    && float.TryParse(tokens[5], out gridY)
                    && float.TryParse(tokens[6], out height)
                    && float.TryParse(tokens[7], out primeElevation)
                    && Int32.TryParse(tokens[8], out id))
                {
                    if (gridX >= 143000 && gridX <= 220500
                        && gridY >= 70000 && gridY <= 110000)
                    {
                        return new SurfacePoint
                        {
                            EquipmentId = equipmentId,
                            GridX = gridX,
                            GridY = gridY,
                            Height = height,
                            Id = id,
                            PrimeElevation = primeElevation,
                            TerrainTime = terrainTime
                        };
                    }
                }
            }

            return null;
        }

        #endregion

        #region CSV

        public void SaveCSV(string filename)
        {
            using (StreamWriter writer = new StreamWriter(File.Open(filename + CSV, FileMode.Create)))
            {
                writer.WriteLine("EquipmentId,SurveyId,TerrainTime,GridX,GridY,Height,PrimeElevation,Id");
                foreach (var point in Points)
                {
                    writer.WriteLine(point.ToString());
                }
            }
        }

        public void LoadCSV(string filename)
        {
            if (!File.Exists(filename + CSV)) { throw new ArgumentNullException(filename + CSV); }

            Points.Clear();

            float minH = float.MaxValue;
            float maxH = float.MinValue;

            var pointsIgnored = 0;
            using (var file = new StreamReader(filename + CSV))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var tokens = line.Split(',');
                    if (tokens.Length == 3)
                    {
                        float x, y, z;

                        if (float.TryParse(tokens[0], out x)
                            && float.TryParse(tokens[1], out y)
                            && float.TryParse(tokens[2], out z)
                            /*&& (height >= 2400f && height <= 3100f)*/)
                        {
                            if (z < minH) { minH = z; }
                            if (z > maxH) { maxH = z; }

                            var point = new SurfacePoint
                            {
                                GridX = x,
                                GridY = y,
                                Height = z
                            };
                            Points.Add(point);
                        }

                    }
                    else
                    {
                        Console.WriteLine(line);
                        pointsIgnored += 1;
                    }
                }
            }

            if (pointsIgnored > 0)
            {
                Console.WriteLine("Ignored:{0}", pointsIgnored);
            }

            Console.WriteLine("minH:{0} maxH:{1}", minH, maxH);

            Points.Sort();
        }

        #endregion

        #region Binary

        public void SaveBinary(string filename)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                writer.Write((Int32)Points.Count);

                foreach (var point in Points)
                {
                    writer.Write((Int32)point.EquipmentId);
                    writer.Write((double)point.GridX);
                    writer.Write((double)point.GridY);
                    writer.Write((double)point.Height);
                    writer.Write((Int32)point.Id);
                    writer.Write((double)point.PrimeElevation);
                    writer.Write((Int32)point.SurveyId);
                    writer.Write((long)point.TerrainTime.Ticks);
                }
            }
        }

        public void LoadBinary(string filename)
        {
            if (!File.Exists(filename)) { throw new ArgumentNullException(filename); }

            Points.Clear();

            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                int pointsCount = reader.ReadInt32();

                for (int i = 0; i < pointsCount; i += 1)
                {
                    var point = new SurfacePoint
                    {
                        EquipmentId = reader.ReadInt32(),
                        GridX = (float)reader.ReadDouble(),
                        GridY = (float)reader.ReadDouble(),
                        Height = (float)reader.ReadDouble(),
                        Id = reader.ReadInt32(),
                        PrimeElevation = (float)reader.ReadDouble(),
                        SurveyId = reader.ReadInt32(),
                        TerrainTime = new DateTime(reader.ReadInt64())
                    };
                    Points.Add(point);
                }
            }
        }

        #endregion
    }
}
