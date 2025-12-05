using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;

namespace FlowerShopApp.Data
{
    public static class DbConnectionHelper
    {
        public static NpgsqlConnection GetConnection()
        {
            string connString = ConfigurationManager
                .ConnectionStrings["PostgresConnection"]
                .ConnectionString;

            return new NpgsqlConnection(connString);
        }
    }
}