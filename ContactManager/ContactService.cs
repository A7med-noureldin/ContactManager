using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactManager
{
    internal class ContactService : IContactService
    {
        private readonly JsonContactProvider provider;

        public ContactService(JsonContactProvider Provider)
        {
            provider = Provider;
        }

        public void AddContact(string name, string email, string phoneNumber)
        {
            var newContact = new Contact(name, email, phoneNumber);
            provider.Add(newContact);
        }

        public void EditContact(Guid id, string name, string email, string phoneNumber)
        {
            var contact = provider.GetById(id);
            contact.Update(name, email, phoneNumber);
            provider.Update(contact);
        }

        public void DeleteContact(Guid id) => provider.Delete(id);

        public IEnumerable<Contact> GetAllContacts() => provider.GetAll();

        public Contact GetContactById(Guid id) => provider.GetById(id);

        public IEnumerable<Contact> Search(string keyword) => provider.Search(keyword);

        public IEnumerable<Contact> FilterByName(string name)
        {
            name = name.ToLower();
            return provider.GetAll().Where(c => c.Name.ToLower().Contains(name));
        }

        public IEnumerable<Contact> FilterByEmail(string email)
        {
            email = email.ToLower();
            return provider.GetAll().Where(c => c.Email.ToLower().Contains(email));
        }

        public IEnumerable<Contact> FilterByPhone(string phone)
            => provider.GetAll().Where(c => c.PhoneNumber.Contains(phone));

        public IEnumerable<Contact> FilterByCreationDate(DateOnly from, DateOnly to)
            => provider.GetAll().Where(c => c.CreatedAt >= from && c.CreatedAt <= to);

        public void SaveContacts() => provider.Save();
    }
}