using MySql.Data.MySqlClient;

namespace C1_Project.Models
{
    public class SchoolDbContext
    {
      
        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "School"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }

      
        protected static string ConnectionString
        {
            get
            {
                return $"server={Server};user={User};database={Database};port={Port};password={Password};convert zero datetime=True";
            }
        }

        /// <summary>
        /// Returns a connection to the School database.
        /// </summary>
        /// <returns>A MySqlConnection Object</returns>

        public MySqlConnection AccessDatabase()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
