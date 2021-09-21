using System;
using Microsoft.Data.SqlClient;

namespace PO9_Increase_Age_Stored_Procedure
{
    public class StartUp
    {
        private const string ConnectionString =
        @"Server = (localdb)\MSSQLLocalDB;Database=MinionsDB;Integrated Security = true";
        public static void Main(string[] args)
        {

            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            int id = int.Parse(Console.ReadLine());
            string query = @"EXEC usp_GetOlder @id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            string selectQuery = @"SELECT Name,Age FROM Minions where Id=@Id";
            using var secondCommand = new SqlCommand(selectQuery, connection);
            secondCommand.Parameters.AddWithValue("@Id", id);
            using var reader = secondCommand.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"{reader[0]} – {reader[1]} years old");
            }
        }
    }
}
