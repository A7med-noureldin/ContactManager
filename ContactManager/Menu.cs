using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManager
{
    internal class Menu
    {
            public void Display()
            {
                Console.WriteLine("=======Contact Manager=======");
                Console.WriteLine("1. Add Contact");
                Console.WriteLine("2. Edit Contact");
                Console.WriteLine("3. Delete Contact");
                Console.WriteLine("4. View Contact");
                Console.WriteLine("5. List Contacts");
                Console.WriteLine("6. Search");
                Console.WriteLine("7. Filter");
                Console.WriteLine("8. Save");
                Console.WriteLine("9. Exit");
            }

            public int GetInput()
            {
                while (true)
                {
                    var input = Console.ReadLine();
                    if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 9)
                    {
                        return choice;
                    }

                    Console.Write("Invalid choice. Please enter a number (1-9): ");
                }
            }
    }
}
