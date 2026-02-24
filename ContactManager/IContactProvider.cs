using System;
using System.Collections.Generic;

namespace ContactManager
{
    internal interface IContactProvider
    {
        void Add(Contact contact);
        void Update(Contact contact);
        void Delete(Guid id);
        Contact GetById(Guid id);
        IEnumerable<Contact> GetAll();
        void Save();
    }
}