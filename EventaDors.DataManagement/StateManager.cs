using System.Data;
using System.Data.SqlClient;

namespace EventaDors.DataManagement
{
    public class StateManager
    {
        private readonly string _connectionString;

        public StateManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public bool PutState(string stateKey, string key, string value)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("STATE_PutKey", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("stateKey", stateKey);
                    cmd.Parameters.AddWithValue("key", key);
                    cmd.Parameters.AddWithValue("value", value);
                    
                    cn.Open();

                    if (cmd.ExecuteNonQuery() != 0)
                        return true;
                    return false;
                }
            }
        }

        public string GetState(string stateKey, string key)
        {
            using (var cn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("STATE_GetKey", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("stateKey", stateKey);
                    cmd.Parameters.AddWithValue("key", key);

                    cn.Open();

                    var ret = cmd.ExecuteScalar();

                    if (ret != null)
                        return ret.ToString();
                    return string.Empty;
                }
            }
        }
    }
}