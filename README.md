# Contact Manager CLI (C#)

A modern C# command‑line Contact Management System built using Object‑Oriented Programming and SOLID principles, with JSON‑based persistent storage and efficient search/indexing.

---

## Overview

This project implements a Command‑Line Contact Management System that allows users to store, retrieve, search, filter, and manage contact information efficiently.

It replaces an outdated phone‑book‑style lookup with a structured, indexed design that supports rapid operations and maintainable code.

---

## Features

- Add a new contact with validation  
- Edit an existing contact  
- Delete contacts  
- View full contact details  
- List all contacts  
- Search contacts by keyword (name, email, phone)  
- Filter contacts by name, email, phone, or creation date  
- JSON persistent storage ("contacts.json")  
- Automatic GUID generation and true creation timestamp

---

## Contact Structure

Each contact includes:

| Field        | Description                           |
|--------------|---------------------------------------|
| Id           | Auto‑generated unique identifier      |
| Name         | Contact’s full name                   |
| Email        | Validated email address               |
| Phone Number | Validated phone number (10–15 digits) |
| CreatedAt    | Auto‑generated creation date          |
--------------------------------------------------------
---

## Architecture & Design

This project is designed with layered separation:

- **Presentation Layer** — CLI (`Program.cs`, `Menu.cs`)  
- **Application Layer** — `ContactService`  
- **Domain Layer** — `Contact` entity with encapsulated validation  
- **Infrastructure Layer** — `JsonContactProvider` for storage + indexing

Important design principles applied:

- Single Responsibility (SRP)  
- Interface Segregation (ISP)  
- Validation within domain entity  
- Separation of concerns
- Atomicity like a transaction

---

## Storage System

Contact data is stored in a file named `contacts.json` at the project root.

- On startup, the JSON file is loaded (if present).  
- Users can make changes via CLI operations.  
- Changes are written to JSON when “Save” is selected.

---

## How to Run

### Requirements
- .NET 6.0 SDK or later

### Steps

1. Clone the repository
   'git clone https://github.com/A7med-noureldin/ContactManager'
2. Navigate to project folder
3. Build the project
4. Run the application

---

## License

This project is submitted as part of the *Microsoft Summer Internship 2026*.
