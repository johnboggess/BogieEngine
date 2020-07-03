using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Entities;

namespace BogieEngineCore.Components
{
    public abstract class Component
    {
        public string Name = nameof(Component);
        public Entity Entity { get { return _Entity; } }
        public bool Destroyed { get { return _destoryed; } }

        internal Entity _Entity = null;

        bool _destoryed = false;

        public virtual void EventInvoked(string evnt, params object[] eventArgs) { }

        /// <summary>
        /// Queue the component to be attached at the beginning of the next update loop. Should not be used during entity setup or at the beginning of the update loop.
        /// </summary>
        /// <param name="entity">The entity to attach this component to.</param>
        public void QueueAttachToEntity(Entity entity)
        {
            QueueDetachFromEntity();
            entity.QueueAddComponent(this);
        }

        /// <summary>
        /// Queue the component to be detached at the beginning of the next update loop. Should not be used during entity setup or at the beginning of the update loop.
        /// </summary>
        public void QueueDetachFromEntity()
        {
            if (_Entity != null)
                _Entity.QueueRemoveComponent(this);
        }

        /// <summary>
        /// Force the component to attach immediately. Should only be used during entity setup, or at the beginning of the update loop.
        /// </summary>
        /// <param name="entity"></param>
        public void ForceAttachToEntity(Entity entity)
        {
            ForceDetachFromEntity();
            entity.ForceAddComponent(this);
        }

        /// <summary>
        /// Force the component to be removed immediately. Should only be used during entity setup, or at the beginning of the update loop.
        /// </summary>
        public void ForceDetachFromEntity()
        {
            if (_Entity != null)
                _Entity.ForceRemoveComponent(this);
        }

        /// <summary>
        /// Queue the component to be destroyed at the beginning of the next update loop. Should not be used during entity setup or at the beginning of the update loop.
        /// </summary>
        public void QueueDestroy()
        {
            QueueDetachFromEntity();
            _EventInvoked(Component.DestroyEvent, null);
            _destoryed = true;
        }

        /// <summary>
        /// Force the component to be destroyed immediately. Should only be used during entity setup, or at the beginning of the update loop.
        /// </summary>
        public void ForceDestroy()
        {
            ForceDetachFromEntity();
            _EventInvoked(Component.DestroyEvent, null);
            _destoryed = true;
        }

        internal void _EventInvoked(string evnt, params object[] eventArgs)
        {
            if (!Destroyed)
                EventInvoked(evnt, eventArgs);
        }

        public static readonly string RenderEvent = "Render";
        public static readonly string UpdateEvent = "Update";
        public static readonly string SetupEvent = "Setup";
        public static readonly string DestroyEvent = "Destroy";
    }
}
