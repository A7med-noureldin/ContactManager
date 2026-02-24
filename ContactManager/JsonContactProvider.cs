using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ContactManager
{
    internal class JsonContactProvider : IContactProvider
    {
        private readonly Dictionary<Guid, Contact> ContactsList;

        // Indexes for fast search
        private readonly Dictionary<string, List<Contact>> nameIndex = new();
        private readonly Dictionary<string, List<Contact>> emailIndex = new();
        private readonly Dictionary<string, List<Contact>> phoneIndex = new();

        private readonly string filePath = "contacts.json";

        public JsonContactProvider()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var contacts = JsonSerializer.Deserialize<List<Contact>>(json);
                ContactsList = contacts?.ToDictionary(c => c.Id) ?? new Dictionary<Guid, Contact>();

                foreach (var contact in ContactsList.Values)
                    IndexContact(contact);
            }
            else
            {
                ContactsList = new Dictionary<Guid, Contact>();
            }
        }

        public void Add(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            // Prevent duplicate email
            if (ContactsList.Values.Any(c => c.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A contact with this email already exists.");

            ContactsList[contact.Id] = contact;
            IndexContact(contact);
        }

        public void Update(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            if (!ContactsList.ContainsKey(contact.Id))
                throw new KeyNotFoundException("Contact not found.");

            // Prevent duplicate email for other contacts
            if (ContactsList.Values.Any(c => c.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase) && c.Id != contact.Id))
                throw new InvalidOperationException("Another contact with this email already exists.");

            RemoveFromIndex(ContactsList[contact.Id]); // remove old data from indexes
            ContactsList[contact.Id] = contact;
            IndexContact(contact);
        }

        public void Delete(Guid id)
        {
            if (!ContactsList.TryGetValue(id, out var contact))
                throw new KeyNotFoundException("Contact not found.");

            RemoveFromIndex(contact);
            ContactsList.Remove(id);
        }

        public Contact GetById(Guid id)
        {
            if (ContactsList.TryGetValue(id, out var contact))
                return contact;

            throw new KeyNotFoundException("Contact not found.");
        }

        public IEnumerable<Contact> GetAll() => ContactsList.Values;

        public void Save()
        {
            // Remove exact duplicates by email before saving
            var contacts = ContactsList.Values
                .GroupBy(c => c.Email.ToLower())
                .Select(g => g.First())
                .ToList();

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(contacts, options);
            File.WriteAllText(filePath, json);

            Console.WriteLine("Contacts saved to: " + Path.GetFullPath(filePath));
        }

        #region Index Methods
        private void IndexContact(Contact contact)
        {
            string nameKey = contact.Name.ToLower();
            if (!nameIndex.ContainsKey(nameKey)) nameIndex[nameKey] = new List<Contact>();
            nameIndex[nameKey].Add(contact);

            string emailKey = contact.Email.ToLower();
            if (!emailIndex.ContainsKey(emailKey)) emailIndex[emailKey] = new List<Contact>();
            emailIndex[emailKey].Add(contact);

            string phoneKey = contact.PhoneNumber;
            if (!phoneIndex.ContainsKey(phoneKey)) phoneIndex[phoneKey] = new List<Contact>();
            phoneIndex[phoneKey].Add(contact);
        }

        private void RemoveFromIndex(Contact contact)
        {
            nameIndex[contact.Name.ToLower()].Remove(contact);
            emailIndex[contact.Email.ToLower()].Remove(contact);
            phoneIndex[contact.PhoneNumber].Remove(contact);
        }

        public IEnumerable<Contact> Search(string keyword)
        {
            keyword = keyword.ToLower();
            var results = new HashSet<Contact>();

            foreach (var key in nameIndex.Keys.Where(k => k.Contains(keyword)))
                results.UnionWith(nameIndex[key]);
            foreach (var key in emailIndex.Keys.Where(k => k.Contains(keyword)))
                results.UnionWith(emailIndex[key]);
            foreach (var key in phoneIndex.Keys.Where(k => k.Contains(keyword)))
                results.UnionWith(phoneIndex[key]);

            return results;
        }
        #endregion
    }
}