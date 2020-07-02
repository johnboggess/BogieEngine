﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using BogieEngineCore;
using BogieEngineCore.Entities;
using BogieEngineCore.Components;
using BogieEngineConsoleTest.Components;
using BogieEngineCore.Shading;

namespace BogieEngineConsoleTest.Entities
{
    class Box : Entity
    {
        public RigidBox RigidBox;

        private bool _reportContacts;
        private Model _model;

        public Box(Entity parent, bool reportContacts, OpenTK.Vector3 position, OpenTK.Vector3 scale, BaseGame game) : base(parent, game)
        {
            LocalTransform.Position = position;
            LocalTransform.Scale = scale;
            _reportContacts = reportContacts;
        }

        public override void EntitySetup()
        {
            RigidBox = new RigidBox(LocalTransform.Position.X, LocalTransform.Position.Y, LocalTransform.Position.Z, LocalTransform.Scale.X, LocalTransform.Scale.Y, LocalTransform.Scale.Z, _reportContacts);
            RigidBox.Name = nameof(RigidBox);
            ForceAddComponent(RigidBox);
            GravityScript gravityScript = new GravityScript();
            ForceAddComponent(gravityScript);

            _model = Model.CreateModel("Resources/Models/Cube.obj", ((Game)BaseGame.GlobalGame).ContentManager, ((Game)BaseGame.GlobalGame).DefaultShader);
            _model.GetMesh(0).Textures.Add(((Game)BaseGame.GlobalGame).CubeTex);
            ForceAddComponent(_model);
        }

        public Shader GetShader(int meshIndex) { return _model.GetMesh(meshIndex).Shader; }
        public void SetShader(Shader shader) { _model.SetShader(shader); }
        public void SetShader(Shader shader, int meshIndex) { _model.GetMesh(meshIndex).Shader = shader; }
    }
}
