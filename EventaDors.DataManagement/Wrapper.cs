using System;
using System.Collections.Generic;
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
                var cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "dbo.USER_LoginUser",
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

                    var u = CreateUser(userId);

                    if (userId == 0)
                        loginResult = new LoginResult(
                            false,
                            "Invalid username or password",
                            null);
                    else
                        loginResult = new LoginResult(
                            true,
                            "Success",
                            u);
                }
            }

            return loginResult;
        }

        public User CreateUser(long userId)
        {
            return new("test", userId, DateTime.Now, DateTime.Now, Guid.NewGuid());
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

        public QuoteRequest CreateRequestFromTemplate(int templateId, int userId, int attendees, DateTime dueDate)
        {
            QuoteRequest ret = null;
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("dbo.QUOTE_CreateFromTemplate")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = cn
                })
                {
                    cmd.Parameters.AddWithValue("TemplateId", templateId);
                    cmd.Parameters.AddWithValue("Attendees", attendees);
                    cmd.Parameters.AddWithValue("OwnerId", userId);
                    cmd.Parameters.AddWithValue("DueDate", dueDate);
                    cmd.Parameters.Add("return", SqlDbType.Int);
                    cmd.Parameters["return"].Direction = ParameterDirection.ReturnValue;
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    var quoteId = (int) cmd.Parameters["return"].Value;

                    ret = LoadQuoteRequest(cn, quoteId);
                }
            }

            return ret;
        }

        public QuoteRequest LoadQuoteRequest(int quoteIdIdentity)
        {
            var ret = LoadQuoteRequest(new SqlConnection(_connectionString), quoteIdIdentity);
            return ret;
        }

        private QuoteRequest LoadQuoteRequest(SqlConnection cn, int quoteId)
        {
            QuoteRequest ret = null;

            if (cn.State == ConnectionState.Closed)
                cn.Open();

            using (var cmdFetch = new SqlCommand("QUOTE_LoadQuote")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = cn
            })
            {
                cmdFetch.Parameters.AddWithValue("QuoteIdIdentity", quoteId);

                var dr = cmdFetch.ExecuteReader();

                while (dr.Read())
                    ret = new QuoteRequest
                    {
                        QuoteId = dr.GetGuid(dr.GetOrdinal("QuoteId")),
                        QuoteIdIdentity = quoteId,
                        Name = dr.GetString(dr.GetOrdinal("Name")),
                        Notes = GetSafeString(dr, "Notes"),
                        DueDate = GetSafeDate(dr, "DueDate"),
                        Created = dr.GetDateTime(dr.GetOrdinal("Created")),
                        Modified = dr.GetDateTime(dr.GetOrdinal("Modified")),
                        Type = new QuoteType
                        {
                            Name = dr.GetString(dr.GetOrdinal("QuoteTypeName")),
                            Notes = GetSafeString(dr, "QuoteTypeNotes"),
                            Link = GetSafeString(dr, "QuoteTypeLink")
                        },
                        SubType = new QuoteSubType
                        {
                            Name = dr.GetString(dr.GetOrdinal("QuoteSubTypeName")),
                            Notes = GetSafeString(dr, "QuoteSubTypeNotes"),
                            Link = GetSafeString(dr, "QuoteSubTypeLink")
                        }
                    };

                if (dr.NextResult())
                    while (dr.Read())
                    {
                        var quoteRequestElement = new QuoteElement
                        {
                            Type = new QuoteElementType
                            {
                                Name = dr.GetString(dr.GetOrdinal("QuoteElementType")),
                                Notes = GetSafeString(dr, "QuoteElementNotes"),
                                Id = dr.GetInt32(dr.GetOrdinal("QuoteRequestElementId"))
                            },
                            Id = dr.GetInt32(dr.GetOrdinal("Id")),
                            Name = dr.GetString(dr.GetOrdinal("QuoteElementType")),
                            Quantity = dr.GetInt32(dr.GetOrdinal("Quantity")),
                            Budget = GetSafeDbl(dr, "Budget"),
                            BudgetTolerance = GetSafeDbl(dr, "BudgetTolerance"),
                            Created = dr.GetDateTime(dr.GetOrdinal("QuoteRequestElementCreated")),
                            Modified = dr.GetDateTime(dr.GetOrdinal("QuoteRequestElementCreated")),
                            Exclude = dr.GetBoolean(dr.GetOrdinal("QuoteRequestElementExclude")),
                            Submitted = GetSafeDate(dr, "QuoteRequestElementSubmitted"),
                            DueDate = GetSafeDate(dr, "DueDate"),
                            LeadWeeks = GetSafeInt(dr, "LeadWeeks"),
                            Completed = dr.GetBoolean(dr.GetOrdinal("Completed")),
                        };

                        ret.Elements.Add(quoteRequestElement);
                    }
            }

            return ret;
        }

        private int? GetSafeInt(SqlDataReader dr, string fieldName)
        {
            if (dr[fieldName] != DBNull.Value)
                return dr.GetInt32(dr.GetOrdinal(fieldName));
            return null;
        }

        private DateTime? GetSafeDate(SqlDataReader dr, string fieldName)
        {
            if (dr[fieldName] != DBNull.Value)
                return dr.GetDateTime(dr.GetOrdinal(fieldName));
            return null;
        }

        private double? GetSafeDbl(SqlDataReader dr, string fieldName)
        {
            if (dr[fieldName] != DBNull.Value)
                return dr.GetDouble(dr.GetOrdinal(fieldName));
            return null;
        }

        private static string GetSafeString(SqlDataReader dr, string fieldName)
        {
            if (dr[fieldName] != DBNull.Value)
                return dr.GetString(dr.GetOrdinal(fieldName));
            return null;
        }

        public User RegisterUser(User user)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("USER_RegisterUser")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = cn
                })
                {
                    cmd.Parameters.AddWithValue("UserName", user.UserName);
                    cmd.Parameters.AddWithValue("Email", user.PrimaryEmail);
                    cmd.Parameters.AddWithValue("Password", user.CurrentPassword);
                    cmd.Parameters.Add("return", SqlDbType.BigInt);
                    cmd.Parameters["return"].Direction = ParameterDirection.ReturnValue;
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    long userId = long.Parse(cmd.Parameters["return"].Value.ToString());

                    using (var cmdFetch = new SqlCommand("USER_GetUser")
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = cn
                    })
                    {
                        cmdFetch.Parameters.AddWithValue("userId", userId);

                        var dr = cmdFetch.ExecuteReader();

                        while (dr.Read())
                        {
                            user.Id = dr.GetInt64(dr.GetOrdinal("id"));
                            user.Uuid = dr.GetGuid(dr.GetOrdinal("uuid"));
                            user.Created = dr.GetDateTime(dr.GetOrdinal("Created"));
                            user.Modified = dr.GetDateTime(dr.GetOrdinal("Modified"));
                        }
                    }
                }
            }
            
            return user;
        }

        public bool VerifyUser(Guid guid)
        {
            try
            {
                using (var cn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("USER_VerfyAccount")
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = cn
                    })
                    {
                        cmd.Parameters.AddWithValue("uuid", guid);
                        cn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool BlockUser(int userId, int blockedUserId)
        {
            return false;
        }

        public bool UnblockUser(int userId, int blockedUserId)
        {
            return false;
        }

        public int GetUserTokenBalance(int userId)
        {
            throw new NotImplementedException();
        }

        public IList<Deadline> GetDeadlines(int quoteIdIdentity, int alarmThreshold)
        {
            var ret = new List<Deadline>();
            
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("QUOTE_GetDeadline")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = cn
                })
                {
                    cmd.Parameters.AddWithValue("QuoteIdIdentity", quoteIdIdentity);
                    cmd.Parameters.AddWithValue("AlarmThreshold", alarmThreshold);
                    
                    cn.Open();

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        var deadline = new Deadline
                        {
                            Name = dr.GetString(dr.GetOrdinal("Name")),
                            DueDate = GetSafeDate(dr, "DueDate"),
                            Weeks = GetSafeInt(dr, "In Weeks"),
                            Status = dr.GetString(dr.GetOrdinal("Status")),
                            QuoteRequestElementId = dr.GetInt32(dr.GetOrdinal("QuoteRequestElementId"))
                        };
                        
                        ret.Add(deadline);
                    }
                }
            }

            return ret;
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