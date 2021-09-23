using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using EventaDors.Entities.Classes;
using EventaDors.Entities.Interfaces;

namespace EventaDors.DataManagement
{
    /// <summary>
    /// Wrapper class for data access to SQl Database
    /// </summary>
    public class Wrapper : IWrapper
    {
        private const string ReturnParamName = "return";
        private const string EmailAddress = "emailAddress";
        private const string EventDate = "eventDate";
        private readonly string _connectionString;

        public Wrapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Journey GetJourney(string emailAddress)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "JOURNEY_GetJourney"))
                {
                    cmd.Parameters.AddWithValue(EmailAddress, emailAddress);
                    return new Journey(cmd.ExecuteReader());
                }
            }
        }

        public bool PutJourney(Journey journey)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "JOURNEY_PutJourney"))
                {
                    cmd.Parameters.AddWithValue(EmailAddress, journey.Email);
                    cmd.Parameters.AddWithValue(EventDate, journey.EventDate);
                    cmd.Parameters.AddWithValue("firstName", journey.FirstName);
                    cmd.Parameters.AddWithValue("surname", journey.Surname);
                    cmd.Parameters.AddWithValue("title", journey.Title);
                    cmd.Parameters.AddWithValue("postalCode", journey.PostalCode);
                    cmd.Parameters.AddWithValue("informPartner", journey.InformPartner);
                    cmd.Parameters.AddWithValue("partnerEmail", journey.PartnerEmail);
                    cmd.Parameters.AddWithValue("currentPage", journey.CurrentPage);
                    cmd.Parameters.AddWithValue("contactNumber", journey.ContactNumber);
                    cmd.Parameters.AddWithValue("yourStory", journey.YourStory);
                    cmd.Parameters.AddWithValue("registered", journey.Registered);
                    cmd.Parameters.AddWithValue("password", journey.Password);
                    cmd.Parameters.AddWithValue("completed", journey.Completed);

                    if (cmd.ExecuteNonQuery() != 0)
                        return true;
                    return false;
                }
            }
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
                        ret.UserKey = dr.GetGuid(dr.GetOrdinal("UserKey"));
                        ret.CurrentPassword = dr.GetString(dr.GetOrdinal("Password"));

                        if (dr.NextResult())
                            while (dr.Read())
                            {
                                var item = new MetaDataItem
                                {
                                    Name = dr.GetString(dr.GetOrdinal("name")),
                                    Value = dr.GetString(dr.GetOrdinal("Value")),
                                    Type = Enum.Parse<MetaDataType>(dr.GetString(dr.GetOrdinal("Type")))
                                };

                                ret.MetaData.Add(dr.GetString(dr.GetOrdinal("name")), item);
                            }
                    }
                }
            }

            return ret;
        }

        public User CreateUser(string emailAddress)
        {
            User ret = null;

            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("USER_LoadUserFromEmail"))
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(EmailAddress, emailAddress);

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
                        ret.UserKey = dr.GetGuid(dr.GetOrdinal("UserKey"));
                        ret.CurrentPassword = dr.GetString(dr.GetOrdinal("Password"));

                        if (dr.NextResult())
                            while (dr.Read())
                            {
                                var item = new MetaDataItem
                                {
                                    Name = dr.GetString(dr.GetOrdinal("name")),
                                    Value = dr.GetString(dr.GetOrdinal("Value")),
                                    Type = Enum.Parse<MetaDataType>(dr.GetString(dr.GetOrdinal("Type")))
                                };

                                ret.MetaData.Add(dr.GetString(dr.GetOrdinal("name")), item);
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

        public QuoteRequest CreateRequestFromTemplate(int templateId, long userId, int attendees, DateTime? dueDate)
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
                    if (dueDate != null) cmd.Parameters.AddWithValue("DueDate", dueDate.Value);
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

            using (var cmdFetch = GetCommand(cn, "QUOTE_LoadQuote"))
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
                        Attendees = dr.GetInt32(dr.GetOrdinal("Attendees")),
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
                        },
                        Owner = new User
                        {
                            Id = dr.GetInt64(dr.GetOrdinal("UserId")),
                            UserName = dr.GetString(dr.GetOrdinal("UserName")),
                            PrimaryEmail = dr.GetString(dr.GetOrdinal("UserEmail")),
                            Uuid = dr.GetGuid(dr.GetOrdinal("UserUuid")),
                            UserKey = dr.GetGuid(dr.GetOrdinal("UserKey"))
                        }
                    };

                if (dr.NextResult())
                    while (dr.Read())
                    {
                        var quoteRequestEvent = new QuoteRequestEvent
                        {
                            Id = dr.GetInt32(dr.GetOrdinal("EventId")),
                            QuoteId = dr.GetGuid(dr.GetOrdinal("QuoteId")),
                            Attendees = GetSafeInt(dr, "Attendees"),
                            Created = dr.GetDateTime("Created"),
                            Modified = dr.GetDateTime("Modified"),
                            Notes = GetSafeString(dr, "QuoteEventNotes"),
                            Event = new Event
                            {
                                Name = dr.GetString(dr.GetOrdinal("EventName")),
                                Notes = GetSafeString(dr, "EventNotes"),
                                Created = dr.GetDateTime("EventCreated"),
                                Modified = dr.GetDateTime("EventModified"),
                                Link = GetSafeString(dr, "EventLink")
                            },
                            EventDate = dr.GetDateTime("EventDate"),
                            Exclude = dr.GetBoolean(dr.GetOrdinal("Exclude")),
                            LeadWeeks = GetSafeInt(dr, "LeadWeeks"),
                            Order = dr.GetInt32(dr.GetOrdinal("EventOrder")),
                            Name = GetSafeString(dr, "DisplayName"),
                            QuoteRequestEventId = dr.GetInt32(dr.GetOrdinal("QuoteRequestEventId"))
                        };

                        if (dr["VenueId"] != DBNull.Value)
                        {
                            var venue = new Venue
                            {
                                Id = dr.GetInt32(dr.GetOrdinal("VenueId")),
                                Name = GetSafeString(dr, "VenueName"),
                                Address1 = GetSafeString(dr, "Address1"),
                                Address2 = GetSafeString(dr, "Address2"),
                                Address3 = GetSafeString(dr, "Address3"),
                                PostTown = GetSafeString(dr, "PostTown"),
                                PostCode = GetSafeString(dr, "PostCode"),
                                Country = GetSafeString(dr, "Country"),
                                ContactNumber = GetSafeString(dr, "ContactNumber"),
                                MapLink = GetSafeString(dr, "MapLink"),
                                SiteLink = GetSafeString(dr, "SiteLink"),
                                Created = dr.GetDateTime(dr.GetOrdinal("Created")),
                                Modified = dr.GetDateTime(dr.GetOrdinal("Modified"))
                                
                            };

                            quoteRequestEvent.Venue = venue;
                        }

                        ret?.Events.Add(quoteRequestEvent);
                    }

                if (dr.NextResult())
                    while (dr.Read())
                    {
                        if (ret != null)
                        {
                            var quoteRequestElement = new QuoteRequestElement
                            {
                                Id = dr.GetInt32(dr.GetOrdinal("Id")),
                                Parent = ret.Events.First(x => x.Id == dr.GetInt32(dr.GetOrdinal("EventId"))),
                                Name = dr.GetString(dr.GetOrdinal("Name")),
                                Notes = GetSafeString(dr, "Notes"),
                                Budget = (decimal) dr.GetSqlMoney(dr.GetOrdinal("Budget")),
                                BudgetTolerance = GetSafeDbl(dr, "BudgetTolerance"),
                                Completed = dr.GetBoolean("Completed"),
                                Quantity = dr.GetInt32(dr.GetOrdinal("Quantity")),
                                Created = dr.GetDateTime(dr.GetOrdinal("Created")),
                                Modified = dr.GetDateTime(dr.GetOrdinal("Modified")),
                                DueDate = GetSafeDate(dr, "DueDate"),
                                Exclude = dr.GetBoolean(dr.GetOrdinal("Exclude")),
                                UnderlyingElementNotes = GetSafeString(dr, "QuoteElementNotes"),
                                QuoteRequestElementId = GetSafeInt(dr, "QuoteRequestElementId")
                            };

                            quoteRequestElement.Parent.Elements.Add(quoteRequestElement);
                        }
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

        private decimal? GetSafeDecimal(SqlDataReader dr, string fieldName)
        {
            if (dr[fieldName] != DBNull.Value)
                return dr.GetDecimal(dr.GetOrdinal(fieldName));
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
                using (var cmd = GetCommand(cn, "USER_RegisterUser"))
                {
                    cmd.Parameters.AddWithValue("UserName", user.UserName);
                    cmd.Parameters.AddWithValue("Email", user.PrimaryEmail);
                    cmd.Parameters.AddWithValue("Password", user.CurrentPassword);
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.BigInt);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;

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

        public User VerifyUser(Guid guid)
        {
            User ret = null;
            try
            {
                using (var cn = new SqlConnection(_connectionString))
                {
                    using (var cmd = GetCommand(cn, "USER_VerfyAccount"))
                    {
                        cmd.Parameters.AddWithValue("uuid", guid);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd1  = GetCommand(cn ,"USER_GetUserByGuid"))
                    {
                        cmd1.Parameters.AddWithValue("uuid", guid);

                        var dr = cmd1.ExecuteReader();
                        
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
                            ret.UserKey = dr.GetGuid(dr.GetOrdinal("UserKey"));
                            ret.CurrentPassword = dr.GetString(dr.GetOrdinal("Password"));

                            if (dr.NextResult())
                                while (dr.Read())
                                {
                                    var item = new MetaDataItem
                                    {
                                        Name = dr.GetString(dr.GetOrdinal("name")),
                                        Value = dr.GetString(dr.GetOrdinal("Value")),
                                        Type = Enum.Parse<MetaDataType>(dr.GetString(dr.GetOrdinal("Type")))
                                    };

                                    ret.MetaData.Add(dr.GetString(dr.GetOrdinal("name")), item);
                                }
                        }
                    }
                }

                return ret;
            }
            catch (Exception)
            {
                return null;
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
                using (var cmd = GetCommand(cn, "QUOTE_GetDeadline"))
                {
                    cmd.Parameters.AddWithValue("QuoteIdIdentity", quoteIdIdentity);
                    cmd.Parameters.AddWithValue("AlarmThreshold", alarmThreshold);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        var nameOrdinal = dr.GetOrdinal("Name");
                        var statusOrdinal = dr.GetOrdinal("Status");
                        var quoteRequestElementIdOrdinal = dr.GetOrdinal("QuoteRequestElementId");
                        var requestsOrdinal = dr.GetOrdinal("Responses");
                        var chatsOrdinal = dr.GetOrdinal("Chats");

                        var deadline = new Deadline
                        {
                            Name = dr.GetString(nameOrdinal),
                            DueDate = GetSafeDate(dr, "DueDate"),
                            Weeks = GetSafeInt(dr, "In Weeks"),
                            Status = dr.GetString(statusOrdinal),
                            QuoteRequestElementId = dr.GetInt32(quoteRequestElementIdOrdinal),
                            Responses = dr.GetInt32(requestsOrdinal),
                            Submitted = GetSafeDate(dr, "Submitted"),
                            Chats = dr.GetInt32(chatsOrdinal)
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
                using (var cmd = GetCommand(cn, "QUOTE_PickupQuoteRequestItem"))
                {
                    cmd.Parameters.AddWithValue("quoteRequestElementId", response.ParentElement.Id);
                    cmd.Parameters.AddWithValue("userId", response.Owner.Id);
                    cmd.Parameters.AddWithValue("accepted", response.Accepted);
                    cmd.Parameters.AddWithValue("amountLow", response.AmountLow);
                    cmd.Parameters.AddWithValue("amountHigh", response.AmountHigh);
                    cmd.Parameters.AddWithValue("notes", response.Notes);
                    cmd.Parameters.AddWithValue("link", response.Link);
                    cmd.Parameters.AddWithValue("estimate", response.Estimate);

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
                    using (var cmd = GetCommand(cn, "USER_AssignToQuoteElement"))
                    {
                        cmd.Parameters.AddWithValue("userId", userId);
                        cmd.Parameters.AddWithValue("quoteElementId", quoteElementId);
                        cmd.Parameters.AddWithValue("active", active);

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
                    using (var cmd = GetCommand(cn, "USER_AssignToQuoteElementType"))
                    {
                        cmd.Parameters.AddWithValue("userId", userId);
                        cmd.Parameters.AddWithValue("quoteElementId", quoteElementTypeId);
                        cmd.Parameters.AddWithValue("active", active);

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
                using (var cmd = GetCommand(cn, "STATIC_AddUpdateQuoteElement"))
                {
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

                    cmd.ExecuteNonQuery();

                    var id = int.Parse(cmd.Parameters[ReturnParamName].Value.ToString() ?? string.Empty);

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
                using (var cmd = GetCommand(cn, "STATIC_LoadQuoteElement"))
                {
                    cmd.Parameters.AddWithValue("id", id);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                        ret = new QuoteElement
                        {
                            Name = dr.GetString(dr.GetOrdinal("Name")),
                            Notes = GetSafeString(dr, "Notes"),
                            BudgetTolerance = dr.GetDouble(dr.GetOrdinal("BudgetTolerance")),
                            Quantity = dr.GetInt32(dr.GetOrdinal("Quantity")),
                            LeadWeeks = dr.GetInt32(dr.GetOrdinal("LeadWeeks")),
                            InheritTopLevelQuantity = dr.GetBoolean(dr.GetOrdinal("InheritTopLevelQuantity")),
                            Type = new QuoteElementType()
                            {
                                Notes = GetSafeString(dr, "ElementTypeNotes"),
                                Name = dr.GetString(dr.GetOrdinal("ElementTypeName")),
                                Link = GetSafeString(dr, "ElementTypeLink"),
                                Id = dr.GetInt32(dr.GetOrdinal("ElementTypeId"))
                            }
                        };

                    if (dr.NextResult())
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

                            ret?.MetaData.Add(metaData.Name, metaData);
                        }
                }
            }

            return ret;
        }

        public QuoteElementType AddUpdateQuoteElementType(QuoteElementType quoteElementType)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "STATIC_AddUpdateQuoteElementType"))
                {
                    cmd.Parameters.AddWithValue("id", quoteElementType.Id);
                    cmd.Parameters.AddWithValue("name", quoteElementType.Name);
                    cmd.Parameters.AddWithValue("notes", quoteElementType.Notes);
                    cmd.Parameters.AddWithValue("link", quoteElementType.Link);
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.Int);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    var id = int.Parse(cmd.Parameters[ReturnParamName].Value.ToString() ?? string.Empty);
                    quoteElementType.Id = id;
                }
            }

            return quoteElementType;
        }

        public QuoteType AddUpdateQuoteType(QuoteType quoteType)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "STATIC_AddUpdateQuoteType"))
                {
                    cmd.Parameters.AddWithValue("id", quoteType.Id);
                    cmd.Parameters.AddWithValue("name", quoteType.Name);
                    cmd.Parameters.AddWithValue("notes", quoteType.Notes);
                    cmd.Parameters.AddWithValue("link", quoteType.Link);
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.Int);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    var id = int.Parse(cmd.Parameters[ReturnParamName].Value.ToString() ?? throw new InvalidOperationException());
                    quoteType.Id = id;
                }
            }

            return quoteType;
        }

        public QuoteSubType AddUpdateQuoteSubType(QuoteSubType quoteSubType)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "STATIC_AddUpdateQuoteSubType"))
                {
                    cmd.Parameters.AddWithValue("id", quoteSubType.Id);
                    cmd.Parameters.AddWithValue("name", quoteSubType.Name);
                    cmd.Parameters.AddWithValue("notes", quoteSubType.Notes);
                    cmd.Parameters.AddWithValue("link", quoteSubType.Link);
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.Int);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    var id = int.Parse(cmd.Parameters[ReturnParamName].Value.ToString());
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
                using (var cmd = GetCommand(cn, "USER_CreateNewUser"))
                {
                    cmd.Parameters.AddWithValue("userName", user.UserName);
                    cmd.Parameters.AddWithValue("email", user.PrimaryEmail);
                    cmd.Parameters.AddWithValue("password", user.CurrentPassword);
                    cmd.Parameters.Add(ReturnParamName, SqlDbType.BigInt);
                    cmd.Parameters[ReturnParamName].Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    var userId = long.Parse(cmd.Parameters[ReturnParamName].Value.ToString());

                    return userId;
                }
            }
        }

        public QuoteRequestElement AddQuoteElementToQuoteRequest(QuoteRequestElement quoteElement)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "STATIC_AddUpdateQuoteElementToQuoteRequest"))
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
                    var id = int.Parse(cmd.Parameters[ReturnParamName].Value.ToString());
                }
            }

            return quoteElement;
        }

        public IEnumerable<User> ListUsers()
        {
            IList<User> ret = new List<User>();
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "STATIC_ListUsers"))
                {
                    var dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        var u = new User(
                            dr.GetString(dr.GetOrdinal("UserName")),
                            dr.GetInt64(dr.GetOrdinal("Id")),
                            dr.GetDateTime(dr.GetOrdinal("Created")),
                            dr.GetDateTime(dr.GetOrdinal("Modified")),
                            dr.GetGuid(dr.GetOrdinal("uuid")));
                        u.Verified = dr.GetBoolean(dr.GetOrdinal("Verified"));
                        u.EventCount = dr.GetInt32(dr.GetOrdinal("Events"));
                        ret.Add(u);
                    }
                }
            }

            return ret;
        }

        private SqlCommand GetCommand(SqlConnection cn, string commandText)
        {
            var cmd = new SqlCommand(commandText)
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure
            };

            if (cn.State == ConnectionState.Closed)
                cn.Open();

            return cmd;
        }

        public IList<QuoteRequest> GetRequestsForUser(int userId)
        {
            IList<QuoteRequest> ret = new List<QuoteRequest>();

            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "STATIC_ListQuoteRequestsForUser"))
                {
                    cmd.Parameters.AddWithValue("userId", userId);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        var qr = new QuoteRequest
                        {
                            QuoteId = dr.GetGuid(dr.GetOrdinal("QuoteId")),
                            Created = dr.GetDateTime(dr.GetOrdinal("Created")),
                            Modified = dr.GetDateTime(dr.GetOrdinal("Modified")),
                            Notes = GetSafeString(dr, "Notes"),
                            Name = GetSafeString(dr, "Name"),
                            Attendees = dr.GetInt32(dr.GetOrdinal("Attendees")),
                            QuoteIdIdentity = dr.GetInt32(dr.GetOrdinal("QuoteidIdentity")),
                            DueDate = GetSafeDate(dr, "DueDate"),
                            Type = new QuoteType()
                            {
                                Id = dr.GetInt32(dr.GetOrdinal("QuoteTypeId")),
                                Name = GetSafeString(dr, "QuoteTypeNotes"),
                                Link = GetSafeString(dr, "QuoteTypeLink")
                            },
                            SubType = new QuoteSubType
                            {
                                Id = dr.GetInt32(dr.GetOrdinal("QuoteSubTypeId")),
                                Name = GetSafeString(dr, "QuoteSubTypeNotes"),
                                Link = GetSafeString(dr, "QuoteSubTypeLink")
                            },
                            Owner = new User
                            {
                                Id = dr.GetInt64(dr.GetOrdinal("UserId")),
                                UserName = dr.GetString(dr.GetOrdinal("UserName")),
                                PrimaryEmail = GetSafeString(dr, "Email"),
                                Verified = dr.GetBoolean(dr.GetOrdinal("Verified")),
                                Uuid = dr.GetGuid(dr.GetOrdinal("uuid"))
                            }
                        };

                        ret.Add(qr);
                    }
                }
            }

            return ret;
        }

        public QuoteRequestElement GetQuoteRequestElement(int quoteRequestElementid)
        {
            QuoteRequestElement ret = null;

            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "QUOTE_LoadQuoteRequestElement"))
                {
                    cmd.Parameters.AddWithValue("quoteRequestElementId", quoteRequestElementid);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                        ret = new QuoteRequestElement
                        {
                            Budget = GetSafeDecimal(dr, "Budget"),
                            BudgetTolerance = GetSafeDbl(dr, "BudgetTolerance"),
                            Completed = dr.GetBoolean(dr.GetOrdinal("Completed")),
                            Created = dr.GetDateTime(dr.GetOrdinal("Created")),
                            Modified = dr.GetDateTime(dr.GetOrdinal("Created")),
                            DueDate = GetSafeDate(dr, "DueDate"),
                            Exclude = dr.GetBoolean(dr.GetOrdinal("Exclude")),
                            Id = quoteRequestElementid,
                            LeadWeeks = GetSafeInt(dr, "LeadWeeks"),
                            Quantity = dr.GetInt32(dr.GetOrdinal("Quantity")),
                            Type = new QuoteElementType
                            {
                                Name = dr.GetString(dr.GetOrdinal("QuoteElementName")),
                                Notes = GetSafeString(dr, "QuoteElementNotes")
                            },
                            QuoteRequestElementId = quoteRequestElementid,
                            QuoteId = dr.GetGuid(dr.GetOrdinal("QuoteId"))
                        };
                }
            }

            return ret;
        }

        public bool LoginUser(User loginUser)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "USER_LoginUser"))
                {
                    cmd.Parameters.AddWithValue("EmailAddress", loginUser.PrimaryEmail);
                    cmd.Parameters.AddWithValue("Password", loginUser.CurrentPassword);
                    cmd.Parameters.Add("return", SqlDbType.Int);
                    cmd.Parameters["return"].Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    var ret = int.Parse(cmd.Parameters["return"].Value.ToString() ?? throw new InvalidOperationException());

                    if (ret != 0)
                    {
                        loginUser.Id = ret;
                        return true;
                    }

                    return false;
                }
            }
        }

        public User LoadUser(User loginUser)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAvailabilityResult> CheckUserAvailability(IEnumerable<DateTime> proposedDates, long userId)
        {
            var list = new List<IAvailabilityResult>();

            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "USER_CheckAvailability"))
                {
                    cmd.Parameters.AddWithValue("userId", userId);
                    cmd.Parameters.AddWithValue("proposedDate", DateTime.Today);
                    cmd.Parameters.Add("return", SqlDbType.Int);
                    cmd.Parameters["return"].Direction = ParameterDirection.ReturnValue;

                    foreach (var proposedDate in proposedDates)
                    {
                        cmd.Parameters["proposedDate"].Value = proposedDate;
                        cmd.ExecuteNonQuery();

                        var ret = int.Parse(cmd.Parameters["return"].Value.ToString() ?? throw new InvalidOperationException());

                        if (ret == 0)
                            list.Add(new AvailabilityResult {ProposedDate = proposedDate, Available = false});
                        else
                            list.Add(new AvailabilityResult {ProposedDate = proposedDate, Available = true});
                    }
                }
            }

            return list;
        }

        public void AddNonAvailability(DateTime @from, DateTime to, int userId)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "CALENDAR_AddNonAvailability"))
                {
                    cmd.Parameters.AddWithValue("userId", userId);
                    cmd.Parameters.AddWithValue("dateTimeFrom", @from);
                    cmd.Parameters.AddWithValue("dateTimeTo", to);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUserMetaData(User user)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "USER_UpdateMetaData"))
                {
                    cmd.Parameters.AddWithValue("userId", user.Id);
                    cmd.Parameters.AddWithValue("name", string.Empty);
                    cmd.Parameters.AddWithValue("value", string.Empty);
                    cmd.Parameters.AddWithValue("type", string.Empty);
                    foreach (var key in user.MetaData.Keys)
                    {
                        if(string.IsNullOrEmpty(user.MetaData[key].Value))
                            continue;
                        cmd.Parameters["name"].Value = user.MetaData[key].Name;
                        cmd.Parameters["value"].Value = user.MetaData[key].Value;
                        cmd.Parameters["type"].Value = user.MetaData[key].Type.ToString();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public IEnumerable<QuoteTemplate> GetQuoteTemplates(int? id = null)
        {
            var ret = new List<QuoteTemplate>();
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "QUOTE_GetQuoteTemplates"))
                {
                    if (id.HasValue) cmd.Parameters.AddWithValue("templateID", id);

                    var dr = cmd.ExecuteReader();

                    var templateId = 0;
                    QuoteTemplate template = null;
                    while (dr.Read())
                    {
                        var newTemplateid = dr.GetInt32(dr.GetOrdinal("TemplateId"));

                        if (newTemplateid != templateId)
                        {
                            if (template != null) ret.Add(template);

                            templateId = newTemplateid;
                            template = new QuoteTemplate
                            {
                                Id = templateId,
                                Created = dr.GetDateTime(dr.GetOrdinal("TemplateCreated")),
                                Modified = dr.GetDateTime(dr.GetOrdinal("TemplateModified")),
                                Name = dr.GetString(dr.GetOrdinal("TemplateName")),
                                Notes = GetSafeString(dr, "TemplateNotes"),
                                Link = GetSafeString(dr, "TemplateNotes"),
                                Type = new QuoteType
                                {
                                    Id = dr.GetInt32(dr.GetOrdinal("QuoteTypeid")),
                                    Name = dr.GetString(dr.GetOrdinal("QuoteTypeName")),
                                    Notes = GetSafeString(dr, "QuoteTypeNotes"),
                                    Link = GetSafeString(dr, "QuoteTypeLink"),
                                    Modified = dr.GetDateTime(dr.GetOrdinal("QuoteTypeModified")),
                                    Created = dr.GetDateTime(dr.GetOrdinal("QuoteTypeCreated"))
                                },
                                SubType = new QuoteSubType
                                {
                                    Id = dr.GetInt32(dr.GetOrdinal("QuoteSubTypeid")),
                                    Name = dr.GetString(dr.GetOrdinal("QuoteSubTypeName")),
                                    Notes = GetSafeString(dr, "QuoteSubTypeNotes"),
                                    Link = GetSafeString(dr, "QuoteSubTypeLink"),
                                    Modified = dr.GetDateTime(dr.GetOrdinal("QuoteSubTypeModified")),
                                    Created = dr.GetDateTime(dr.GetOrdinal("QuoteSubTypeCreated"))
                                }
                            };
                        }

                        template?.Events.Add(new QuoteTemplateEvent
                        {
                            Id = dr.GetInt32(dr.GetOrdinal("QuoteTemplateEventId")),
                            Created = dr.GetDateTime(dr.GetOrdinal("EventCreated")),
                            Modified = dr.GetDateTime(dr.GetOrdinal("EventModified")),
                            Order = dr.GetInt32(dr.GetOrdinal("EventOrder")),
                            Event = new Event
                            {
                                Name = dr.GetString("EventName"),
                                Link = GetSafeString(dr, "EventLink"),
                                Created = dr.GetDateTime(dr.GetOrdinal("EventCreated")),
                                Modified = dr.GetDateTime(dr.GetOrdinal("EventModified"))
                            }
                        });
                    }

                    ret.Add(template);
                }
            }

            return ret;
        }

        public QuoteTemplateEvent GetQuoteTemplateEvent(int quoteTemplateEventId)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "QUOTE_LoadQuoteTemplateEvent"))
                {
                    cmd.Parameters.AddWithValue("quoteTemplateEventId", quoteTemplateEventId);

                    var dr = cmd.ExecuteReader();
                    var quoteRequestEventId = 0;
                    QuoteTemplateEvent ret = null;

                    while (dr.Read())
                    {
                        if (dr.GetInt32(dr.GetOrdinal("QuoteTemplateEventId")) != quoteRequestEventId)
                        {
                            quoteRequestEventId = dr.GetInt32(dr.GetOrdinal("QuoteTemplateEventId"));
                            ret = new QuoteTemplateEvent
                            {
                                Id = dr.GetInt32(dr.GetOrdinal("QuoteTemplateEventId")),
                                Event = new Event
                                {
                                    Name = dr.GetString(dr.GetOrdinal("EventName")),
                                    Link = GetSafeString(dr, "EventLink"),
                                    Notes = GetSafeString(dr, "EventNotes").Replace("\n", "<br>"),
                                    Created = dr.GetDateTime(dr.GetOrdinal("EventCreated")),
                                    Modified = dr.GetDateTime(dr.GetOrdinal("EventModified"))
                                },
                                Created = dr.GetDateTime(dr.GetOrdinal("EventCreated")),
                                Modified = dr.GetDateTime(dr.GetOrdinal("EventModified"))
                            };
                        }

                        if (dr["ElementName"] != DBNull.Value)
                            ret?.TemplateElements.Add(new QuoteElement
                            {
                                Name = dr.GetString(dr.GetOrdinal("ElementName")),
                                Notes = GetSafeString(dr, "ElementNotes"),
                                Id = dr.GetInt32(dr.GetOrdinal("ElementId"))
                            });
                    }

                    return ret;
                }
            }
        }

        public IEnumerable<Event> ListEvents(int? eventId = null)
        {
            var ret = new List<Event>();
            
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "EVENTS_List"))
                {
                    if (eventId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("eventId", eventId.Value);
                    }
                    
                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        var evt = new Event()
                        {
                            Name = dr.GetString(dr.GetOrdinal("Name")),
                            Id = dr.GetInt32(dr.GetOrdinal("Id")),
                            Notes = GetSafeString(dr, "Notes"),
                            Link = GetSafeString(dr, "Link"),
                            Created = dr.GetDateTime(dr.GetOrdinal("Created")),
                            Modified = dr.GetDateTime(dr.GetOrdinal("Modified")),
                        };

                        if (eventId.HasValue)
                        {
                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    var subType = new QuoteSubType
                                    {
                                        Id = dr.GetInt32(dr.GetOrdinal("SubTypeId")),
                                        Name = dr.GetString(dr.GetOrdinal("Name")),
                                        Notes = GetSafeString(dr, "Notes"),
                                        Link = GetSafeString(dr, "Link"),
                                        Created = dr.GetDateTime((dr.GetOrdinal("Created"))),
                                        Modified = dr.GetDateTime((dr.GetOrdinal("Modified"))),
                                    };
                                    evt.SubTypes.Add(subType);
                                }
                            }
                        }
                        
                        ret.Add(evt);
                    }
                }
            }

            return ret;
        }

        public Event SaveEvent(Event eventIn)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "EVENTS_Save"))
                {
                    cmd.Parameters.AddWithValue("name", eventIn.Name);
                    cmd.Parameters.AddWithValue("notes", eventIn.Notes);
                    cmd.Parameters.AddWithValue("link", eventIn.Link);
                    cmd.Parameters.AddWithValue("id", eventIn.Id);
                    cmd.Parameters.Add("return", SqlDbType.Int);
                    cmd.Parameters["return"].Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    int ret = int.Parse(cmd.Parameters["return"].Value.ToString() ?? string.Empty);

                    return ListEvents(ret).First();
                }
            }
        }

        public IList<QuoteSubType> ListSubTypes()
        {
            var ret = new List<QuoteSubType>();
            
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = GetCommand(cn, "EVENTS_ListSubTypes"))
                {
                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        var subType = new QuoteSubType
                        {
                            Id = dr.GetInt32(dr.GetOrdinal("Id")),
                            Name = dr.GetString(dr.GetOrdinal("Name")),
                            Link = GetSafeString(dr, "Link"),
                            Notes = GetSafeString(dr, "Notes"),
                            Created = dr.GetDateTime(dr.GetOrdinal("Created")),
                            Modified = dr.GetDateTime(dr.GetOrdinal("Modified")),
                        };
                        
                        ret.Add(subType);
                    }
                }
            }

            return ret;
        }
    }
}