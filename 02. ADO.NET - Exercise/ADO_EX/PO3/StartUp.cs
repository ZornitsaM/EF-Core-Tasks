using System;
using Microsoft.Data.SqlClient;

namespace PO3
{
    public class StartUp
    {
        private const string ConnectionString =
            @"Server = (localdb)\MSSQLLocalDB;Database=MinionsDB;Integrated Security = true";
        public static void Main(string[] args)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            int Id = int.Parse(Console.ReadLine());

            string villianNameQuery = "SELECT [Name] FROM Villains WHERE Id = @Id";

            using var villianCommand = new SqlCommand(villianNameQuery, connection);
            villianCommand.Parameters.AddWithValue("@Id", Id);
            var name = villianCommand.ExecuteScalar();

            string minionsQuery = @"SELECT ROW_NUMBER() OVER(ORDER BY m.Name) AS Row,m.Name,m.Age FROM MinionsVillains mv
                                    JOIN Minions m ON m.Id=mv.MinionId
                                    WHERE mv.VillainId=@Id
                                    ORDER BY m.Name";

            if (name == null)
            {
                Console.WriteLine($"No villain with ID {Id} exists in the database.");
            }

            else
            {
                Console.WriteLine($"Villain: {name}");
                using (var minionCommand = new SqlCommand(minionsQuery, connection))
                {
                    minionCommand.Parameters.AddWithValue("@Id", Id);
                    var reader = minionCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("(no minions)");
                        }

                        else
                        {
                            Console.WriteLine($"{reader[0]}. {reader[1]} {reader[2]}");
                        }

                    }
                }
            }
        }

        private static object ExecuteScalar(SqlConnection connection, string query)
        {
            using var command = new SqlCommand(query, connection);
            var result = command.ExecuteScalar();
            return result;
        }
    }
}
