using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using BepuPhysics;
using BepuPhysics.Collidables;

namespace BogieEngineCore.Components
{
    public class StaticBox : StaticBody
    {
        public StaticBox(float x, float y, float z, float scaleX, float scaleY, float scaleZ) : base(x, y, z, scaleX, scaleY, scaleZ, nameof(StaticBox))
        {
        }
        public StaticBox(Vector3 position, Vector3 scale) : base(position, scale, nameof(StaticBox))
        {
        }
        public StaticBox(Transform transform) : base(transform, nameof(StaticBox))
        {
        }

        protected override void _SetupCollider(float x, float y, float z, float scaleX, float scaleY, float scaleZ)
        {
            Box box = new Box(scaleX, scaleY, scaleZ);

            StaticDescription staticDescription = new StaticDescription(
                new System.Numerics.Vector3(x, y, z),
                new CollidableDescription(BaseGame.GlobalGame._PhysicsSimulation.Shapes.Add(box), 0.1f));

            _BodyHandle = BaseGame.GlobalGame._PhysicsSimulation.Statics.Add(staticDescription);
            _IsBody = false;
        }
    }
}
