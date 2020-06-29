using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogieEngineCore.Physics.Shapes
{
    public abstract class Shape { }

    public class Box : Shape
    {
        public float Width_X = -1;
        public float Height_Y = -1;
        public float Length_Z = -1;

        public Box() { }
        public Box(float widthX, float heightY, float lengthZ)
        {
            Width_X = widthX;
            Height_Y = heightY;
            Length_Z = lengthZ;
        }
    }

    public class Cylinder : Shape
    {
        public float Radius;
        public float Length;

        public Cylinder(float radius, float length)
        {
            Radius = radius;
            Length = length;
        }
    }
}
