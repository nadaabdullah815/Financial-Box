# Financial Box 

A web-based system for managing personal and small-business financial funds. It helps users accurately track their income and expenses and manage multiple financial boxes in one place instead of relying on paper records or separate Excel files.

The project was developed individually (**Frontend + Backend + Database**) as a practical training project using **ASP.NET Core MVC**.

---

## Main Features

### Account Management

* User registration and secure login using ASP.NET Identity.
* Persistent login sessions that remain active until the user manually logs out.

### Financial Boxes

* Create multiple financial boxes with support for two currencies: **Syrian Pound (SYP) / US Dollar (USD)**.
* Four default boxes are automatically created for each user upon registration: **Sham Cash**, **Syriatel Cash**, **Home Cash**, and **Debt Settlement**.
* Protection of default boxes from accidental deletion or modification.
* The debt box is independent from the general reporting accounts, with a dedicated interface and terminology (**Debt / Payment**) and a progress bar toward the repayment target.

### Financial Transactions

* Record income and expense transactions linked to a specific box and category.
* Automatically update the box balance within a single **Database Transaction** to ensure data consistency.
* Predefined default income and expense categories, with the ability for users to add their own custom categories.

### Internal Transfers

* Transfer money between the user's own boxes using the same currency.
* Update both box balances within a single transaction to prevent money loss or duplication.

### Reports

* Income and expense lists filtered by a specific date range.
* Current balances of all financial boxes.
* Income and expense aggregation by category with percentage breakdowns.

### Main Dashboard

* Quick summary of the current month's income and expenses.
* Latest recorded transactions.
* Quick navigation links to all system sections.

---

## Technologies Used

| Technology                    | Usage                                                            |
| ----------------------------- | ---------------------------------------------------------------- |
| ASP.NET Core MVC (.NET)       | Main project architecture (Backend + Frontend in one project)    |
| Entity Framework Core         | Database interaction                                             |
| SQL Server                    | Database                                                         |
| ASP.NET Identity              | User authentication and account management                       |
| Bootstrap 5 + Bootstrap Icons | User interface design                                            |
| JavaScript                    | Dynamic UI interactions (live summaries, option switching, etc.) |

---

## Architecture

The project follows a three-layer architecture:

```text
Controller → Service → DbContext → Database
```

* **Controller**: Handles user requests only, without business or calculation logic.
* **Service**: Contains all business logic, calculations, validations, and business rules.
* **DbContext (EF Core)**: Acts as the bridge between the application and the database.

### Core Entities

```text
ApplicationUser (User)

    │
    ├── Box (Financial Boxes) — Each box belongs to one user
    │     └── Transaction (Financial Transactions)
    │
    ├── Category — Shared or user-specific categories
    │     └── Transaction
    │
    └── Transfer — Internal transfers between the user's boxes
```

### Core Business Rules

* Amounts less than or equal to zero are not accepted.
* Every financial transaction must be associated with a box and a category.
* Users can access only their own data.
* Updating the balance and saving the transaction are performed within a single transaction to prevent data inconsistencies.

---

## Local Setup

```bash
# Clone the repository
git clone <repository-url>

cd FinancialBox

# Restore packages
dotnet restore

# Update the database connection string in appsettings.json
# "ConnectionStrings": { "DefaultConnection": "..." }

# Apply migrations
dotnet ef database update

# Run the project
dotnet run
```

---

## Known Limitations

* The project is a financial record-keeping system, not a real electronic wallet. It does not connect to real bank accounts or external payment services.
* Password recovery (**Forgot Password**) is not currently implemented.
* Invoice/image attachments, monthly budgeting, and month-to-month comparison percentages are planned as future improvements.

---

## Future Improvements

* Password recovery via email.
* Upload and attach invoices and receipts.
* Set a monthly budget and compare spending with previous months.
* Export reports as PDF or Excel files.
