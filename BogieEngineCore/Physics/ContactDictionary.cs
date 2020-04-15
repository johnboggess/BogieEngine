using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogieEngineCore.Physics
{
    class ContactDictionary
    {
        ConcurrentDictionary<int, ConcurrentDictionary<int, Contact>> _contacts = new ConcurrentDictionary<int, ConcurrentDictionary<int, Contact>>();

        internal void _Add(int handle)
        {
            _contacts.TryAdd(handle, new ConcurrentDictionary<int, Contact>());
        }

        internal bool _AreContactsRecorded(int handle)
        {
            return _contacts.ContainsKey(handle);
        }

        internal void _Clear()
        {
            for (int i = 0; i < _contacts.Count; i++)
            {
                _contacts[i].Clear();
            }
        }
        
        internal void _Clear(int handle)
        {
            _contacts[handle].Clear();
        }

        internal bool _IsColliding(int handle)
        {
            return _contacts[handle].Count > 0;
        }

        internal void _ContactGenerated(BepuPhysics.Collidables.CollidableReference ref1, BepuPhysics.Collidables.CollidableReference ref2)
        {
            if(_AreContactsRecorded(ref1.Handle))
            {
                if(!_contacts[ref1.Handle].ContainsKey(ref2.Handle))
                {
                     _contacts[ref1.Handle].TryAdd(ref2.Handle, new Contact(ref1.Handle, ref1.Mobility, ref2.Handle, ref2.Mobility));
                }
            }
        }

        internal List<Contact> _GetContacts(int handle)
        {
            return _contacts[handle].Values.ToList();
        }

        internal Contact _GetContacts(int handle, int contactHandle)
        {
            if (_contacts[handle].ContainsKey(contactHandle))
            {
                return _contacts[handle][contactHandle];
            }
            return null;
        }
    }
}
