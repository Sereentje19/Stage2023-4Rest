using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace Back_end
{
    public class UserRepository : BaseRepository
    {
        public string GetName()
        {
            string query = "SELECT email FROM users WHERE id = @id";

            SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@id", 1)
                };

            return query;
            // return ReadTables(ExecuteSelectQuery(query ,sqlParameters));
        }

        // private Staff ReadTables(DataTable dataTable)
        // {
        //     Staff staff = new Staff();

        //     foreach (DataRow dr in dataTable.Rows)
        //     {
        //         Staff login = new Staff()
        //         {
        //             StaffId = (int)dr["StaffId"],
        //             Password = (int)dr["Password"],
        //             Specialty = (string)dr["Specialty"],
        //             Name = (string)dr["Name"]
        //         };
        //         return login;
        //     }
        //     return staff;
        // }

    }
}
