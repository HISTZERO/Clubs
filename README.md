# ClubsService source code
## **ClubsService - Part 2**
### 📄 [Part 2.md](#)

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) 
- (Optional) [Entity Framework Core CLI Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)  
  Install via:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Build Procedure
1. **Restore Dependencies:**
   ```bash
   dotnet restore
   ```

2. **Build the Solution:**
   ```bash
   dotnet build
   ```

3. **Configure the Database:**
   Update the connection string in `src/ClubsService/appsettings.json`. For example:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=ClubsDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
     }
   }
   ```

4. **Run EF Core Migrations:**
   Create the initial migration and update the database:
   ```bash
   dotnet ef migrations add InitialCreate --project src/ClubsService
   dotnet ef database update --project src/ClubsService
   ```

5. **Run the Application:**
   Start the API by running:
   ```bash
   dotnet run --project src/ClubsService
   ```

## Test Procedure
### Automated Unit Tests
1. **Run Unit Tests:**
   From the solution root, run:
   ```bash
   dotnet test
   ```
   This command builds and runs all tests in your test project and displays the results in the terminal or Visual Studio's Test Explorer.

### Manual Testing
2. **Test via Swagger or Postman:**
   - Open [Swagger UI](http://localhost:7069/swagger) or use Postman.
   - **Example for Creating a Club:**
     - **Request Headers:**
       ```
       Content-Type: application/json
       Accept: application/json
       Player-ID: 123
       ```
     - **Request Body:**
       ```json
       {
         "name": "My Awesome Club"
       }
       ```
     - **Expected Response:**
       - **Status Code:** 201 Created
       - **Response Body:**
         ```json
         {
           "id": "5e324948-6a88-40e6-8b09-578d061de816",
           "members": [123]
         }
         ```
       - **Location Header:** `/api/clubs/{clubId}`

