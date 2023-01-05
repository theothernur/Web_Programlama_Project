using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace StudentsDatabase.Helper
{
    public class CommonHelper
    {
        private IConfiguration _config;
        
            public CommonHelper(IConfiguration config)
        {
            _config = config;
        }

        public int DMLTransaction(string Query)
        {
            int Result;
            string connectionString = _config["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = Query;
                SqlCommand com = new SqlCommand(sql, connection);
                Result = com.ExecuteNonQuery();
                connection.Close();
            }
            return Result;

        }



        public bool UserAlreadyExists(string query)
        {
            bool flag = false;
            string connectionString = _config["ConnectionStrings:DefaultConnection"];

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = query;
                SqlCommand com = new SqlCommand(sql, connection);
                SqlDataReader rd = com.ExecuteReader();
                if(rd.HasRows)
                {
                    flag = true;
                }
                connection.Close();
            }
            return flag;
        }


    }
}
