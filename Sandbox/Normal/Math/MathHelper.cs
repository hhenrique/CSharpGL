using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathUtils
{
    public static class MathHelper
    {
        private const float FLOAT_MIN_NORMAL = 1.17549435E-38f;

        public static bool NearlyEqual(float a, float b)
        {
            float absA = (float)Math.Abs(a);
            float absB = (float)Math.Abs(b);
            float diff = (float)Math.Abs(a - b);

            if (a == b)
            { // shortcut, handles infinities
                return true;
            }
            else if (a == 0f || b == 0f || diff < FLOAT_MIN_NORMAL)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < (float.Epsilon * FLOAT_MIN_NORMAL);
            }
            else
            { // use relative error
                return diff / (absA + absB) < float.Epsilon;
            }
        }

        public static bool NearlyEqual(double a, double b, double epsilon)
        {
            double absA = Math.Abs(a);
            double absB = Math.Abs(b);
            double diff = Math.Abs(a - b);

            if (a == b)
            { // shortcut, handles infinities
                return true;
            }
            else if (a == 0d || b == 0d || diff < Double.Epsilon)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < epsilon;
            }
            else
            { // use relative error
                return diff / (absA + absB) < epsilon;
            }
        }
    }
}
