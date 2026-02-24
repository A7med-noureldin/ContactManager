using System;
using System.Collections.Generic;

namespace ContactManager
{
    internal interface IContactService
    {
        void AddContact(string name, string email, string phoneNumber);
        void EditContact(Guid id, string name, string email, string phoneNumber);
        void DeleteContact(Guid id);
        void SaveContacts();

        Contact GetContactById(Guid id);
        IEnumerable<Contact> GetAllContacts();
        IEnumerable<Contact> Search(string keyword);

        IEnumerable<Contact> FilterByName(string name);
        IEnumerable<Contact> FilterByEmail(string email);
        IEnumerable<Contact> FilterByPhone(string phone);
        IEnumerable<Contact> FilterByCreationDate(DateOnly from, DateOnly to);
    }
}