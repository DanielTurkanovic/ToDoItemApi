<h1> ToDoItemApi</h1>

<p><strong>Version:</strong> ASP.NET Core 8</p>

<h2>1. Description</h2>
<p>This is a simple RESTful Web API built with <strong>ASP.NET Core 8.0</strong> that allows registered users to manage their to-do tasks. Each user can perform CRUD operations only on their own tasks.</p>

<hr>

<h2> 2. Features</h2>
<ul>
  <li>User registration and login (JWT authentication)</li>
  <li>Authorization using bearer token</li>
  <li>CRUD operations for ToDo items</li>
  <li>Search by title and description</li>
  <li>User account management:</li>
      Users can soft-delete (deactivate) their own account</br>
      Admins can soft-delete any account and restore deleted accounts</li>
  <li>Email verification on registration</li>
  <li>AutoMapper for DTO mapping</li>
  <li>EF Core with SQL Server</li>
  <li>Repository Pattern used</li>
  <li>Request validation with FluentValidation</li>
</ul>

<hr>

<h2> 3. Technologies Used</h2>
<ul>
  <li>ASP.NET Core 8.0</li>
  <li>Entity Framework Core</li>
  <li>SQL Server</li>
  <li>JWT Bearer Authentication</li>
  <li>AutoMapper</li>
  <li>Swagger (Swashbuckle)</li>
  <li>FluentValidation</li>
  <li>NUnit (for unit testing)</li>
</ul>

<hr>

<h2>4. Getting Started</h2>
<p><strong> This API uses a <code>appsettings.Development.json</code> file for secrets such as database connection strings and JWT keys. You must create this file before running the app.</strong></p>

<ol>
  <li><strong>Clone the repository:</strong>
    <pre><code>git clone https://github.com/your-username/ToDoItemApi.git
cd ToDoItemApi</code></pre>
  </li>
  <li><strong>Create the <code>appsettings.Development.json</code> file</strong> in the root of the project.</li>
  <li><strong>Add your connection string and JWT settings</strong></li>
  <li><strong>Apply EF Core migrations and create the database:</strong>
    <pre><code>dotnet ef database update</code></pre>
  </li>
  <li><strong>Run the application:</strong>
    <pre><code>dotnet run</code></pre>
  </li>
  <li><strong>Open Swagger UI:</strong> Navigate to <code>https://localhost:5001/swagger</code> or as shown in console output.</li>
  <li><strong>Use Swagger to:</strong>
    <ul>
      <li>Register a new user (<code>/api/Auth/Register</code>)</li>
      <li>Log in (<code>/api/Auth/Login</code>) and copy the JWT token</li>
      <li>Click “Authorize” in Swagger and paste the token: <code>Bearer &lt;token&gt;</code></li>
      <li>Access protected ToDo endpoints</li>
    </ul>
  </li>
</ol>

<hr>

<h2> 5. Notes</h2>
<ul>
  <li>Only authenticated users can access ToDo endpoints</li>
  <li>Each user sees only <strong>their own</strong> tasks</li>
  <li>Only admins can view, soft-delete, or restore user accounts</li>
  <li>The app <strong>seeds users</strong> in the database automatically (see <code>DbSeeder</code>)</li>
  <li>JWT tokens are used to secure all protected routes</li>
</ul>

<hr>

<h2>6. Future Improvements</h2>
<ul>
  <li>Pagination and sorting</li>
  <li>Refresh tokens</li>
</ul>

<hr>

<h2> 7. Configuration</h2>
<p>Secrets are <strong>not</strong> stored in <code>appsettings.json</code>, which is committed to Git.  
Instead, use a separate file ignored by Git:</p>

<h4> <code>appsettings.Development.json</code></h4>
<p>(ignored by <code>.gitignore</code>)</p>

<p><strong>Example:</strong></p>
<pre><code>{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=YOUR_SERVER;Initial Catalog=ToDoDb;Integrated Security=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "ToDoApi",
    "Audience": "ToDoApiUser",
    "ExpiresInMinutes": 60
  }
}</code></pre>

<p><strong> Never share this file or its contents publicly. It is used only during development.</strong></p>

<h4> Generating a secure JWT key</h4>
<p>To generate a secure <code>Key</code> value for the <code>JwtSettings</code>, run the following PowerShell commands:</p>

<pre><code>$bytes = New-Object 'System.Byte[]' 32
(New-Object System.Security.Cryptography.RNGCryptoServiceProvider).GetBytes($bytes)
[Convert]::ToBase64String($bytes)</code></pre>

<p>This will output a secure 256-bit base64 string that you can copy and paste into the <code>Key</code> field.</p>

<hr>

<h2>8. Testing</h2>
<p>This project includes <strong>unit tests</strong> for:</p>
<ul>
  <li> <strong>ToDoValidatorTests</strong> – Verifies that the validation logic (using FluentValidation) correctly accepts or rejects input data to ensure early validation and data integrity.</li>
  <li> <strong>ToDoControllerTests</strong> – Tests the controller’s logic, HTTP responses, and interactions with services by simulating HTTP requests.</li>
  <li> <strong>ToDoRepositoryTests</strong> – Confirms the repository correctly accesses and returns data from the database using an in-memory provider.</li>
</ul>
