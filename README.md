# EfCore6Test

An ASP.NET Core Web API and Entity Framework Core practice project. It targets .NET 6 and uses EF Core 6.0.21 with the SQL Server provider. Swagger is added with Swashbuckle.AspNetCore 6.5.0.

## What it covers

Three controllers:

- `UserController` gets, inserts, updates, and deletes users.
- `RuleController` gets, inserts, updates, and deletes roles.
- `LoginController` checks a username and password, sets `LastLoginTime` when they match, and can replace a user's password.

Passwords are stored on `User` and compared as plain text.

`TestContext` is the EF Core context. It exposes `User` and `Role`. A user has a required `RoleId`, and a role has a collection of users. The context maps the entities to tables named `user` and `role`. `OnModelCreating` also marks `Role.RoleTitle` as unique; that index is not in the checked-in migrations. `UserInsert` is an input model marked `[NotMapped]`, so it is not a table.

`EfCore6Test/Migrations` contains the existing EF Core migrations. The initial migration creates `role` and `user`, with a foreign key from `user.RoleId` to `role.RoleId`. The later migrations in that folder do not change the schema.

Swagger UI is enabled only when the host environment is Development.

## How to run

1. Install the .NET 6 SDK.
2. Use a SQL Server instance the app can reach. The connection string in `EfCore6Test/appsettings.json` is a local placeholder:

   `Server=localhost;Database=myDataBase;Trusted_Connection=True;TrustServerCertificate=True`

   `Trusted_Connection` is Windows authentication. Change the value if the server, database name, or login is different. Setting the `ConnectionStrings__DefaultConnection` environment variable overrides `appsettings.json`.

3. From the repository root:

   ```bash
   dotnet restore
   dotnet tool install --global dotnet-ef --version 6.0.21
   dotnet ef database update --project EfCore6Test
   dotnet run --project EfCore6Test
   ```

`dotnet run` reads `EfCore6Test/Properties/launchSettings.json`. The `EfCore6Test` profile sets `ASPNETCORE_ENVIRONMENT` to Development and listens on `https://localhost:7100` and `http://localhost:5177`. In Development, Swagger UI is at `/swagger`.
