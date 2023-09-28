using System;
using Back_end.Models;

namespace Back_end.Repositories
{
    public interface ILoginRepository
    {
        User checkCredentials(User user);
    }
}