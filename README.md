
  <h1>ToDoItemApi</h1>

   <p><strong>Version:</strong> ASP.NET Core 8</p>

   <h2>1. Description</h2>
    <p>This is a simple RESTful Web API built with ASP.NET Core 8.0 that allows registered users to manage their to-do tasks. Each user can perform CRUD operations only on their own tasks.</p>

   <h2>2. Features</h2>
    <ul>
        <li>User registration and login (JWT authentication)</li>
        <li>Authorization using bearer token</li>
        <li>CRUD operations for ToDo items</li>
        <li>Search by title and description</li>
        <li>AutoMapper for DTO mapping</li>
        <li>EF Core with SQL Server</li>
        <li>Repository Pattern used</li>
    </ul>

   <h2>3. Technologies Used</h2>
    <ul>
        <li>ASP.NET Core 8.0</li>
        <li>Entity Framework Core</li>
        <li>SQL Server (local)</li>
        <li>JWT Bearer Authentication</li>
        <li>Swagger (Swashbuckle)</li>
        <li>AutoMapper</li>
    </ul>

   <h2>4. Getting Started</h2>
    <ol>
        <li>Clone the repository</li>
        <li>Run <code>dotnet ef database update</code> to apply migrations</li>
        <li>Run the app with Visual Studio 2022</li>
        <li>Use Swagger UI to register, login, and authorize</li>
    </ol>

  <h2>5. Notes</h2>
    <ul>
        <li>Only authenticated users can access ToDo endpoints</li>
        <li>Each user sees only their own tasks</li>
        <li>Seeded users are available in the database</li>
    </ul>

  <h2>6. Future Improvements (Optional)</h2>
    <ul>
        <li>Adding email verification</li>
        <li>Allow users to delete their own account</li>
    </ul>


