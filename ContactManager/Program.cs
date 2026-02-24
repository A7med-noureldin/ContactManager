using System;
using System.Linq;
using System.Collections.Generic;

namespace ContactManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var provider = new JsonContactProvider();
            var service = new ContactService(provider);
            var menu = new Menu();
            bool running = true;


            // Load Data & Display Existing Contacts
            Console.WriteLine("===== Contact Manager =====");
            Console.WriteLine();

            var existingContacts = service.GetAllContacts().ToList();

            if (!existingContacts.Any())
            {
                Console.WriteLine("No contacts found.");
            }
            else
            {
                Console.WriteLine("Existing Contacts:");
                foreach (var c in existingContacts)
                    Console.WriteLine($"{c.Name} | {c.Email} | {c.PhoneNumber} | {c.CreatedAt}");
            }

            Console.WriteLine();

            while (running)
            {
                menu.Display();
                int choice = menu.GetInput();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case 1: // Add
                            Console.Write("Name: ");
                            var name = Console.ReadLine();

                            Console.Write("Email: ");
                            var email = Console.ReadLine();

                            Console.Write("Phone: ");
                            var phone = Console.ReadLine();

                            service.AddContact(name, email, phone);
                            Console.WriteLine("Contact added successfully.");
                            break;

                        case 2: // Edit
                            var contactToEdit = SelectContact(service);

                            Console.Write("New Name: ");
                            var newName = Console.ReadLine();

                            Console.Write("New Email: ");
                            var newEmail = Console.ReadLine();

                            Console.Write("New Phone: ");
                            var newPhone = Console.ReadLine();

                            service.EditContact(contactToEdit.Id, newName, newEmail, newPhone);
                            Console.WriteLine("Contact updated successfully.");
                            break;

                        case 3: // Delete
                            var contactToDelete = SelectContact(service);
                            service.DeleteContact(contactToDelete.Id);
                            Console.WriteLine("Contact deleted successfully.");
                            break;

                        case 4: // View
                            var contactToView = SelectContact(service);
                            Console.WriteLine($"{contactToView.Name} | {contactToView.Email} | {contactToView.PhoneNumber}");
                            break;

                        case 5: // List
                            var all = service.GetAllContacts();
                            if (!all.Any())
                            {
                                Console.WriteLine("No contacts available.");
                            }
                            else
                            {
                                foreach (var c in all)
                                    Console.WriteLine($"{c.Name} | {c.Email} | {c.PhoneNumber}");
                            }
                            break;

                        case 6: // Search
                            Console.Write("Enter keyword: ");
                            var keyword = Console.ReadLine();

                            var results = service.Search(keyword);

                            if (!results.Any())
                            {
                                Console.WriteLine("No contacts found.");
                            }
                            else
                            {
                                foreach (var c in results)
                                    Console.WriteLine($"{c.Name} | {c.Email} | {c.PhoneNumber}");
                            }
                            break;

                        case 7: // Filter
                            Console.WriteLine("Filter by:");
                            Console.WriteLine("1. Name");
                            Console.WriteLine("2. Email");
                            Console.WriteLine("3. Phone");
                            Console.WriteLine("4. Creation Date");

                            if (!int.TryParse(Console.ReadLine(), out int filterChoice))
                                throw new Exception("Invalid filter choice.");

                            IEnumerable<Contact> filtered = Enumerable.Empty<Contact>();

                            switch (filterChoice)
                            {
                                case 1:
                                    Console.Write("Enter name: ");
                                    filtered = service.FilterByName(Console.ReadLine());
                                    break;

                                case 2:
                                    Console.Write("Enter email: ");
                                    filtered = service.FilterByEmail(Console.ReadLine());
                                    break;

                                case 3:
                                    Console.Write("Enter phone: ");
                                    filtered = service.FilterByPhone(Console.ReadLine());
                                    break;

                                case 4:
                                    Console.Write("From (yyyy-mm-dd): ");
                                    var from = DateOnly.Parse(Console.ReadLine());

                                    Console.Write("To (yyyy-mm-dd): ");
                                    var to = DateOnly.Parse(Console.ReadLine());

                                    filtered = service.FilterByCreationDate(from, to);
                                    break;

                                default:
                                    Console.WriteLine("Invalid filter option.");
                                    break;
                            }

                            if (!filtered.Any())
                            {
                                Console.WriteLine("No contacts found.");
                            }
                            else
                            {
                                foreach (var c in filtered)
                                    Console.WriteLine($"{c.Name} | {c.Email} | {c.PhoneNumber}");
                            }

                            break;

                        case 8: // Save
                            service.SaveContacts();
                            Console.WriteLine("Contacts saved.");
                            break;

                        case 9: // Exit
                            running = false;
                            Console.WriteLine("Goodbye!");
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine();
            }
        }

        private static Contact SelectContact(ContactService service)
        {
            var contacts = service.GetAllContacts().ToList();

            if (!contacts.Any())
                throw new Exception("No contacts available.");

            for (int i = 0; i < contacts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {contacts[i].Name} | {contacts[i].Email} | {contacts[i].PhoneNumber} | {contacts[i].CreatedAt}");
            }

            Console.Write("Select a contact number: ");

            if (!int.TryParse(Console.ReadLine(), out int choice) ||
                choice < 1 || choice > contacts.Count)
                throw new Exception("Invalid selection.");

            return contacts[choice - 1];
        }
    }
}