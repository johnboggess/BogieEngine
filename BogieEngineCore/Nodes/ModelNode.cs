using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Modelling;

using OpenTK;
namespace BogieEngineCore.Nodes
{
    /// <summary>
    /// Node representing a model.
    /// </summary>
    public class ModelNode : Node
    {
        /// <summary>
        /// The model of the node.
        /// </summary>
        public Model Model;

        /// <summary>
        /// Creates a model node.
        /// </summary>
        /// <param name="game">The game the node is apart of.</param>
        /// <param name="model">The model the node represents.</param>
        public ModelNode(BaseGame game, Model model) : base(game)
        {
            Model = model;
        }

        public override void Draw(float deltaT, Transform parentWorldTransform)
        {
            Model.Draw(LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4());
        }

        /// <summary>
        /// Find meshes with the given name.
        /// </summary>
        /// <param name="name">The name of the meshes to search for.</param>
        /// <returns>All the meshes matching the given name.</returns>
        public List<MeshData> GetMeshWithName(string name)
        {
            return Model.GetMeshWithName(name);
        }
    }
}
