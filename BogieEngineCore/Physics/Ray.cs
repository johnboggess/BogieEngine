using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Components;

using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Trees;

namespace BogieEngineCore.Physics
{
    public class Ray
    {
        public Vector3 Position;
        public Vector3 Direction;

        public Ray(in Vector3 position, in Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }

        public PhysicsObject Cast(List<PhysicsObject> ignore, out float distance)
        {
            RayHitHandler rayHitHandler = new RayHitHandler(ignore);
            BaseGame.GlobalGame._PhysicsSimulation.RayCast<RayHitHandler>(Position, Direction, float.PositiveInfinity, ref rayHitHandler);
            distance = -1;
            if (rayHitHandler.ResultFound)
            {
                distance = rayHitHandler.Distance;
                return BaseGame.GlobalGame._GamePhysics.GetPhysicsObject(rayHitHandler.Result);
            }
            return null;
        }

        public PhysicsObject Cast(out float distance)
        {
            return Cast(new List<PhysicsObject>(), out distance);
        }

        private class RayHitHandler : IRayHitHandler
        {
            public CollidableReference Result;
            public float Distance = -1;
            public bool ResultFound = false;

            List<PhysicsObject> _ignore;

            public RayHitHandler(List<PhysicsObject> ignore)
            {
                _ignore = ignore;
            }

            bool IRayHitHandler.AllowTest(CollidableReference collidable)
            {
                foreach(PhysicsObject po in _ignore)
                {
                    if((collidable.Mobility == CollidableMobility.Dynamic || collidable.Mobility == CollidableMobility.Kinematic ) && po._IsBody)
                        if(collidable.Handle == po._BodyHandle)
                            return false;
                    if (collidable.Mobility == CollidableMobility.Static && !po._IsBody)
                        if (collidable.Handle == po._BodyHandle)
                            return false;
                }
                return true;
            }

            bool IRayHitHandler.AllowTest(CollidableReference collidable, int childIndex)
            {
                foreach (PhysicsObject po in _ignore)
                {
                    if ((collidable.Mobility == CollidableMobility.Dynamic || collidable.Mobility == CollidableMobility.Kinematic) && po._IsBody)
                        if (collidable.Handle == po._BodyHandle)
                            return false;
                    if (collidable.Mobility == CollidableMobility.Static && !po._IsBody)
                        if (collidable.Handle == po._BodyHandle)
                            return false;
                }
                return true;
            }

            void IRayHitHandler.OnRayHit(in RayData ray, ref float maximumT, float t, in Vector3 normal, CollidableReference collidable, int childIndex)
            {
                if (t < maximumT)
                    maximumT = t;
                Result = collidable;
                ResultFound = true;
                Distance = t;
            }
        }
    }
}
