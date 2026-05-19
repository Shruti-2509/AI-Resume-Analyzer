using MySql.Data.MySqlClient;

namespace ResumeAnalyzer.Data
{
    public class DbConnection
    {
        private static string connectionString =
            "server=localhost;user=root;password=root;database=resume_analyzer;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}