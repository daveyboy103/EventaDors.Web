using System;
using System.Data;
using System.Data.SqlClient;
using EventaDors.Entities.Classes;

namespace EventaDors.DataManagement
{
    public class Wrapper : IWrapper
    {
        private readonly string _connectionString;

        public Wrapper(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public LoginResult Authenticate(string userName, string password)
        {
            LoginResult loginResult = null;
            using (var cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                var cmd = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "dbo.LoginUser",
                    Connection = cn
                };

                cmd.Parameters.AddWithValue("UserName", userName);
                cmd.Parameters.AddWithValue("Password", password);

                var ret = cmd.ExecuteScalar();

                if (ret == null)
                {
                    loginResult = new LoginResult(
                        false,
                        "User not found",
                        null
                    );
                }
                else
                {
                    var userId = long.Parse(ret.ToString());

                    User u = CreateUser(userId);

                    if (userId == 0)
                    {
                        loginResult = new LoginResult(
                            false,
                            "Invalid username or password",
                            null);
                    }
                    else
                    {
                        loginResult = new LoginResult(
                            true,
                            "Success",
                            u);
                    }
                }
            }

            return loginResult;
        }

        public User CreateUser(long userId)
        {
            return new User("test", userId, DateTime.Now, DateTime.Now, Guid.NewGuid());
        }

        public bool DeleteUser(long userId, bool deactivateOnly)
        {
            return false;
        }

        public bool ChangePassword(long userId, string oldPassword, string newPassword)
        {
            return false;
        }

        public User UpdateUser(User user)
        {
            return User.Empty;
        }
    }

    public interface IWrapper
    {
        LoginResult Authenticate(string userName, string password);
        User CreateUser(long userId);
    }

    public class LoginResult 
    {
        public LoginResult(bool result, string notes, User user)
        {
            Result = result;
            Notes = notes;
            User = user;
        }

        public bool Result { get; }
        public string Notes { get; }
        public User User { get; }
    }
}