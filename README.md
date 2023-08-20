# Pierre's Sweet and Savory Treats

By: Stella Marie

## Technologies Used

- C# 12
- .Net 7.0
  - Identity
  - Entity Framework Core
  - MySQL

## Description

Pierre's Sweet and Savory Treats is an MVC app for managing treats and flavors in a many-to-many relationship. With authentication and authorization implemented, users, likely bakery employees, can create, edit and delete treats and flavors offered. Anyone can view all treats and flavors and view their details.

**NOTE**: If you would like to see or test the features with authentication and authorization, either click on the site name in the header (Pierre's Sweet and Savory Treats) or navigate to /Account/Login.

- Users
  - Register
  - Log in/out
  - Create, update and delete flavors and treats

- Anyone
  - View all flavors and treats
  - View details

## Complete Setup

This app requires use of a database. It is suggested to use migrations for ensuring the smooth setup of Identity.

### Database Schemas

To setup the database, follow the directions in [Connecting the Database](#connecting-the-database) and then implement migrations as detailed in [Migrations](#migrations).

### Connecting the Database

In your IDE:
- Create a file in the Bakery assembly: appsettings.json
  - Do not remove the mention of this file from .gitignore
- Add this code:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=[hostname];Port=[port_number];database=[database_name];uid=[username];pwd=[password]"
    }
}
```

Replace the values accordingly and brackets are not to be included.

### Migrations

- In the terminal, run ```dotnet build --project Bakery```
  - Or navigate into the Factory subdirectory of the project and run ```dotnet build```

This command will install the necessary dependencies. For it to work though, appsettings.json should already be setup. As migrations are included in the clone or fork, you should only need to run:

```dotnet ef database update```

However, since the models are already set up, if update does not work, then do:

```bash
dotnet ef migrations add Initial
dotnet ef database update
```

### Run the App

Once you have a database setup and the connection string included in the appsettings.json, you can run the app:

- Navigate to main page of repo
- Either fork or clone project to local directory
- Bash or Terminal: ```dotnet run --project Bakery```
  - If you navigate into the main assembly: Bakery, use ```dotnet run```

If the app does not launch in the browser:
- Run the app
- Open a browser
- Enter the url: https://localhost:5001/

## Known Bugs

- FIXED: Register page lists invalid or incomplete fields
- FIXED: User cannot submit invalid or incomplete forms for creating, editing or adding to treats and flavors

Please report any issues in using the app.

## License

[MIT](https://choosealicense.com/licenses/mit/)