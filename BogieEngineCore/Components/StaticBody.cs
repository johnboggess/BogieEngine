using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore;
using BogieEngineCore.Entities;

using BepuPhysics;
using BepuPhysics.Collidables;
namespace BogieEngineCore.Components
{
    public class StaticBody : Component
    {
        private int _bodyHandle;

        public static StaticBody CreateStaticBody(Entity entity, Physics.Shapes.Shape shapeInfo, bool queue, string name = nameof(RigidBody))
        {
            StaticBody staticBody = new StaticBody();
            staticBody.Name = name;
            if(queue)
                staticBody.QueueAttachToEntity(entity);
            else
                staticBody.ForceAttachToEntity(entity);

            if (shapeInfo is Physics.Shapes.Box)
                staticBody._createBox((Physics.Shapes.Box)shapeInfo);

            return staticBody;
        }

        private void _createBox(Physics.Shapes.Box boxInfo)
        {
            if (boxInfo.Height_Y < 0)
                boxInfo = new Physics.Shapes.Box(_Entity.LocalTransform.Scale.X, _Entity.LocalTransform.Scale.Y, _Entity.LocalTransform.Scale.Z);
            Box box = new Box(boxInfo.Width_X, boxInfo.Height_Y, boxInfo.Length_Z);

            StaticDescription staticDescription = new StaticDescription(
                new System.Numerics.Vector3(_Entity.GlobalTransform.Position.X, _Entity.GlobalTransform.Position.Y, _Entity.GlobalTransform.Position.Z),
                new CollidableDescription(_Entity.Game._PhysicsSimulation.Shapes.Add(box), 0.1f));

            _bodyHandle = _Entity.Game._PhysicsSimulation.Statics.Add(staticDescription);
        }
    }
}
