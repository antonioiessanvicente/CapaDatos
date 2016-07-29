using System;
using System.Data;
 
namespace AccesoDatos
{
    public abstract class GDatos
    {
        #region "Declaración de Variables"
 
        protected string MServidor = "";  // Servidor
        protected string MBase = ""; // Base de datos
        protected string MUsuario = ""; // Usuario
        protected string MPassword = ""; // Contraseña
        protected string MCadenaConexion = ""; // Cadena de conexión
        protected IDbConnection MConexion; // La conexión
 
        #endregion
 
        #region "Setters y Getters"
 
        // Nombre del equipo servidor de datos. 
        public string Servidor
        {
            get { return MServidor; }
            set { MServidor = value; }
        } // end Servidor
 
        // Nombre de la base de datos a utilizar.
        public string Base
        {
            get { return MBase; }
            set { MBase = value; }
        } // end Base
 
        // Nombre del Usuario de la BD. 
        public string Usuario
        {
            get { return MUsuario; }
            set { MUsuario = value; }
        } // end Usuario
 
        // Password del Usuario de la BD. 
        public string Password
        {
            get { return MPassword; }
            set { MPassword = value; }
        } // end Password
 
        // Cadena de conexión completa a la base. // Se debe implementar en las clases derivadas
        public abstract string CadenaConexion
        { get; set; }
 
        #endregion
 
        #region "Privadas"
 
        // Crea u obtiene un objeto para conectarse a la base de datos. 
        protected IDbConnection Conexion
        {
            get
            {
                // si aun no tiene asignada la cadena de conexion lo hace
                if (MConexion == null)
                    MConexion = CrearConexion(CadenaConexion);
 
                // si no esta abierta aun la conexion, lo abre
                if (MConexion.State != ConnectionState.Open)
                    MConexion.Open();
 
                // retorna la conexion en modo interfaz, para que se adapte a cualquier implementacion de los distintos fabricantes de motores de bases de datos
                return MConexion;
            } // end get
        } // end Conexion
 
        #endregion
 
        #region "Obtener datos de la BD"
 
        // Obtiene un DataSet a partir de un Query Sql.
        public DataSet TraerDataSetSql(string comandoSql)
        {
            var mDataSet = new DataSet();
            CrearDataAdapterSql(comandoSql).Fill(mDataSet);
            return mDataSet;
        } // end TraerDataSetSql
 
        //Obtiene un DataTable a partir de un Query SQL
        public DataTable TraerDataTableSql(string comandoSql)
        { return TraerDataSetSql(comandoSql).Tables[0].Copy(); } // end TraerDataTableSql
 
        // Obtiene un DataReader a partir de una sql
        public IDataReader TraerDataReaderSql(string comandoSql)
        {
            var com = ComandoSql(comandoSql);
            return com.ExecuteReader();
        } // end TraerDataReaderSql  

        // Obtiene un Valor de una funcion Escalar a partir de un Query SQL
        public object TraerValorEscalarSql(string comandoSql)
        {
            var com = ComandoSql(comandoSql);
            return com.ExecuteScalar();
        } // end TraerValorEscalarSql
 
        #endregion
 
        #region "Acciones"
        protected abstract IDbConnection CrearConexion(string cadena);
        //protected abstract IDbCommand Comando(string procedimientoAlmacenado);
        protected abstract IDbCommand ComandoSql(string comandoSql);
        //protected abstract IDataAdapter CrearDataAdapter(string procedimientoAlmacenado, params Object[] args);
        protected abstract IDataAdapter CrearDataAdapterSql(string comandoSql);
        //protected abstract void CargarParametros(IDbCommand comando, Object[] args);
 
        // metodo sobrecargado para autenticarse contra el motor de BBDD
        public bool Autentificar()
        {
            if (Conexion.State != ConnectionState.Open)
                Conexion.Open();
            return true;
        }// end Autenticar
 
        // metodo sobrecargado para autenticarse contra el motor de BBDD
        public bool Autentificar(string vUsuario, string vPassword)
        {
            MUsuario = vUsuario;
            MPassword = vPassword;
            MConexion = CrearConexion(CadenaConexion);
 
            MConexion.Open();
            return true;
        }// end Autenticar
 
 
        // cerrar conexion
        public void CerrarConexion()
        {
            if (Conexion.State != ConnectionState.Closed)
                MConexion.Close();
        }
 
        // end CerrarConexion
 
 
        // Ejecuta un query sql
        public int EjecutarSql(string comandoSql)
        { return ComandoSql(comandoSql).ExecuteNonQuery(); } // end Ejecutar

        #endregion
 
        #region "Transacciones"
 
        protected IDbTransaction MTransaccion;
        protected bool EnTransaccion;
 
        //Comienza una Transacción en la base en uso. 
        public void IniciarTransaccion()
        {
            try
            {
                MTransaccion = Conexion.BeginTransaction();
                EnTransaccion = true;
            }// end try
            finally
            { EnTransaccion = false; }
        }// end IniciarTransaccion
 
 
        //Confirma la transacción activa. 
        public void TerminarTransaccion()
        {
            try
            { MTransaccion.Commit(); }
            finally
            {
                MTransaccion = null;
                EnTransaccion = false;
            }// end finally
        }// end TerminarTransaccion
 
 
        //Cancela la transacción activa.
        public void AbortarTransaccion()
        {
            try
            { MTransaccion.Rollback(); }
            finally
            {
                MTransaccion = null;
                EnTransaccion = false;
            }// end finally
        }// end AbortarTransaccion
 
 
        #endregion
 
    }// end class gDatos
}// end namespace