using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using Back_end.Models;

namespace Back_end.Repositories
{
    public class Documentrepository : BaseRepository
    {

        private readonly NotificationContext _context;
        private readonly DbSet<T> _dbSet;

        public Documentrepository(NotificationContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Post(Document document)
        {
            // conn.Open();



            SqlCommand command = new SqlCommand("INSERT INTO Documents " +
            "(DocumentId, Image, Date, CustomerId, UserId, Type)" +
            "VALUES (@DocumentId, @Image, @Date, @CustomerId, @UserId, @Type)");

            command.Parameters.AddWithValue("@DocumentId", 1);
            command.Parameters.AddWithValue("@Image", "1");
            command.Parameters.AddWithValue("@Date", new DateTime(2020, 02, 02));
            command.Parameters.AddWithValue("@CustomerId", 1);
            command.Parameters.AddWithValue("@UserId", 1);
            command.Parameters.AddWithValue("@Type", "1");

            command.ExecuteNonQuery();
        }
    }
}