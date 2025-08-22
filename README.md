# Made by Asad Arif and Talha Arif 
# Library Management System (MAUI + SQLite)

A desktop Library Management System built with .NET MAUI and SQLite. It supports managing Books and Members and performing Loan transactions (borrow/return) with validation and exception handling.

## IMPORTANT: The login for the app is admin admin

## 🎯 Purpose
Provide librarians a simple, reliable GUI to:
- Add/update/delete **Books** and **Members**
- **Borrow** and **Return** books, with active-loan tracking
- Persist data locally using **SQLite**

## Tech Used
- **.NET MAUI** (WinUI desktop)
- **SQLite-net-pcl** (async ORM)
- **C#**: abstract classes, interfaces, exceptions, validation

##  Design Overview
- **UML**: `Person` (abstract) → `Member`, `Librarian`; `Book : IBorrowable`; `Loan` links Book↔Member
- **ERD**: Tables `Books`, `Members`, `Loans` (PK/FK) with one-to-many relations
- **UI Pages**: Login → Dashboard → Books / Members / Loans



## How to Use
- **Books**: add/update/delete; status flips to *Borrowed* on loan and back to *Available* on return.
- **Members**: add/update/delete.
- **Loans**: select a Member + Book → **Borrow**; select an active loan → **Return**.

## Validation & Exceptions
- Validation throws `ValidationException` for missing/invalid fields.
- Business rules throw `InvalidOperationException` (e.g., borrowing an already borrowed book).
- UI catches exceptions and shows user-friendly messages.

## Highlights
- `Person` **abstract** base; `Member`, `Librarian` derive.
- `IBorrowable` **interface** implemented by `Book`.

- Separation of concerns via `DatabaseService`.
