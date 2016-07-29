using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;

namespace AccesoDatos
{
    public class SQLite : GDatos
    {


        public override sealed string CadenaConexion
        {
            get
            {
                if (MCadenaConexion.Length == 0)
                {
                  //  if (MBase.Length != 0 && MServidor.Length != 0)
                 //   {
                        var sCadena = new System.Text.StringBuilder("");
                        sCadena.Append("data source=<SERVIDOR>;");
                       /* sCadena.Append("initial catalog=<BASE>;");
                        sCadena.Append("user id=<USER>;");
                        sCadena.Append("password=<PASSWORD>;");
                        sCadena.Append("persist security info=True;");
                        sCadena.Append("user id=sa;packet size=4096");
                */        
                        sCadena.Replace("<SERVIDOR>", Servidor);
                /*
                        sCadena.Replace("<BASE>", Base);
                        sCadena.Replace("<USER>", Usuario);
                        sCadena.Replace("<PASSWORD>", Password);
                        
            */
                       // return sCadena.ToString();
                   // }
                    //throw new Exception("No se puede establecer la cadena de conexión en la clase DatosSQLServer");
                }
                return MCadenaConexion = CadenaConexion;

            }// end get
            set
            { MCadenaConexion = value; } // end set
        }// end CadenaConexion

        protected override System.Data.IDbCommand ComandoSql(string comandoSql)
        {
            var com = new System.Data.SQLite.SQLiteCommand(comandoSql, (System.Data.SQLite.SQLiteConnection)Conexion, (System.Data.SQLite.SQLiteTransaction)MTransaccion);
            return com;
        }// end Comando

        /* 
         * Luego implementaremos CrearConexion, donde simplemente se devuelve una nueva instancia del 
         * objeto Conexión de SQLite, utilizando la cadena de conexión del objeto.
         */
        protected override System.Data.IDbConnection CrearConexion(string cadenaConexion)
        { return new System.Data.SQLite.SQLiteConnection(cadenaConexion); }


        //Finalmente, es el turno de definir CrearDataAdapter, el cual aprovecha el método Comando para crear el comando necesario.
        protected override System.Data.IDataAdapter CrearDataAdapterSql(string comandoSql)
        {
            var da = new System.Data.SQLite.SQLiteDataAdapter((System.Data.SQLite.SQLiteCommand)ComandoSql(comandoSql));
            return da;
        } // end CrearDataAdapterSql

        /*
         * Definiremos dos constructores especializados, uno que reciba como argumentos los valores de Nombre del Servidor 
         * y de base de datos que son necesarios para acceder a datos, y otro que admita directamente la cadena de conexión completa.
         * Los constructores se definen como procedimientos públicos de nombre New.
         */
        public SQLite()
        {
            Base = "";
            Servidor = "";
            Usuario = "";
            Password = "";
        }// end DatosSQLServer


        public SQLite(string servidor)
        {
            Servidor = servidor;
        }// end DatosSQLServer

    }// end class DatosSQLServer
}
