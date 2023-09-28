using System;
using Back_end.Models;
using Microsoft.EntityFrameworkCore;


namespace Back_end.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly NotificationContext _context;
        private readonly DbSet<User> _dbSet;

        public LoginRepository(NotificationContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public User checkCredentials(User user)
        {
            List<User> users = _dbSet.ToList();

            foreach (User u in users)
            {
                if (u.Email == user.Email && u.Password == user.Password)
                {
                    return u;
                }
                else if (u.Email == user.Email)
                {
                    throw new Exception("Wachtwoord is incorrect!");
                }
            }
            
            throw new Exception("Email is incorrect!");
        }
    }
}