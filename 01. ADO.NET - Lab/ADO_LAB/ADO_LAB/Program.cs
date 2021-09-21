using System;
using Microsoft.Data.SqlClient;

namespace ADO_LAB
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string connectionString = @"Server = (localdb)\MSSQLLocalDB; Database = SoftUni; Integrated Security = true;";
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();
                var query = "SELECT * FROM Employees";
                var sqlCommand = new SqlCommand(query, connection);

                var sqlData = sqlCommand.ExecuteReader();
                sqlData.Read();
                
                Console.WriteLine(sqlData[1]);
            }


            

        }
    }
}
