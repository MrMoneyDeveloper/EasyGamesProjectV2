using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;

namespace EasyGamesProjectV2.Data
{
    public class DbInitializer
    {
        private readonly IConfiguration _configuration;

        public DbInitializer(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Initialize()
        {
            try
            {
                // Create or recreate the database
                ExecuteScript("dbo\\Script\\CreateDatabase.sql", "MasterConnection");

                // Create tables
                ExecuteScript("dbo\\Script\\CreateTables.sql", "DefaultConnection");

                // Seed data
                ExecuteScript("dbo\\Script\\SeedData.sql", "DefaultConnection");

                // Create stored procedures
                ExecuteScript("dbo\\StoredProcedures\\spAddClient.sql", "DefaultConnection");
                ExecuteScript("dbo\\StoredProcedures\\spAddTransaction.sql", "DefaultConnection");
                ExecuteScript("dbo\\StoredProcedures\\spDeleteClient.sql", "DefaultConnection");
                ExecuteScript("dbo\\StoredProcedures\\spGetAllClients.sql", "DefaultConnection");
                ExecuteScript("dbo\\StoredProcedures\\spGetClientByID.sql", "DefaultConnection");
                ExecuteScript("dbo\\StoredProcedures\\spGetTransactionsByClientId.sql", "DefaultConnection");
                ExecuteScript("dbo\\StoredProcedures\\spUpdateClient.sql", "DefaultConnection");
            }
            catch (Exception ex)
            {
                // Log exception here
                Console.WriteLine($"Error initializing the database: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw new InvalidOperationException("Database initialization failed.", ex);
            }
        }

        private void ExecuteScript(string scriptFilePath, string connectionStringName)
        {
            if (!File.Exists(scriptFilePath))
            {
                throw new FileNotFoundException($"The SQL script file '{scriptFilePath}' was not found.");
            }

            var script = File.ReadAllText(scriptFilePath);
            var scriptParts = script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            using (var connection = new SqlConnection(_configuration.GetConnectionString(connectionStringName)))
            {
                connection.Open();

                foreach (var scriptPart in scriptParts)
                {
                    if (!string.IsNullOrWhiteSpace(scriptPart))
                    {
                        try
                        {
                            using (var command = new SqlCommand(scriptPart, connection))
                            {
                                command.CommandType = CommandType.Text;
                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log specific script execution errors
                            Console.WriteLine($"Error executing script part: {scriptPart}");
                            Console.WriteLine($"Error: {ex.Message}");
                            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                            throw;
                        }
                    }
                }
            }
        }
    }
}
