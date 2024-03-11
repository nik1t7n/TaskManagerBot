using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using TelegramBot.Entities;

namespace TelegramBot.Data
{
    public class DataContext
    {
        private readonly string _connectionString;

        public DataContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");

            _connectionString = connectionString;
        }

        public IEnumerable<Challenge> GetAllChallenges()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<Challenge>("SELECT * FROM Challenges");
            }
        }

        public Challenge GetChallengeById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.QueryFirstOrDefault<Challenge>("SELECT * FROM Challenges WHERE Id = @Id", new { Id = id });
                if (result == null)
                {
                    throw new ArgumentNullException("Challenge object cannot be null.");
                }
                else
                {
                    return result;
                }
            }
        }

        public void AddChallenge(Challenge challenge)
        {
            if (challenge == null)
                throw new ArgumentNullException(nameof(challenge), "Challenge object cannot be null.");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("INSERT INTO Challenges (Description, CreatedAt, Deadline) VALUES (@Description, @CreatedAt, @Deadline)", challenge);
            }
        }

        public void UpdateChallenge(Challenge challenge)
        {
            if (challenge == null)
                throw new ArgumentNullException(nameof(challenge), "Challenge object cannot be null.");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("UPDATE Challenges SET Description = @Description, CreatedAt = @CreatedAt, Deadline = @Deadline WHERE Id = @Id", challenge);
            }
        }

        public void DeleteChallenge(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("DELETE FROM Challenges WHERE Id = @Id", new { Id = id });
            }
        }

        // Другие методы для работы с базой данных...
    }
}
