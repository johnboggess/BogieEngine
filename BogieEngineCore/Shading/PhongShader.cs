﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Lighting;

using OpenTK;
using OpenTK.Graphics.ES10;

namespace BogieEngineCore.Shading
{
    public class PhongShader : Shader
    {
        public PhongShader(BaseGame game) : base(game, "Resources/Shaders/default.vert", "Resources/Shaders/PhongLight.frag") { }

        public BasicLight BasicLight = new BasicLight()
        {
            AmbientColor = new Vector3(.3f, .3f, .3f),
            DiffuseColor = new Vector3(.5f, .5f, .5f),
            SpecularColor = new Vector3(.5f, .5f, .5f),
        };

        public Matrix4 Projection
        {
            set 
            {
                SetUniformMatrix(nameof(Projection), false, value);
            }
        }

        public Matrix4 View
        {
            set
            {
                SetUniformMatrix(nameof(View), false, value);
            }
        }

        public Matrix4 Model
        {
            set
            {
                SetUniformMatrix(nameof(Model), false, value);
            }
        }

        public Vector3 ViewPosition
        {
            set
            {
                SetUniform3(nameof(ViewPosition), value);
            }
        }

        public override void Use(params object[] values)
        {
            BasicLight.SetLightUniform("light", this);
            Model = (Matrix4)values[0];
        }
    }
}