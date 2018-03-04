using MathUtils;
using System;

namespace DataSource
{
    public class SurfacePoint : IComparable<SurfacePoint>
    {
        public int EquipmentId { get; set; }
        public int SurveyId { get; set; }
        public DateTime TerrainTime { get; set; }
        public float GridX { get; set; }
        public float GridY { get; set; }
        public float Height { get; set; }
        public float PrimeElevation { get; set; }
        public int Id { get; set; }

        /// <summary>
        /// Compares GridX and GridY ONLY! It ignores Height. It order by Y (rows) and then by X (columns.)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>

        public int CompareTo(SurfacePoint other)
        {
            if (other == null) { return -1; }

            // same X and Y, then it's same point
            if (MathHelper.NearlyEqual(GridX, other.GridX)
                && MathHelper.NearlyEqual(GridY, other.GridY)) { return 0; }

            // Y has priority over X. If this.Y is less than other.Y, then 'this' comes first otherwise 'other' comes first
            if (GridY < other.GridY) { return -1; }
            if (GridY > other.GridY) { return 1; }

            // If Y is same, then the order onlX depends on X
            if (GridX < other.GridX) { return -1; }

            return 1;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                EquipmentId,
                SurveyId,
                TerrainTime,
                GridX,
                GridY,
                Height,
                PrimeElevation,
                Id);
        }
    }
}
