using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ContactManager
{
    internal class Contact
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateOnly CreatedAt { get; }

        public Contact(string name, string email, string phoneNumber)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            Update(name, email, phoneNumber);
        }

        public void Update(string name, string email, string phoneNumber)
        {
            // Atomicity: validate all fields before updating any
            ValidateName(name);
            ValidateEmail(email);
            ValidatePhone(phoneNumber);

            Name = name.Trim();
            Email = email.Trim().ToLower();
            PhoneNumber = phoneNumber.Trim();
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.");

            try { var addr = new MailAddress(email); }
            catch { throw new ArgumentException("Invalid email format."); }
        }

        private void ValidatePhone(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty.");

            if (!Regex.IsMatch(phoneNumber, @"^\d{10,15}$"))
                throw new ArgumentException("Phone number must contain 10–15 digits.");
        }
    }
}