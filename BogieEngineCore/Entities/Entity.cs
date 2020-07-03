using Assimp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Components;

namespace BogieEngineCore.Entities
{
    public class Entity
    {
        public BaseGame Game;

        /// <summary>
        /// Set up that can be set for a specific instance of an entity. Occurs after <see cref="EntitySetup"./>
        /// </summary>
        public Delegate InstanceSetup = new Action(() => { });

        public Transform LocalTransform = new Transform();
        public Transform GlobalTransform
        {
            get
            {
                if (Parent == null){ return LocalTransform; }
                Transform result = new Transform();
                result.FromMatrix4(LocalTransform.GetMatrix4() * Parent.GlobalTransform.GetMatrix4());
                return result;
            }
        }

        public Entity Parent
        {
            get { return _parent; }
            set 
            {
                if (value != null)
                    value.QueueAddChild(this);
                else if (_parent != null)
                    _parent.QueueRemoveChild(this);
            }
        }

        public bool Destroyed { get { return _destoryed; } }

        internal bool _FirstTimeSetup = false;

        bool _destoryed = false;
        Entity _parent = null;
        List<Entity> _childern = new List<Entity>();
        Guid _instanceID = Guid.NewGuid();
        List<Component> _components = new List<Component>();

        public Entity(Entity parent, BaseGame game)
        {
            Parent = parent;
            Game = game;
        }

        /// <summary>
        /// Set up that is shared across all instances of an entity. Occurs prior to <see cref="InstanceSetup"./>
        /// </summary>
        public virtual void EntitySetup() { }
        public virtual void ParentChanged() { }

        /// <summary>
        /// Queue the entity to be moved. Should not be used during entity setup or at the beginning of the update loop.
        /// </summary>
        /// <param name="child"></param>
        public void QueueAddChild(Entity child)
        {
            Game._EntitiesToMove.TryAdd(child, this);
        }

        /// <summary>
        /// Force the entity to move immediately. Should only be used during entity setup, or at the beginning of the update loop.
        /// </summary>
        /// <param name="child"></param>
        public void ForceAddChild(Entity child)
        {
            if (child.Parent != null)
                child.Parent.ForceRemoveChild(child);
            child._parent = this;
            _childern.Add(child);
        }

        /// <summary>
        /// Queue the entity to be removed. Should not be used during entity setup or at the beginning of the update loop.
        /// </summary>
        /// <param name="child"></param>
        public void QueueRemoveChild(Entity child)
        {
            Game._EntitiesToMove.TryAdd(child, null);
        }

        /// <summary>
        /// Force the entity to be removed immediately. Should only be used during entity setup, or at the beginning of the update loop.
        /// </summary>
        /// <param name="child"></param>
        public void ForceRemoveChild(Entity child)
        {
            child._parent = null;
            _childern.Remove(child);
        }

        public Component GetComponent(string Name)
        {
            return _components.Where(c => c.Name == Name).First();
        }

        public T GetComponet<T>(string Name) where T : Component
        {
            return (T)GetComponent(Name);
        }

        public List<T> GetComponents<T>() where T : Component
        {
            return _components.Where(c => c is T).Cast<T>().ToList();
        }

        /// <summary>
        /// Queue the component to be moved. Should not be used during entity setup or at the beginning of the update loop.
        /// </summary>
        /// <param name="component">The component to move.</param>
        public void QueueAddComponent(Component component)
        {
            Game._ComponentsToMove.TryAdd(component, this);
        }

        /// <summary>
        /// Queue the component to be removed. Should not be used during entity setup or at the beginning of the update loop.
        /// </summary>
        /// <param name="component">The component to removed.</param>
        public void QueueRemoveComponent(Component component)
        {
            Game._ComponentsToMove.TryAdd(component, null);
        }

        /// <summary>
        /// Queue the component to be removed. Should not be used during entity setup or at the beginning of the update loop.
        /// </summary>
        /// <param name="Name">The name of the componenet.</param>
        public void QueueRemoveComponent(string Name)
        {
            QueueRemoveComponent(_components.Where(c => c.Name == Name).First());
        }

        /// <summary>
        /// Force the component to attach immediately. Should only be used during entity setup, or at the beginning of the update loop.
        /// </summary>
        /// <param name="component"></param>
        public void ForceAddComponent(Component component)
        {
            component.ForceDetachFromEntity();
            if (_components.Any(c => c.Name == component.Name))
                throw new Exception("Component with name " + component.Name + "already exists");
            _components.Add(component);
            component._Entity = this;
        }

        /// <summary>
        /// Force the component to be removed immediately. Should only be used during entity setup, or at the beginning of the update loop.
        /// </summary>
        /// <param name="component"></param>
        public void ForceRemoveComponent(Component component)
        {
            component._Entity = null;
            _components.Remove(component);
        }

        /// <summary>
        /// Queue the entity to be destroyed at the beginning of the next update loop. Should not be used during entity setup or at the beginning of the update loop.
        /// Notifies all components and childern
        /// </summary>
        public void QueueDestroy()
        {
            if (Parent != null)
                Parent.QueueRemoveChild(this);

            _components.ForEach(c => c.QueueDestroy());
            _childern.ForEach(e => e.QueueDestroy());

            _destoryed = true;
        }

        /// <summary>
        /// Force the entity to be destroyed immediately. Should only be used during entity setup, or at the beginning of the update loop.
        /// Notifies all components and childern
        /// </summary>
        public void ForceDestroy()
        {
            if (Parent != null)
                Parent.ForceAddChild(this);

            _components.ForEach(c => c.ForceDestroy());
            _childern.ForEach(e => e.ForceDestroy());

            _destoryed = true;
        }

        public void InvokeEvent(string evnt, bool invokeChildern, params object[] eventArgs)
        {
            if (Destroyed)
                return;

            _components.ForEach(c => c._EventInvoked(evnt, eventArgs));
            if (invokeChildern)
                _childern.ForEach(e => e.InvokeEvent(evnt, invokeChildern, eventArgs));
        }
    }
}
