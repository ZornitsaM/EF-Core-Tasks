using System;
using Microsoft.Data.SqlClient;

namespace PO2_2._Villain_Names
{
    public class StartUp
    {
        private const string ConnectionString =
           @"Server = (localdb)\MSSQLLocalDB;Database=MinionsDB;Integrated Security = true";
        public static void Main(string[] args)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string query = @"SELECT v.Name,COUNT(*) FROM MinionsVillains mv
                                            JOIN Villains v ON v.Id = mv.VillainId
                                            GROUP BY v.Id,v.Name
                                            HAVING COUNT(*)>3
                                            ORDER BY COUNT(*) DESC";
                                           

            using (var command = new SqlCommand(query, connection))
                
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader[0];
                        var countMinions = reader[1];
                        Console.WriteLine($"{name} - {countMinions}");
                    }
                    
                }

            }




        }
    }
}
