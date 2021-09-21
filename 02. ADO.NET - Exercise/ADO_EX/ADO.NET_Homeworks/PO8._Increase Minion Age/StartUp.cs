using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Threading;

namespace PO8._Increase_Minion_Age
{
    public class StartUp
    {
        private const string ConnectionString =
        @"Server = (localdb)\MSSQLLocalDB;Database=MinionsDB;Integrated Security = true";
        public static void Main(string[] args)
        {

            int[] inputIds = Console.ReadLine().Split().Select(int.Parse).ToArray();

            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using (connection)
            {
                for (int i = 0; i < inputIds.Length; i++)
                {
                    string commandText = $"SELECT * FROM Minions WHERE Id = @Id";
                    var command = new SqlCommand(commandText, connection);
                    command.Parameters.AddWithValue("@Id", inputIds[i]);
                    var reader = command.ExecuteReader();
                    reader.Read();
                    string name = Convert.ToString(reader["Name"]);
                    reader.Close();

                    var cultureInfo = Thread.CurrentThread.CurrentCulture;
                    var textInfo = cultureInfo.TextInfo;
                    string convertedName = textInfo.ToTitleCase(name);

                    var updateCmd = $"UPDATE Minions SET Name = '{convertedName}', Age += 1 WHERE Id = {inputIds[i]}";
                    var updateCommand = new SqlCommand(updateCmd, connection);
                    updateCommand.ExecuteNonQuery();
                }

                string printQuery = "SELECT Name, Age FROM Minions";
                var printCommand = new SqlCommand(printQuery, connection);
                var printer = printCommand.ExecuteReader();
                while (printer.Read())
                {
                    string minionName = (string)printer["Name"];
                    int age = (int)printer["Age"];

                    Console.WriteLine($"{minionName} {age}");
                }
            }
        }
    }
}
