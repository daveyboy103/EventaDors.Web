using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EventaDors.Entities.Classes;
using EventaDors.Entities.Interfaces;

namespace EventaDors.DataManagement
{
    public class Wrapper : IWrapper
    {
        private const string ReturnParamName = "return";
        private readonly string _connectionString;

        public Wrapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public LoginResult Authenticate(string userName, string password)
        {
            LoginResult loginResult;
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
                    var userId = long.Parse(ret.ToString() ?? string.Empty);

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
            User ret = null;
            
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("USER_LoadUser"))
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("userId", userId);
                    
                    cn.Open();

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ret = new User(
                            dr.GetString(dr.GetOrdinal("UserName")),
                            dr.GetInt64(dr.GetOrdinal("id")),
                            dr.GetDateTime(dr.GetOrdinal("Created")),
                            dr.GetDateTime(dr.GetOrdinal("Modified")),
                            dr.GetGuid(dr.GetOrdinal("uuid"))
                        );

                        ret.Verified = dr.GetBoolean(dr.GetOrdinal("Verified"));
                        ret.CurrentPassword = dr.GetString(dr.GetOrdinal("Password"));

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                MetaDataItem item = new MetaDataItem
                                {
                                    Name = dr.GetString(dr.GetOrdinal("name")),
                                    Value = dr.GetString(dr.GetOrdinal("Value")),
                                    Type = Enum.Parse<MetaDataType>( dr.GetString(dr.GetOrdinal("Type")))
                                };
                                
                                ret.MetaData.Add(dr.GetString(dr.GetOrdinal("name")), item);
                            }
                        }
                    }
                }
            }

            return ret;
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
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("USER_CreateUser"))
                {
                    return null;
                }
            }
        }

        public QuoteRequest CreateRequestFromTemplate(int templateId, int userId, int attendees, DateTime dueDate)
        {
            QuoteRequest ret;
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
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.Int);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    var quoteId = (int) cmd.Parameters[ReturnParamName].Value;

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
                        var quoteRequestElement = new QuoteRequestElement
                        {
                            Type = new QuoteElementType
                            {
                                Name = dr.GetString(dr.GetOrdinal("QuoteElementType")),
                                Notes = GetSafeString(dr, "QuoteElementNotes"),
                                Id = dr.GetInt32(dr.GetOrdinal("QuoteRequestElementId"))
                            },
                            Id = dr.GetInt32(dr.GetOrdinal("QuoteRequestElementId")),
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
                            Completed = dr.GetBoolean(dr.GetOrdinal("Completed"))
                        };

                        ret?.Elements.Add(quoteRequestElement);
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
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.BigInt);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    var userId = long.Parse(cmd.Parameters[ReturnParamName].Value.ToString() ?? string.Empty);

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

        public QuoteRequestElementResponse PickupQuoteRequestItem(QuoteRequestElementResponse response)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("QUOTE_PickupQuoteRequestItem")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = cn
                })
                {
                    cmd.Parameters.AddWithValue("quoteRequestElementId", response.ParentElement.Id);
                    cmd.Parameters.AddWithValue("userId", response.Owner.Id);
                    cmd.Parameters.AddWithValue("accepted", response.Accepted);
                    cmd.Parameters.AddWithValue("amountLow", response.AmountLow);
                    cmd.Parameters.AddWithValue("amountHigh", response.AmountHigh);
                    cmd.Parameters.AddWithValue("notes", response.Notes);
                    cmd.Parameters.AddWithValue("link", response.Link);
                    cmd.Parameters.AddWithValue("estimate", response.Estimate);
                    
                    cn.Open();
                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        response.Accepted = dr.GetBoolean(dr.GetOrdinal("Accepted"));
                        response.Submitted = GetSafeDate(dr, "Submitted");
                        response.Created = dr.GetDateTime(dr.GetOrdinal("Created"));
                        response.Modified = dr.GetDateTime(dr.GetOrdinal("Modified"));
                        response.Owner.UserName = dr.GetString(dr.GetOrdinal("UserName"));
                        response.Owner.Uuid = dr.GetGuid(dr.GetOrdinal("UserUuid"));
                        response.Owner.Created = dr.GetDateTime(dr.GetOrdinal("UserCreated"));
                    }
                }
            }

            return response;
        }

        public bool AssignUserToQuoteElement(int userId, int quoteElementId, bool active)
        {
            try
            {
                using (var cn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("USER_AssignToQuoteElement")
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = cn
                    })
                    {
                        cmd.Parameters.AddWithValue("userId", userId);
                        cmd.Parameters.AddWithValue("quoteElementId", quoteElementId);
                        cmd.Parameters.AddWithValue("active", active);
                    
                        cn.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        public bool AssignUserToElementType(int userId, int quoteElementTypeId, bool active)
        {
            try
            {
                using (var cn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("USER_AssignToQuoteElementType")
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = cn
                    })
                    {
                        cmd.Parameters.AddWithValue("userId", userId);
                        cmd.Parameters.AddWithValue("quoteElementId", quoteElementTypeId);
                        cmd.Parameters.AddWithValue("active", active);
                    
                        cn.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        public QuoteElement AddUpdateQuoteElement(QuoteElement quoteElement)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("STATIC_AddUpdateQuoteElement"))
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("id", quoteElement.Id);
                    cmd.Parameters.AddWithValue("name", quoteElement.Name);
                    cmd.Parameters.AddWithValue("notes", quoteElement.Notes);
                    cmd.Parameters.AddWithValue("budgetTolerance", quoteElement.BudgetTolerance);
                    cmd.Parameters.AddWithValue("quantity", quoteElement.Quantity);
                    cmd.Parameters.AddWithValue("elementTypeId", quoteElement.Type.Id);
                    cmd.Parameters.AddWithValue("inheritTopLevelQuantity", quoteElement.InheritTopLevelQuantity);
                    cmd.Parameters.AddWithValue("leadWeeks", quoteElement.LeadWeeks);
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.Int);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters[ReturnParamName].DbType = DbType.Int32;
                    
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    int id = int.Parse(cmd.Parameters[ReturnParamName].Value.ToString() ?? string.Empty);

                    quoteElement = LoadQuoteElement(id);
                }
            }

            return quoteElement;
        }

        private QuoteElement LoadQuoteElement(int id)
        {
            QuoteElement ret = null;
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("STATIC_LoadQuoteElement"))
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", id);
                    cn.Open();

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ret = new QuoteElement
                        {
                            Name = dr.GetString(dr.GetOrdinal("Name")),
                            Notes = GetSafeString(dr,"Notes"),
                            BudgetTolerance = dr.GetDouble(dr.GetOrdinal("BudgetTolerance")),
                            Quantity = dr.GetInt32(dr.GetOrdinal("Quantity")),
                            LeadWeeks = dr.GetInt32(dr.GetOrdinal("LeadWeeks")),
                            InheritTopLevelQuantity = dr.GetBoolean(dr.GetOrdinal("InheritTopLevelQuantity")),
                            Type = new QuoteElementType()
                            {
                                Notes = GetSafeString(dr,"ElementTypeNotes"),
                                Name = dr.GetString(dr.GetOrdinal("ElementTypeName")),
                                Link = GetSafeString(dr,"ElementTypeLink"),
                                Id = dr.GetInt32(dr.GetOrdinal("ElementTypeId"))
                            }
                        };
                    }

                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            var metaData = new MetaDataItem
                            {
                                Name = dr.GetString(dr.GetOrdinal("Name")),
                                Value = dr.GetString(dr.GetOrdinal("Value")),
                                Type = Enum.Parse<MetaDataType>(dr.GetString(dr.GetOrdinal("Type"))),
                                Created = dr.GetDateTime(dr.GetOrdinal("Created")),
                                Modified = dr.GetDateTime(dr.GetOrdinal("Created")),
                                MandatoryWhenUsed = dr.GetBoolean(dr.GetOrdinal("MandatoryWhenUsed"))
                            };

                            ret.MetaData.Add(metaData.Name, metaData);
                        }
                    }
                }
            }

            return ret;
        }

        public QuoteElementType AddUpdateQuoteElementType(QuoteElementType quoteElementType)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("STATIC_AddUpdateQuoteElementType"))
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", quoteElementType.Id);
                    cmd.Parameters.AddWithValue("name", quoteElementType.Name);
                    cmd.Parameters.AddWithValue("notes", quoteElementType.Notes);
                    cmd.Parameters.AddWithValue("link", quoteElementType.Link);
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.Int);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;
                    
                    cn.Open();

                    cmd.ExecuteNonQuery();

                    int id = int.Parse(cmd.Parameters[ReturnParamName].Value.ToString());
                    quoteElementType.Id = id;
                }
            }

            return quoteElementType;
        }

        public QuoteType AddUpdateQuoteType(QuoteType quoteType)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("STATIC_AddUpdateQuoteType"))
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", quoteType.Id);
                    cmd.Parameters.AddWithValue("name", quoteType.Name);
                    cmd.Parameters.AddWithValue("notes", quoteType.Notes);
                    cmd.Parameters.AddWithValue("link", quoteType.Link);
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.Int);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;
                    
                    cn.Open();

                    cmd.ExecuteNonQuery();

                    int id = int.Parse(cmd.Parameters[ReturnParamName].Value.ToString());
                    quoteType.Id = id;
                }
            }

            return quoteType;
        }

        public QuoteSubType AddUpdateQuoteSubType(QuoteSubType quoteSubType)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("STATIC_AddUpdateQuoteSubType"))
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", quoteSubType.Id);
                    cmd.Parameters.AddWithValue("name", quoteSubType.Name);
                    cmd.Parameters.AddWithValue("notes", quoteSubType.Notes);
                    cmd.Parameters.AddWithValue("link", quoteSubType.Link);
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.Int);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;
                    
                    cn.Open();

                    cmd.ExecuteNonQuery();

                    int id = int.Parse(cmd.Parameters[ReturnParamName].Value.ToString());
                    quoteSubType.Id = id;
                }
            }

            return quoteSubType;
        }

        public QuoteTemplate AddUpdateQuoteTemplate(QuoteTemplate quoteTemplate)
        {
            throw new NotImplementedException();
        }

        public UserType AddUpdateUserType(UserType userType)
        {
            throw new NotImplementedException();
        }

        public long CreateNewUser(User user)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("USER_CreateNewUser")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = cn
                })
                {
                    cmd.Parameters.AddWithValue("userName", user.UserName);
                    cmd.Parameters.AddWithValue("email", user.PrimaryEmail);
                    cmd.Parameters.AddWithValue("password", user.CurrentPassword);
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.BigInt);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;
                    cn.Open();

                    cmd.ExecuteNonQuery();

                    long userId = long.Parse(cmd.Parameters[ReturnParamName].Value.ToString());

                    return userId;
                }
            }
        }

        public QuoteRequestElement AddQuoteElementToQuoteRequest(QuoteRequestElement quoteElement)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("STATIC_AddUpdateQuoteElementToQuoteRequest")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = cn
                })
                {
                    cmd.Parameters.AddWithValue("quoteId", quoteElement.QuoteId);
                    cmd.Parameters.AddWithValue("quoteElementId", quoteElement.Id);
                    cmd.Parameters.AddWithValue("quoteRequestElementId", quoteElement.QuoteRequestElementId);
                    cmd.Parameters.AddWithValue("budget", quoteElement.Budget);
                    cmd.Parameters.AddWithValue("budgetTolerance", quoteElement.BudgetTolerance);
                    cmd.Parameters.AddWithValue("quantity", quoteElement.Quantity);
                    cmd.Parameters.AddWithValue("exclude", quoteElement.Exclude);
                    cmd.Parameters.AddWithValue("notes", quoteElement.Notes);
                    cmd.Parameters.AddWithValue("leadWeeks", quoteElement.LeadWeeks);
                    cmd.Parameters.AddWithValue("DueDate", quoteElement.DueDate);
                    cmd.Parameters.AddWithValue("Completed", quoteElement.Completed);
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.BigInt);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;
                    cn.Open();

                    cmd.ExecuteNonQuery();
                    int id = int.Parse(cmd.Parameters[ReturnParamName].Value.ToString());
                }
            }

            return quoteElement;
        }

        public IEnumerable<User> ListUsers()
        {
            IList<User> ret = new List<User>();
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("STATIC_ListUsers"))
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cn.Open();

                    var dr = cmd.ExecuteReader();

                    
                    while (dr.Read())
                    {
                        User u = new User(
                            dr.GetString(dr.GetOrdinal("UserName")),
                            dr.GetInt64(dr.GetOrdinal("Id")),
                            dr.GetDateTime(dr.GetOrdinal("Created")),
                            dr.GetDateTime(dr.GetOrdinal("Modified")),
                            dr.GetGuid(dr.GetOrdinal("uuid")));
                        u.Verified = dr.GetBoolean(dr.GetOrdinal("Verified"));
                        ret.Add(u);
                    }
                }
            }

            return ret;
        }
    }
}