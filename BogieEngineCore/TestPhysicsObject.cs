using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Nodes;
namespace BogieEngineCore
{
    class TestPhysicsObject : RigidBody
    {
        public Modelling.Model Model;

        public TestPhysicsObject(BaseGame game, Modelling.Model model) : base(game)
        {
            Model = model;
        }

        public override void Draw(float deltaT, Transform parentWorldTransform)
        {
            Model.Draw(LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4());
        }
    }
}
