using DataSource;
using System;
using System.Collections.Generic;

namespace Viewer
{
    public struct BoundingBox
    {
        public float MinX, MaxX, MinY, MaxY, MinZ, MaxZ;

        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Depth { get; private set; }

        public void Set(List<SurfacePoint> points)
        {
            float minX, maxX, minY, maxY, minZ, maxZ;

            minX = minY = minZ = float.MaxValue;
            maxX = maxY = maxZ = float.MinValue;

            points.ForEach(point =>
            {
                minX = Math.Min(minX, point.GridX);
                minY = Math.Min(minY, point.GridY);
                minZ = Math.Min(minZ, point.Height);
                maxX = Math.Max(maxX, point.GridX);
                maxY = Math.Max(maxY, point.GridY);
                maxZ = Math.Max(maxZ, point.Height);
            });

            MinX = minX; MaxX = maxX;
            MinY = minY; MaxY = maxY;
            MinZ = minZ; MaxZ = maxZ;

            Width = MaxX - MinX + 1f;
            Height = MaxY - MinY + 1f;
            Depth = MaxZ - MinZ + 1f;
        }

        public float NormalizeX(float x)
        {
            return (x - MinX) / Width;
        }

        public float NormalizeY(float y)
        {
            return (y - MinY) / Height;
        }

        public float NormalizeZ(float z)
        {
            return (z - MinZ) / Depth;
        }
    }
}
