﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
namespace BogieEngineCore
{
    struct Vertex
    {
        public static readonly int Size = 5 * sizeof(float);

        internal Vector3 _Position;
        internal Vector2 _UV;

        public Vertex(Vector3 position, Vector2 uv)
        {
            _Position = position;
            _UV = uv;
        }
    }
}
