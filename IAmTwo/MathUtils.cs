using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmTwo
{
    class MathUtils
    {
        public static float Lerp(float v0, float v1, float t)
        {
            return v0 + t * (v1 - v0);
        }
    }
}
