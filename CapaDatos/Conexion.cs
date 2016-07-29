namespace AccesoDatos
{
    public class Conexion
    {
        public static GDatos GDatos;
        // iniciar sesión contra un SQLServer
        public static bool IniciarSesion(string nombreServidor, string baseDatos, string usuario, string password)
        {
            GDatos = new SqlServer(nombreServidor, baseDatos, usuario, password);
            return GDatos.Autentificar();
        } //fin inicializa sesion

        //Iniciar Sesion contra in SQLite
        public static bool IniciarSesionSQLite( string baseDatos)
        {
            GDatos = new SQLite(baseDatos);
            return GDatos.Autentificar();
        } //fin inicializa sesion

        public static void FinalizarSesion()
        {
            GDatos.CerrarConexion();
        } //fin FinalizaSesion
 
    }
}