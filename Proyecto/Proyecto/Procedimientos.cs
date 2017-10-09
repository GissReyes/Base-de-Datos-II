using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;


namespace Proyecto
{
    public partial class Procedimientos
    {
        
        Conexión cn;
        OracleCommand Comando;
        public int llenarTabla(string procedimiento, OracleParameter[] param)
        {
            int resultado = 0;
            cn = new Conexión();
            try
            {
                OracleConnection con = new OracleConnection(cn.cadena);
                con.InfoMessage += new OracleInfoMessageEventHandler(con_InfoMessage);
                con.Open();
                Comando = new OracleCommand(procedimiento, con);
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.CommandText = procedimiento;
                OracleDataAdapter da = new OracleDataAdapter(Comando);
                for (int x = 0; x < (param.Length); x++)
                    Comando.Parameters.Add(param[x]);
                    resultado = Comando.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message, ex.ErrorCode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return resultado;
        }
        public DataTable llenar_DataTable(string procedimiento)
        {
            cn = new Conexión();
            DataTable dt = new DataTable();
            try
            {
                OracleConnection conex = new OracleConnection(cn.cadena);
                conex.Open();
                Comando = new OracleCommand(procedimiento, conex);
                Comando.CommandType = CommandType.StoredProcedure;
                OracleDataAdapter da = new OracleDataAdapter(Comando);
                da.Fill(dt);
                da.Dispose();
                conex.Close();
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message, ex.ErrorCode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }
        public DataTable llenar_DataTable(string procedimiento, OracleParameter[] param)
        {
            cn = new Conexión();
            DataTable dt = new DataTable();
            try
            {
                OracleConnection conex = new OracleConnection(cn.cadena);
                conex.Open();
                Comando = new OracleCommand();
            Comando.Connection = conex;
            Comando.CommandText = procedimiento;
                Comando.CommandType = CommandType.StoredProcedure;
                for (int x = 0; x < (param.Length); x++)
                    Comando.Parameters.Add(param[x]).Direction = ParameterDirection.Output;
                OracleDataAdapter da = new OracleDataAdapter(Comando);
            da.SelectCommand = Comando;
                da.Fill(dt);
                da.Dispose();
                conex.Close();
        }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al intentar extraer los datos.\n" + ex.Message);
            }
            return dt;
        }
        public DataSet llenar_DataSet(string procedimiento, OracleParameter[] param, String ptabla)
        {
            cn = new Conexión();
            DataSet ds = new DataSet();
            try
            {
                OracleConnection conex = new OracleConnection(cn.cadena);
                conex.InfoMessage += new OracleInfoMessageEventHandler(con_InfoMessage);
                conex.Open();
                Comando = new OracleCommand();
                Comando.Connection = conex;
            Comando.CommandText = procedimiento;
                Comando.CommandType = CommandType.StoredProcedure;
                OracleDataAdapter da = new OracleDataAdapter(Comando);
                for (int x = 0; x < (param.Length - 1); x++)
                {
                    Comando.Parameters.Add(param[x]);
                }
                    Comando.Parameters.Add(param[param.Length-1]).Direction = ParameterDirection.Output;
                int registro = da.Fill(ds, ptabla);
            da.SelectCommand = Comando;
                da.Dispose();
                conex.Close();
        }
            catch (OracleException ex)
            {
                MessageBox.Show("Ha ocurrido un error al intentar extraer los datos.\n" + ex.Message.ToString());
            }
            return ds;
        }
        public void con_InfoMessage(object sender, OracleInfoMessageEventArgs e)
        {
            String mensaje = "";
            if (e.Errors.Count > 0)
            {
                mensaje = e.Errors[0].Message;
            }
      //      Globales.gbError = mensaje;
        }
        
    }
}
