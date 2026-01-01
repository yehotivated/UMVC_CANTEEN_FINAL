using MySql.Data.MySqlClient;

namespace UMVC_INVENTORY
{
    class DBConnection
    {
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(
                "server=localhost;" +
                "database=canteen_db;" +
                "uid=root;" +
                "pwd=joshua;"
            );
        }
    }
}
