using System;
using System.Data;
using System.Data.SqlClient;
using EventaDors.Entities.Classes;
using NUnit.Framework;

namespace EventaDors.Test
{
    public class UserTests
    {
        private SqlConnection cn;
        [SetUp]
        public void Setup()
        {
            var connectionString = "Server=localhost;Database=EventaDors;User Id=sa;Password=pb6eNW&eE%w%;";
            cn = new SqlConnection(connectionString);
            cn.Open();
        }

        [TearDown]
        public void TearDown()
        {
            cn.Close();
        }

        [Test]
        public void ShouldBuildUser()
        {
            SqlCommand cmd = new SqlCommand("dbo.GetUser", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("userId", SqlDbType.BigInt);
            cmd.Parameters["userId"].Value = 1;
            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var uuid = dr.GetGuid(dr.GetOrdinal("uuid"));
                User user = new User(
                    dr.GetString(dr.GetOrdinal("UserName")),
                    dr.GetInt64(dr.GetOrdinal("id")),
                    dr.GetDateTime(dr.GetOrdinal("Created")),
                    dr.GetDateTime(dr.GetOrdinal("Modified")),
                    uuid
                );
                
                Assert.NotNull(user.UserName);
            }
        }
    }
}