using System;
using Microsoft.Data.SqlClient;

namespace PO4._Add_Minion
{
    public class StartUp
    {
        private const string ConnectionString =
            @"Server = (localdb)\MSSQLLocalDB;Database=MinionsDB;Integrated Security = true";
        public static void Main(string[] args)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var minionInfo = Console.ReadLine().Split(' ');
            var villianInfo = Console.ReadLine().Split(' ');
            string minionName = minionInfo[1];
            int age = int.Parse(minionInfo[2]);
            string town = minionInfo[3];

            int? townId = GetTownId(connection, town);

            if (townId == null)
            {
                string createTownQuery = @"INSERT INTO Towns(Name) VALUES (@town)";
                using var sqlCommand = new SqlCommand(createTownQuery, connection);
                sqlCommand.Parameters.AddWithValue("@town", town);
                sqlCommand.ExecuteNonQuery();
                townId = GetTownId(connection, town);
                Console.WriteLine($"Town {town} was added to the database.");
            }

            string villianName = villianInfo[1];
            int? villianId = GetVillianName(connection, villianName);

            if (villianId == null)
            {
                string createVillianQuery = @"INSERT INTO Villains(EvilnessFactorId,Name) VALUES (4,@villianName)";
                using var sqlCommand = new SqlCommand(createVillianQuery, connection);
                sqlCommand.Parameters.AddWithValue("@villianName", villianName);
                sqlCommand.ExecuteNonQuery();
                townId = GetTownId(connection, town);
                villianId = GetVillianName(connection, villianName);
                Console.WriteLine($"Villain {villianName} was added to the database.");
            }

            CreateMinion(connection, minionName, age, townId);

            var minionId = GetMinionId(connection, minionName);

            InsertMinionVillian(connection, villianId, minionId);
            Console.WriteLine($"Successfully added {minionName} to be minion of {villianName}.");
        }

        private static void InsertMinionVillian(SqlConnection connection, int? villianId, int? minionId)
        {
            var insertIntoMinionVilion = @"INSERT INTO MinionsVillains (MinionId,VillainId) VALUES (@minionId,@villianid)";
            var sqlCommand = new SqlCommand(insertIntoMinionVilion, connection);
            sqlCommand.Parameters.AddWithValue("@minionId", minionId);
            sqlCommand.Parameters.AddWithValue("@villianid", villianId);
            sqlCommand.ExecuteNonQuery();
           
        }

        private static int? GetMinionId(SqlConnection connection, string minionName)
        {
            string query = @"SELECT Id FROM Minions WHERE Name=@Name";
            var sqlCommand = new SqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Name", minionName);
            var minionId = sqlCommand.ExecuteScalar();
            return (int?)minionId;
        }

        private static void CreateMinion(SqlConnection connection, string minionName, int age,int? townId)
        {
            string createMinionQuery = @"INSERT INTO Minions (Name,Age,TownId) VALUES(@name,@age,@townId)";
            using var sqlCommand = new SqlCommand(createMinionQuery, connection);
            sqlCommand.Parameters.AddWithValue("@name", minionName);
            sqlCommand.Parameters.AddWithValue("@age", age);
            sqlCommand.Parameters.AddWithValue("@townId", townId);
            sqlCommand.ExecuteNonQuery();
        }

        private static int? GetVillianName(SqlConnection connection, string villianName)
        {

            string query = @"SELECT Id FROM Villains WHERE Name=@Name";
            using var sqlCommand = new SqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Name", villianName);
            var villlianId = sqlCommand.ExecuteScalar();
            return (int?)villlianId;
        }

        private static int? GetTownId(SqlConnection connection, string town)
        {
            var townIdQuery = @"SELECT Id FROM Towns WHERE Name=@townName";
            using var sqlCommand = new SqlCommand(townIdQuery, connection);
            sqlCommand.Parameters.AddWithValue("@townName", town);
            var townId = sqlCommand.ExecuteScalar();
            return (int?)townId;
        }
    }
}
