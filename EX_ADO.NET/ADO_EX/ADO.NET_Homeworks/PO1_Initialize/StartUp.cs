using System;
using Microsoft.Data.SqlClient;

namespace ADO_EX
{
    public class StartUp
    {
        const string SqlConnectionString =
            @"Server = (localdb)\MSSQLLocalDB;Database=master;Integrated Security = true";
       public static void Main(string[] args)
        {
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                string createDataBase = "CREATE DATABASE MinionsDB";
                using (var command = new SqlCommand(createDataBase, connection))
                {
                    command.ExecuteNonQuery();
                }

                var createTableStatements = GetCreateTableStatements();

                foreach (var query in createTableStatements)
                {
                    ExecuteNonQuery(connection, query);
                }

                var insertStatements = InsertData();

                foreach (var query in insertStatements)
                {
                    ExecuteNonQuery(connection, query);

                }
            }
        }

        private static void ExecuteNonQuery(SqlConnection connection,string query)
        {
            using (var command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static string[] InsertData()
        {
            var result = new string[]
            {
                "INSERT INTO Countries VALUES(1,'Bulgaria'),(2,'Greece'),(3,'Norway'),(4,'UK'),(5,'Cyprus')",
                "INSERT INTO Towns VALUES(1,'Plovdiv',1),(2,'Oslo',2),(3,'Larnaka',3),(4,'Athens',4),(5,'London',5)",
                "INSERT INTO Minions VALUES(1,'Stoyan',12,1),(2,'George',22,2),(3,'Ivan',25,3),(4,'Kiro',30,4),(5,'Mimi',5,5)",
                "INSERT INTO EvilnessFactors VALUES(1,'super good'),(2,'good'),(3,'bad'),(4,'evil'),(5,'super evil')",
                "INSERT INTO VillainsVALUES(1,'Gru',2),(2,'Ivo',3),(3,'Teo',4),(4,'Sto',5),(5,'Pro',1)",
                "INSERT INTO MinionsVillains VALUES(1,1),(2,2),(3,3),(4,4),(5,5)"
            };

            return result;
        }

        private static string[] GetCreateTableStatements()
        {
            var result = new string[]
            {
                "CREATE TABLE Countries(Id INT PRIMARY KEY, [Name] VARCHAR(50))",
                "CREATE TABLE Towns( Id INT PRIMARY KEY, [Name] VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))",
                "CREATE TABLE Minions(Id INT PRIMARY KEY, [Name] VARCHAR(50), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))",
                "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY, [Name] VARCHAR(50))",
                "CREATE TABLE Villains(Id INT PRIMARY KEY, [Name] VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))",
                "CREATE TABLE MinionsVillains(MinionId INT FOREIGN KEY REFERENCES Minions(Id), VillainId INT FOREIGN KEY REFERENCES Villains(Id), PRIMARY KEY(MinionId,VillainId))"

            };

            return result;
        }
    }
}
