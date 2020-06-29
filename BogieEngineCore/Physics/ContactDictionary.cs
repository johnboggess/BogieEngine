using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BogieEngineCore.Physics
{
    class ContactDictionary
    {
        ConcurrentDictionary<ContactInfo, List<ContactInfo>> _contacts = new ConcurrentDictionary<ContactInfo, List<ContactInfo>>();

        internal void _Add(ContactInfo info)
        {
            _contacts.TryAdd(info, new List<ContactInfo>());
        }

        internal bool _AreContactsRecorded(ContactInfo info)
        {
            return _contacts.ContainsKey(info);
        }

        internal void _Clear()
        {
            foreach (KeyValuePair<ContactInfo, List<ContactInfo>> info in _contacts)
            {
                _contacts[info.Key].Clear();
            }
        }

        internal void _Clear(ContactInfo info)
        {
            _contacts[info].Clear();
        }

        internal bool _IsColliding(ContactInfo info)
        {
            return _contacts[info].Count > 0;
        }

        internal void _ContactGenerated(BepuPhysics.Collidables.CollidableReference ref1, BepuPhysics.Collidables.CollidableReference ref2)
        {
            ContactInfo co1 = new ContactInfo(ref1.Handle, ref1.Mobility);
            ContactInfo co2 = new ContactInfo(ref2.Handle, ref2.Mobility);
            if (_AreContactsRecorded(co1))
            {
                if (!_contacts[co1].Contains(co2))
                {
                    _contacts[co1].Add(co2);
                }
            }
        }

        internal List<ContactInfo> _GetContacts(ContactInfo info)
        {
            return _contacts[info];
        }

        internal bool _AreInContact(ContactInfo info1, ContactInfo info2)
        {
            return _contacts.ContainsKey(info1) && _contacts.ContainsKey(info2);
        }
    }
}
