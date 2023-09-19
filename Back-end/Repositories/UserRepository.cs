using System;
using Back_end.Models;
using Microsoft.EntityFrameworkCore;


namespace Back_end.Repositories
{
    public class UserRepository : IUserRepository
    {
         private readonly NotificationContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(NotificationContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public User GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Add(User entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

    }
}