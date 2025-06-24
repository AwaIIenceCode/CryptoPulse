using Microsoft.Data.SqlClient;
using System;

namespace CryptoPulse.Application.Features.Users
{
    public class UserRegistrationService
    {
        private readonly string _connectionString;

        public UserRegistrationService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public bool RegisterUser(string login, string email, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string createTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                        CREATE TABLE Users (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Login NVARCHAR(50) NOT NULL,
                            Email NVARCHAR(100) NOT NULL,
                            Password NVARCHAR(100) NOT NULL
                        )";
                    using (var command = new SqlCommand(createTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string checkLogin = "SELECT COUNT(*) FROM Users WHERE Login = @Login";
                    using (var command = new SqlCommand(checkLogin, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        int count = (int)command.ExecuteScalar();
                        if (count > 0)
                        {
                            return false; // Логин занят
                        }
                    }

                    string insertUser = "INSERT INTO Users (Login, Email, Password) VALUES (@Login, @Email, @Password)";
                    using (var command = new SqlCommand(insertUser, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password); // Позднее заменим на хэш
                        command.ExecuteNonQuery();
                    }

                    return true;
                }
                catch (SqlException)
                {
                    return false; // Ошибка БД
                }
            }
        }
    }
}