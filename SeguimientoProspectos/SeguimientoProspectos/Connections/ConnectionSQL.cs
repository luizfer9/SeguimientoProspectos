using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeguimientoProspectos.Connections
{
    class ConnectionSQL
    {
        public string connectionString, ip, db, user, pass;
        private int cont;

        public string readFile(string file)
        {
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    String line;
                    cont = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        cont += 1;
                        switch (cont)
                        {
                            case 1:
                                ip = line;
                                break;
                            case 2:
                                db = line;
                                break;
                            case 3:
                                user = line;
                                break;
                            case 4:
                                pass = line;
                                break;
                        }
                    }
                    sr.Close();
                }
                connectionString = "Data Source=" + ip + ";Initial Catalog=" + db + ";User ID=" + user + ";Password=" + pass;
                return connectionString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return connectionString;
            }
            
        }
        //conexion
        private SqlConnection con = new SqlConnection();
        private SqlConnection sqlCon
        {
            get { return con; }
            set { con = value; }
        }
        //comando
        private SqlCommand comm = new SqlCommand();
        private SqlCommand sqlComm
        {
            get { return comm; }
            set { comm = value; }
        }
        //adaptador
        private SqlDataAdapter da = new SqlDataAdapter();
        private SqlDataAdapter Da
        {
            get { return da; }
            set { da = value; }
        }
        //toma la conexion dl txt
        private void conectar()
        {
            sqlCon.ConnectionString = connectionString;
        }
        //Abre la conexion
        private void ConexionAbrir()
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                conectar();
                sqlCon.Open();
            }
        }
        //Cierra la conexion
        private void ConexionCerrar()
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
        //llena un data table y lo pasa como referencia
        public DataTable LLenaDateTable(ref DataTable dtTable, string Sentencia)
        {
            try
            {
                ConexionAbrir();

                sqlComm.CommandType = CommandType.Text;
                sqlComm.CommandText = Sentencia;
                sqlComm.CommandTimeout = 0000;
                Da.SelectCommand = sqlComm;
                Da.SelectCommand.Connection = con;
                Da.SelectCommand.CommandTimeout = 0000;
                Da.Fill(dtTable);
                return dtTable;
            }
            catch (Exception ex) { throw ex; }
            finally { ConexionCerrar(); }
        }
        public void BulkCopy(DataTable dtTable, string Tabla)
        {
            conectar();
            SqlConnection conn = new SqlConnection();
            conn = con;
            SqlTransaction tran;
            SqlBulkCopy bulkcopy;

            if (dtTable.Rows.Count > 0)
            {

                conn.Open();
                tran = conn.BeginTransaction();

                bulkcopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran);
                bulkcopy.BatchSize = 25000;
                bulkcopy.BulkCopyTimeout = 0;
                bulkcopy.DestinationTableName = "dbo." + Tabla;

                try
                {
                    bulkcopy.WriteToServer(dtTable);
                    tran.Commit();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }
        }
        public void Execute(string sentencia)
        {
            try
            {
                ConexionAbrir();
                sqlComm.Connection = con;
                sqlComm.CommandType = CommandType.Text;
                sqlComm.CommandText = sentencia;
                sqlComm.CommandTimeout = 0;
                sqlComm.ExecuteNonQuery();
            }
            catch (Exception ex) { throw ex; }
            finally { ConexionCerrar(); }

        }
        public Int32 ExecuteScalarInt(string sentencia)
        {
            try
            {
                Int32 iReturn = 0;
                ConexionAbrir();
                sqlComm.Connection = con;
                sqlComm.CommandType = CommandType.Text;
                sqlComm.CommandText = sentencia;
                sqlComm.CommandTimeout = 0;
                iReturn = Convert.ToInt32(sqlComm.ExecuteScalar().ToString());
                return iReturn;
            }
            catch (Exception ex) { throw ex; }
            finally { ConexionCerrar(); }

        }
        public string ExecuteScalarString(string sentencia)
        {
            try
            {
                string iReturn = "";
                ConexionAbrir();
                sqlComm.Connection = con;
                sqlComm.CommandType = CommandType.Text;
                sqlComm.CommandText = sentencia;
                sqlComm.CommandTimeout = 0;
                iReturn = sqlComm.ExecuteScalar().ToString();
                return iReturn;
            }
            catch (Exception ex) { throw ex; }
            finally { ConexionCerrar(); }
        }
    }
}
