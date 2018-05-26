using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Central_LED.DAL
{
    public class DbConnection
    {

        public OleDbConnection con = new OleDbConnection();
        public OleDbCommand cmd = new OleDbCommand();
        public OleDbDataAdapter adp = new OleDbDataAdapter();

        public DbConnection()
        {
            try
            {
                string databaspath = System.IO.Directory.GetCurrentDirectory();
                con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+databaspath+"\\database\\database.accdb;Jet OLEDB:Database Password=Admin@123";
            }
            catch (OleDbException sqlex)
            {
                MessageBox.Show(sqlex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Open_Connection()
        {
            try
            {
                con.Open();
            }
            catch (OleDbException sqlex)
            {
                MessageBox.Show(sqlex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Close_Connection()
        {
            con.Close();
        }

    }
}
