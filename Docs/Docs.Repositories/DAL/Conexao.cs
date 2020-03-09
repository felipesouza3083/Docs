using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docs.Repositories.DAL
{
    public class Conexao
    {
        protected MySqlConnection con;
        protected MySqlCommand cmd;
        protected MySqlDataReader dr;
        protected MySqlTransaction tr;

        protected void OpenConnection()
        {
            con = new MySqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            con.Open();
        }

        protected void CloseConnection()
        {
            con.Close();
        }
    }
}
