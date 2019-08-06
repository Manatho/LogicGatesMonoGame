using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicGateFront.PointExtensions
{
    public static class PointExtensions
    {
        public static Vector2 Rotate(this Vector2 point, Vector2 center, float angle)
        {
            float s = (float)Math.Sin(angle);
            float c = (float)Math.Cos(angle);
            point -= center;

            float x = point.X * c - point.Y * s;
            float y = point.X * s + point.Y * c;

            return new Vector2(x,y) + center;
        }
    }
}
