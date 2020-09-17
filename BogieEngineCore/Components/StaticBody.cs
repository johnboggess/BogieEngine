using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using BogieEngineCore;
using BogieEngineCore.Entities;

using BepuPhysics;
using BepuPhysics.Collidables;
namespace BogieEngineCore.Components
{
    public class StaticBody : PhysicsObject
    {
        public StaticBody(float x, float y, float z, float scaleX, float scaleY, float scaleZ, string name)
        {
            Name = name;

            _SetupCollider(x, y, z, scaleX, scaleY, scaleZ);
            registerPhysicsObject();
        }

        public StaticBody(Vector3 position, Vector3 scale, string name) : this(position.X, position.Y, position.Z, scale.X, scale.Y, scale.Z, name)
        {
        }

        public StaticBody(Transform transform, string name) : this(transform.Position.X, transform.Position.Y, transform.Position.Z, transform.Scale.X, transform.Scale.Y, transform.Scale.Z, name)
        {
        }

        public override void EventInvoked(string evnt, params object[] eventArgs)
        {
            if (Destroyed)
                return;
            else if (evnt == Component.DestroyEvent)
            {
                _BodyReference.Bodies.Remove(_BodyReference.Handle);
                unregisterPhysicsObject();
            }
        }


        protected virtual void _SetupCollider(float x, float y, float z, float scaleX, float scaleY, float scaleZ) { throw new NotImplementedException(); }

        /*public static StaticBody CreateStaticBody(Entity entity, Physics.Shapes.Shape shapeInfo, bool queue, string name = nameof(StaticBody))
        {
            StaticBody staticBody = new StaticBody();
            staticBody.Name = name;
            if(queue)
                staticBody.QueueAttachToEntity(entity);
            else
                staticBody.ForceAttachToEntity(entity);

            if (shapeInfo is Physics.Shapes.Box)
                staticBody._createBox((Physics.Shapes.Box)shapeInfo);

            staticBody.registerPhysicsObject();

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
            
            _BodyHandle = _Entity.Game._PhysicsSimulation.Statics.Add(staticDescription);
            _IsBody = false;
        }*/
    }
}
