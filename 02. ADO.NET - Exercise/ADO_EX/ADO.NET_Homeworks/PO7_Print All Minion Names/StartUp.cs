using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace PO7_7._Print_All_Minion_Names
{
    public class StartUp
    {
        private const string ConnectionString =
         @"Server = (localdb)\MSSQLLocalDB;Database=MinionsDB;Integrated Security = true";

        public static void Main(string[] args)
        {
            var listMinions = new List<string>();
            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using (connection)
            {
                string cmdText = "SELECT Name FROM Minions";
                var command = new SqlCommand(cmdText, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string name = (string)reader["Name"];
                    listMinions.Add(name);
                }

                reader.Close();

                int count = listMinions.Count;
                int loopEnd = count / 2;

                for (int i = 0; i < loopEnd; i++)
                {
                    Console.WriteLine(listMinions[i]);
                    Console.WriteLine(listMinions[count - 1 - i]);
                }

                if (count % 2 == 1)
                {
                    Console.WriteLine(listMinions[count / 2]);
                }
            }
        }
    }
}
