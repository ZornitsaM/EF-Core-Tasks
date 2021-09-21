using System;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;

namespace PO5_5._Change_Town_Names_Casing
{
    public class StartUp
    {
        private const string ConnectionString =
            @"Server = (localdb)\MSSQLLocalDB;Database=MinionsDB;Integrated Security = true";
        public static void Main(string[] args)
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string countryName = Console.ReadLine();

            string updateTownsNames = @"UPDATE Towns SET Name = UPPER(Name) 
                            WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name =@countryName)";

            string selectTownsNames = @"SELECT t.Name FROM Towns as t JOIN Countries c ON c.Id=t.CountryCode
                                        WHERE c.Name=@countryName";

            using var updateCommand = new SqlCommand(updateTownsNames, connection);
            updateCommand.Parameters.AddWithValue("@countryName", countryName);
            int? affectedRows = updateCommand.ExecuteNonQuery();

            if (affectedRows==0)
            {
                Console.WriteLine($"No town names were affected.");
            }

            else
            {
                Console.WriteLine($"{affectedRows} town names were affected.");

                using var selectCommand = new SqlCommand(selectTownsNames, connection);
                selectCommand.Parameters.AddWithValue("@countryName", countryName);
                using (var reader = selectCommand.ExecuteReader())
                {
                    var towns = new List<string>();
                    while (reader.Read())
                    {
                        towns.Add((string)reader[0]);
                    }

                    Console.WriteLine($"[{string.Join(", ",towns)}]");
                }
            }
        }
    }
}
