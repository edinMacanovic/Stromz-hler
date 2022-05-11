using System.Data.SqlClient;

namespace _65_WPF_Stromzähler
{
    public class Connection
    {
        public SqlConnection ConnectionDb()
        {
            var con = new SqlConnection(@"Data Source = (localDb)\MSSQLLocalDb;" +
                                        "Initial Catalog = WPF_Stromzähler;" +
                                        "Integrated Security = sspi");
            con.Open();
            return con;
        }
    }
}